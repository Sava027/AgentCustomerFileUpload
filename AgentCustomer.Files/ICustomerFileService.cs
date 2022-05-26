using Microsoft.AspNetCore.Http;

namespace AgentCustomer.Files
{
    public interface ICustomerFileService {

        Task<string> CustomerFileUpload(long agentId, long customerId, List<IFormFile> files);
        Task<IEnumerable<CustomerFileType>> ProcessFiles(long agentId, long customerId, string trackingId);
    }
}
