
namespace AgentCustomer.Files
{
    public interface IFileRepository
    {
        public void AddTracking(TrackingInfo info);

        public Task UpdateTrackingStatus(long id, FileTrackingInfoStatus fileTrackingStatus);

        public void AddFileInfo(CustomerFile customerFile);

        public Task<IEnumerable<CustomerFile>> GetFilesByAgentAndCustomer(string agentId, string customerId);

        public Task<TrackingInfo> GetTracking(string pulicId);
        public Task<int> SaveChangesAsync();

    }
}
