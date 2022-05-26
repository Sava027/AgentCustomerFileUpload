using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace AgentCustomer.Files
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly string[] _supportedFileExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" }; // read from app settings /DB
        private readonly int _maxFileSize = 5242880; // read from app settings
        public FileProcessor(IFileSystem fileSystem, ILogger<FileProcessor> logger)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public async Task<FileTrackingStatus> ProcessFile(string path, IFormFile formFile)
        {

            var fileType = FileValidation(formFile);

            if (string.IsNullOrEmpty(fileType))
            {
                return FileTrackingStatus.Failed;
            }

            var fileName = $"{fileType}{Path.GetExtension(formFile.FileName)}";
            var filePath = $"{path}\\{fileName}";

            CheckDirectoryExistsOrAdd(path);          

            using (var stream = _fileSystem.FileStream.Create(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return FileTrackingStatus.Success;
        }

        private bool CheckDirectoryExistsOrAdd(string path)
        {
            if (!_fileSystem.Directory.Exists(path))
            {
                _fileSystem.Directory.CreateDirectory(path);
            }

            return true;
        }
        private string FileValidation(IFormFile formFile)
        {

            if (formFile.Length == 0)
            {
                _logger.LogTrace($"File with name {formFile.FileName} was empty");
                return string.Empty;
            }

            if (formFile.Length > _maxFileSize)
            {
                _logger.LogTrace($"File Size ({formFile.Length}) for {formFile.FileName} was too big.");
                return string.Empty;
            }

            var extension = Path.GetExtension(formFile.FileName);
            if (!_supportedFileExtensions.Contains(extension.ToLower()))
            {
                _logger.LogTrace($"File Extension ({extension}) for {formFile.FileName} is not allowed ");
                return string.Empty;
            }

            var fileType = ReadFileType(formFile);

            if (fileType == CustomerFileType.Unknown)
            {
                _logger.LogTrace($"File Type for {formFile.FileName} coud not be parsed.");
                return string.Empty;
            }
            return fileType.GetDescription();
        }
        private CustomerFileType ReadFileType(IFormFile file)
        {           
            if (file.Headers != null && file.Headers.ContainsKey("CustomerFileType"))
            {
                var type = file.Headers.FirstOrDefault(x => x.Key == "CustomerFileType").Value;
                return EnumExtension.GetValueFromDescription<CustomerFileType>(type, CustomerFileType.Unknown);
            }

            var extension = Path.GetExtension(file.FileName);
            var fileNameParsed = file.FileName.Replace(extension, "");

            if (fileNameParsed.Contains('-'))
            {
                fileNameParsed = fileNameParsed.Substring(fileNameParsed.LastIndexOf('-') + 1);
            }

            return EnumExtension.GetValueFromDescription<CustomerFileType>(fileNameParsed, CustomerFileType.Unknown);



        }
    }
}
