using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AgentCustomer.Files.AgentAdapter
{
    public class AgentAdapter : IAgentAdapter
    {
        private string _agentAPIUrl;
        private readonly IHttpClientWrapper _clientWrapper;
        private readonly ILogger _logger;
        public AgentAdapter(IConfiguration configuration, IHttpClientWrapper clientWrapper, ILogger<AgentAdapter> logger)
        {
            _clientWrapper = clientWrapper;
            _logger = logger;
            _agentAPIUrl = "https://localhost:7229/Agent";

        }
        public async Task<AgentDTO> GetAgent(string id)
        {            
            string urlMethod = $"/{id}";
            var requestMessage = _clientWrapper.GetRequestMessage(HttpMethod.Get, $"{_agentAPIUrl}{urlMethod}");
            using (var response = await _clientWrapper.SendAsync(requestMessage))
            {

                string content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogTrace($"Failed Response: {response.StatusCode}::{response.ReasonPhrase}:: {content}");

                    throw new Exception($"Failed Response: {response.StatusCode}::{response.ReasonPhrase}:: {content}");
                }

                var result = JsonConvert.DeserializeObject<AgentDTO>(content);
                return result;
            }

            return null;
        }
    }
}

