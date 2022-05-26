
namespace AgentCustomer.Files.AgentAdapter
{
    public class AgentDTO
    {
        public long AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentStatus { get; set; }
        public string Email { get; set; }

        public IEnumerable<CustomerDTO> Customers { get; set; }
    }
}
