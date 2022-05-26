using Microsoft.AspNetCore.Http;

namespace AgentCustomer.Files
{
    public interface IFileProcessor {
        Task<FileTrackingStatus> ProcessFile(string path, IFormFile formFile);
    }
}
