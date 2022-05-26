using AgentCustomer.Files.AgentAdapter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentCustomer.Files
{
    public class CustomerFileValidator : ICustomerFileValidator
    {
        private readonly ILogger _logger;
        public CustomerFileValidator(ILogger<CustomerFileValidator> logger)
        {
            _logger = logger;
        }
        public bool CheckSendMessageNeeded(CustomerDTO customer, IEnumerable<CustomerFileType> processdTypes)
        {
            if (customer == null)
                return false;

            var missingDocs = customer.RequiredDocumentTypes.Where(p => !processdTypes.Any(x => x.GetDescription() == p));

            if (missingDocs.Any())
                return false;

            return true;

        }

        public bool IsAgentCustomerInfoValid(AgentDTO agent, long customerId)
        {
            if (agent == null)
            {
                _logger.LogTrace($"Agent with was not found.");
                return false;
            }

            if (agent.AgentStatus != PeopleStatus.Active.GetDescription())
            {
                _logger.LogTrace($"Agent with Id {agent.AgentId} is not active.");
                return false;
            }

            if (agent.Customers == null || !agent.Customers.Any())
            {
                _logger.LogTrace($"Agent with Id {agent.AgentId} has no customers.");
                return false;
            }
            var customer = agent.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
            {
                _logger.LogTrace($"Agent with Id {agent.AgentId} has no customer with id {customerId}.");
                return false;
            }
            return true;

        }
    }
}
