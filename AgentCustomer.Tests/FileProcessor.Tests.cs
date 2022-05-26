using AgentCustomer.Files;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO.Abstractions;

namespace AgentCustomer.Tests
{
    public class FileProcessorTests
    {
        readonly Mock<IFileSystem> _fileSystem;
        readonly Mock<ILogger<FileProcessor>> _logger;
        public FileProcessorTests()
        {

            _logger = new Mock<ILogger<FileProcessor>>();
            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.Directory.CreateDirectory(It.IsAny<String>())).Verifiable(); 
            _fileSystem.Setup(f => f.FileStream.Create(It.IsAny<String>(), FileMode.Create)).Verifiable();             
        }

        [Fact]
        public async Task ProcessFile_StatusSuccess()
        {
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

            var fileProcessor = new FileProcessor(_fileSystem.Object, _logger.Object);

           var status = await fileProcessor.ProcessFile("", fileMock.Object);

            status.Should().Be(FileTrackingStatus.Success);

        }

        [Fact]
        public async Task ProcessFile_FileExtensionInvalid_StatusFailed()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test-Car.xsx";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.FileName).Returns(fileName);
            fileMock.Setup(m => m.Length).Returns(ms.Length);

            var fileProcessor = new FileProcessor(_fileSystem.Object, _logger.Object);

            var status = await fileProcessor.ProcessFile("", fileMock.Object);

            status.Should().Be(FileTrackingStatus.Failed);

        }

        [Fact]
        public async Task ProcessFile_FileTypeUnkown_StatusFailed()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.FileName).Returns(fileName);
            fileMock.Setup(m => m.Length).Returns(ms.Length);

            var fileProcessor = new FileProcessor(_fileSystem.Object, _logger.Object);

            var status = await fileProcessor.ProcessFile("", fileMock.Object);

            status.Should().Be(FileTrackingStatus.Failed);

        }
    }
}