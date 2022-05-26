
using Microsoft.AspNetCore.Http;
using System.IO.Abstractions;

namespace AgentCustomer.Files
{
    public interface IFileService
    {
        string GetTemporaryDirectory(string publicId);
        Task<Dictionary<string, FileTrackingStatus>> SaveFilesToTemporary(string path, List<IFormFile> files);

        IEnumerable<IFileInfo> GetFiles(string path);

        void DeleteFile(string path);
    }
}
