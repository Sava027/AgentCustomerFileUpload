using AgentCustomer.Files;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO.Abstractions;

namespace AgentCustomer.Tests
{
    public class FileServiceTests
    {

        readonly Mock<IFileSystem> _fileSystem;
        readonly Mock<ILogger<FileService>> _logger;

        public FileServiceTests()
        {

            _logger = new Mock<ILogger<FileService>>();
            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.Directory.CreateDirectory(It.IsAny<String>())).Verifiable();
            _fileSystem.Setup(f => f.FileStream.Create(It.IsAny<String>(), FileMode.Create)).Verifiable();

        }

        [Fact]
        public async Task SaveFilesToTemporary_AllSuccess()
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

            var fileMock2 = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content2 = "Hello World from a Fake File";
            var fileName2 = "test2-Car.pdf";
            writer.Write(content2);
            writer.Flush();
            ms.Position = 0;
            fileMock2.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock2.Setup(m => m.FileName).Returns(fileName2);
            fileMock2.Setup(m => m.Length).Returns(ms.Length);
            list.Add(fileMock2.Object);

            var fileProcessorMock = new Mock<IFileProcessor>();
            fileProcessorMock.Setup(m => m.ProcessFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync(FileTrackingStatus.Success);

            var fileService = new FileService(fileProcessorMock.Object, _fileSystem.Object, _logger.Object);

            var result = await fileService.SaveFilesToTemporary("", list);

            result.Count.Should().Be(2);
            result.All(x => x.Value == FileTrackingStatus.Success).Should().BeTrue();

        }

        [Fact]
        public async Task SaveFilesToTemporary_ThrowsNoFilesException()
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
            fileMock.Setup(m => m.Length).Returns(0);
            list.Add(fileMock.Object);



            var fileProcessorMock = new Mock<IFileProcessor>();
            fileProcessorMock.Setup(m => m.ProcessFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync(FileTrackingStatus.Success);

            var fileService = new FileService(fileProcessorMock.Object, _fileSystem.Object, _logger.Object);

            Func<Task> act = () => fileService.SaveFilesToTemporary("", list);

            await act.Should().ThrowAsync<Exception>().WithMessage("There are no files in the provided list.");

        }


        [Fact]
        public async Task GetFiles_ReturnsFiles()
        {
            var _path = "test\\tests";
            var fileInfoMock = new Mock<IFileInfo>();

            _fileSystem.Setup(f => f.Directory.Exists(_path)).Returns(true);
            _fileSystem.Setup(f => f.Directory.GetFiles(_path)).Returns(new string[] { "File1", "File2" });

            _fileSystem.Setup(f => f.FileInfo.FromFileName("File1")).Returns(fileInfoMock.Object);
            _fileSystem.Setup(f => f.FileInfo.FromFileName("File2")).Returns(fileInfoMock.Object);


            var fileProcessorMock = new Mock<IFileProcessor>();
            fileProcessorMock.Setup(m => m.ProcessFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync(FileTrackingStatus.Success);

            var fileService = new FileService(fileProcessorMock.Object, _fileSystem.Object, _logger.Object);

            var result = fileService.GetFiles(_path);

            result.Count().Should().Be(2);

        }

    }
}