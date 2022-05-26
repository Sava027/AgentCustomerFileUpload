using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CloudFileProcessing
{
    public class CloudFileService : ICloudFileService
    {
        private readonly ILogger _logger;
        public CloudFileService(ILogger<CloudFileService> logger)
        {
            _logger = logger;
        }

        public async Task<string> SaveFileToCloud(IFormFile file)
        {                        
            return file.FileName;
        }
    }
}
