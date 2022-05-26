using AgentCustomer.Files.AgentAdapter;

namespace AgentCustomer.Files
{
    public interface ICustomerFileValidator {
        bool CheckSendMessageNeeded(CustomerDTO customer, IEnumerable<CustomerFileType> processdTypes);
        bool IsAgentCustomerInfoValid(AgentDTO agent, long customerId);
    }
}
