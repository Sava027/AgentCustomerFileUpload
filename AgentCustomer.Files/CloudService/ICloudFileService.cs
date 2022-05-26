using System.IO.Abstractions;

namespace AgentCustomer.Files
{
    public interface ICloudFileService
    {
        Task<string> SaveFileToCloud(IFileInfo fileInfo);
    }
}
