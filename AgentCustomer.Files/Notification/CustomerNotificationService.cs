
using AgentCustomer.Files.AgentAdapter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AgentCustomer.Files
{
    public class CustomerNotificationService : ICustomerNotificationService
    {
         
        private readonly IAgentAdapter _agentAdapter;    
        private readonly ICustomerFileValidator _validator;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;

        public CustomerNotificationService(IAgentAdapter agentAdapter, IEmailService emailService,
             ICustomerFileValidator validator, ILogger<CustomerFileService> logger)
        {
           
            _agentAdapter = agentAdapter;          
            _validator = validator;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task CustomerNotification(long agentId, long customerId, List<CustomerFileType> processedTypes)
        {
            var agent = await _agentAdapter.GetAgent(agentId.ToString());

            if (!_validator.IsAgentCustomerInfoValid(agent, customerId))
                throw new Exception("The provided agent or customer identifier are not valid for uploading files."); 

            var customer = agent.Customers.Single(x => x.CustomerId == customerId);

            if (!_validator.CheckSendMessageNeeded(customer, processedTypes))
                return;

            _emailService.SendNotification(customer.Email, "Your Documents are ready", "<a href='link goes here' />");

        }         
    }
}
