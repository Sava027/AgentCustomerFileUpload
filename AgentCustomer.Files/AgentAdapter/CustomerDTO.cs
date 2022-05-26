
namespace AgentCustomer.Files.AgentAdapter
{
    public class CustomerDTO
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> RequiredDocumentTypes { get; set; }
    }
}
