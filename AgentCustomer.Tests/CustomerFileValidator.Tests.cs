using AgentCustomer.Files;
using AgentCustomer.Files.AgentAdapter;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AgentCustomer.Tests
{
    public class CustomerFileValidatorTests
    {

        readonly Mock<ILogger<CustomerFileValidator>> _logger;
        public CustomerFileValidatorTests()
        {
            _logger = new Mock<ILogger<CustomerFileValidator>>();
        }

        [Fact]
        public void IsAgentCustomerInfoValid_Valid()
        {
            var agent = new AgentDTO()
            {
                AgentId = 2,
                AgentStatus = "Active",
                AgentName = "TestAgent",
                Customers = new List<CustomerDTO>() {
            new CustomerDTO(){ CustomerId =2, Status= "Activce", Email= "Test@email.com", RequiredDocumentTypes = new []{"Car" }}
            }
            };

            var customerValidator = new CustomerFileValidator(_logger.Object);
            var result = customerValidator.IsAgentCustomerInfoValid(agent, 2);

            result.Should().BeTrue();

        }

        [Fact]
        public void IsAgentCustomerInfoValid_NotValid()
        {

            var agent = new AgentDTO()
            {
                AgentId = 2,
                AgentStatus = "Inactive",
                AgentName = "TestAgent",
                Customers = new List<CustomerDTO>() {
            new CustomerDTO(){ CustomerId =2, Status= "Activce", Email= "Test@email.com", RequiredDocumentTypes = new []{"Car" }}
            }
            };

            var customerValidator = new CustomerFileValidator(_logger.Object);
            var result = customerValidator.IsAgentCustomerInfoValid(agent, 2);

            result.Should().BeFalse();

        }
    }
}
