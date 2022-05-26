
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace AgentCustomer.Files
{
    public class FileService : IFileService
    {
        private readonly ILogger _logger;
        private readonly IFileProcessor _fileProcessor;
        private readonly IFileSystem _fileSystem;
        public FileService( IFileProcessor fileProcessor,   IFileSystem fileSystem, ILogger<FileService> logger)
        {
            _logger = logger;
            _fileProcessor = fileProcessor;
            _fileSystem = fileSystem;
        }

        public string GetTemporaryDirectory(string publicId)
        {
            return $"{Path.GetTempPath()}\\{publicId}";
        }
        
        public async Task<Dictionary<string, FileTrackingStatus>> SaveFilesToTemporary(string path, List<IFormFile> files)
        {
            if (files.Sum(f => f.Length) == 0)
            {
                _logger.LogTrace("File List was empty");
                throw new Exception("There are no files in the provided list."); //return null;
            }

            var result = new Dictionary<string, FileTrackingStatus>();
            foreach (var formFile in files)
            {             
                result.Add(formFile.FileName, await _fileProcessor.ProcessFile(path, formFile));
            }
            return result;
        }

        public IEnumerable<IFileInfo> GetFiles(string path)
        {
            var fileInfos = new List<IFileInfo>();
            if (!_fileSystem.Directory.Exists(path))
            {
                _logger.LogTrace($"Directory with path  {path} was not found.");
                return null;
            }

            foreach (var fileName in _fileSystem.Directory.GetFiles(path))
            {
                fileInfos.Add(_fileSystem.FileInfo.FromFileName(fileName));
            }
            return fileInfos;
        }
        public void DeleteFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (_fileSystem.File.Exists(path))
            {
                _fileSystem.File.Delete(path);
            }
        }
    
    }
}
