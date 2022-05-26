namespace AgentCustomer.Files
{
    public interface ICustomerNotificationService {
        Task CustomerNotification(long agentId, long customerId, List<CustomerFileType> processedTypes);
    }
}
