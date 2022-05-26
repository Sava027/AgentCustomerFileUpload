using AgentCustomer.Files;
using AgentCustomer.Files.AgentAdapter;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO.Abstractions;

namespace AgentCustomer.Tests
{
    public class CustomerFileServiceTests
    {

        readonly Mock<ILogger<CustomerFileService>> _logger;

        public CustomerFileServiceTests()
        {
            _logger = new Mock<ILogger<CustomerFileService>>();
        }

        [Fact]
        public async Task CustomerFileUpload_PublicId()
        {

            var list = new List<IFormFile>();
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test-Car.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.FileName).Returns(fileName);
            fileMock.Setup(m => m.Length).Returns(ms.Length);
            list.Add(fileMock.Object);


            var agent = new AgentDTO()
            {
                AgentId = 2,
                AgentStatus = "Active",
                AgentName = "TestAgent",
                Customers = new List<CustomerDTO>() {
            new CustomerDTO(){ CustomerId =2, Status= "Activce", Email= "Test@email.com", RequiredDocumentTypes = new []{"Car" }}
            }
            };

            var agentAdapterMock = new Mock<IAgentAdapter>();
            agentAdapterMock.Setup(m => m.GetAgent("2")).ReturnsAsync(agent);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.GetTemporaryDirectory(It.IsAny<string>())).Returns(string.Empty);

            fileServiceMock.Setup(m => m.SaveFilesToTemporary(string.Empty, list)).ReturnsAsync(new Dictionary<string, FileTrackingStatus>());


            var fileRepoMock = new Mock<IFileRepository>();
            fileRepoMock.Setup(m => m.AddTracking(It.IsAny<TrackingInfo>())).Verifiable();
            fileRepoMock.Setup(m => m.SaveChangesAsync()).Verifiable();

            var customerValidatorMock = new Mock<ICustomerFileValidator>();
            customerValidatorMock.Setup(m => m.IsAgentCustomerInfoValid(agent, 2)).Returns(true);

            var customerNotificationMock = new Mock<ICustomerNotificationService>();
            var cloudServiceMock = new Mock<ICloudFileService>();

            var customerFileService = new CustomerFileService(agentAdapterMock.Object, fileServiceMock.Object, cloudServiceMock.Object,
                 fileRepoMock.Object, customerValidatorMock.Object, customerNotificationMock.Object, _logger.Object);

            var result = await customerFileService.CustomerFileUpload(2, 2, list);

            result.Contains("Track-2-2").Should().BeTrue();
            fileRepoMock.Verify(m => m.AddTracking(It.IsAny<TrackingInfo>()), Times.Once());

        }

        [Fact]
        public async Task CustomerFileUpload_ThrowsInvalidAgentException()
        {

            var list = new List<IFormFile>();

            var agent = new AgentDTO()
            {
                AgentId = 2,
                AgentStatus = "Active",
                AgentName = "TestAgent",
                Customers = new List<CustomerDTO>()
            };

            var agentAdapterMock = new Mock<IAgentAdapter>();
            agentAdapterMock.Setup(m => m.GetAgent("2")).ReturnsAsync(agent);

            var fileServiceMock = new Mock<IFileService>();
            var fileRepoMock = new Mock<IFileRepository>();
            var customerNotificationMock = new Mock<ICustomerNotificationService>();
            var cloudServiceMock = new Mock<ICloudFileService>();

            var customerValidatorMock = new Mock<ICustomerFileValidator>();
            customerValidatorMock.Setup(m => m.IsAgentCustomerInfoValid(agent, 2)).Returns(false);

            var customerFileService = new CustomerFileService(agentAdapterMock.Object, fileServiceMock.Object, cloudServiceMock.Object,
                 fileRepoMock.Object, customerValidatorMock.Object, customerNotificationMock.Object, _logger.Object);

            Func<Task> act = () => customerFileService.CustomerFileUpload(2, 2, list);

            await act.Should().ThrowAsync<Exception>().WithMessage("The provided agent or customer identifier are not valid for uploading files.");


        }


        [Fact]
        public async Task ProcessFiles_CustomerFileTypeList_Returns()
        {
            var trackingId = "Track-2-2";

            var list = new List<IFormFile>();
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test-Car.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.FileName).Returns(fileName);
            fileMock.Setup(m => m.Length).Returns(ms.Length);
            list.Add(fileMock.Object);

            var fileRepoMock = new Mock<IFileRepository>();
            fileRepoMock.Setup(m => m.GetTracking(trackingId)).ReturnsAsync(new TrackingInfo() { Id = 2, TempLocationPath = "test\\test" });
            fileRepoMock.Setup(m => m.UpdateTrackingStatus(2, It.IsAny<FileTrackingInfoStatus>())).Verifiable();
            fileRepoMock.Setup(m => m.AddFileInfo(It.IsAny<CustomerFile>())).Verifiable();
            fileRepoMock.Setup(m => m.SaveChangesAsync()).Verifiable();


            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(m => m.Name).Returns("Car.pdf");
            
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.GetFiles(It.IsAny<string>())).Returns(new List<IFileInfo>() { fileInfoMock.Object });
            fileServiceMock.Setup(m => m.DeleteFile(It.IsAny<string>())).Verifiable();


            var customerValidatorMock = new Mock<ICustomerFileValidator>();
            var agentAdapterMock = new Mock<IAgentAdapter>();

            var customerNotificationMock = new Mock<ICustomerNotificationService>();
            var cloudServiceMock = new Mock<ICloudFileService>();
            cloudServiceMock.Setup(m => m.SaveFileToCloud(It.IsAny<IFileInfo>())).ReturnsAsync("test\\test\\Car.pdf");


            var customerFileService = new CustomerFileService(agentAdapterMock.Object, fileServiceMock.Object, cloudServiceMock.Object,
                 fileRepoMock.Object, customerValidatorMock.Object, customerNotificationMock.Object, _logger.Object);

            var result = await customerFileService.ProcessFiles(2, 2, trackingId);

            result.Count().Should().Be(1);
            result.First().Should().Be(CustomerFileType.CarInsurancePolicy);
            fileRepoMock.Setup(m => m.UpdateTrackingStatus(2, It.IsAny<FileTrackingInfoStatus>())).Verifiable();
            fileRepoMock.Setup(m => m.AddFileInfo(It.IsAny<CustomerFile>())).Verifiable();
            //TODO add the verify asserts

        }

    }
}