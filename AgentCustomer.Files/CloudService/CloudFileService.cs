using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace AgentCustomer.Files
{
    /// <summary>
    /// This really would not be a class but a connector to some cloud storage account
    /// </summary>
    public class CloudFileService : ICloudFileService
    {
        private readonly ILogger _logger;
        public CloudFileService(ILogger<CloudFileService> logger)
        {
            _logger = logger;
        }

        public async Task<string> SaveFileToCloud(IFileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                _logger.LogTrace($"File was no found at {fileInfo.FullName}");
                return null;
            }
            return fileInfo.FullName;
        }
    }
}
