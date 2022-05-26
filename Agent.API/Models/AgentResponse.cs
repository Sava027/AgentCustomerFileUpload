namespace Agent.API
{
    public class AgentResponse
    {

        public AgentResponse(long id, string name, string status, IEnumerable<CustomerResponse> customers)
        {

            AgentId = id;
            AgentName = name;
            AgentStatus = status;
            Email = "Sava027@yahoo.de";
            Customers = customers;
        }

        public long AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentStatus { get; set; }
        public string Email { get; set; }

        public IEnumerable<CustomerResponse> Customers { get; set; }
    }

    public class CustomerResponse
    {

        private static readonly string[] RequiredDocuments = new[]
        {
        "Car", "Liability"
        };
        public CustomerResponse(long id, string name, string status)
        {

            CustomerId = id;
            Name = name;
            Status = status;
            Email = "Test@test.com";
            RequiredDocumentTypes = RequiredDocuments.ToList();

        }
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> RequiredDocumentTypes { get; set; }

    }
}
