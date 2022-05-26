using Agent.API.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Agent.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger _logger;
        public AgentsController(ILogger<AgentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(200, "Get Agent By Id (with Customers)", typeof(AgentResponse))]
        public async Task<IActionResult> GetAgent(string id)
        {
            try
            {
                if (!int.TryParse(id, out int agentId))
                    return StatusCode((int)HttpStatusCode.InternalServerError, $"You must provide an Agent Identifier");

                var agent = TestDataAgents.GetTestData(agentId);

                return Ok(agent);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Agent with Id {id} could not be retrieved.");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An Error occurred: {ex.Message}");
            }
        }
    }
}