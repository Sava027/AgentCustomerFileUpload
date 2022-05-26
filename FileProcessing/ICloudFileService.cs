using Microsoft.AspNetCore.Http;

namespace CloudFileProcessing
{
    public interface ICloudFileService
    {
        Task<string> SaveFileToCloud(IFormFile file);
    }
}
