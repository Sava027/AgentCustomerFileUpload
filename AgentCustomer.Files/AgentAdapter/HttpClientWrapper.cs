
namespace AgentCustomer.Files.AgentAdapter
{
    public interface IHttpClientWrapper
    {
        HttpRequestMessage GetRequestMessage(HttpMethod method, string requestURI, HttpContent content = null);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
    }
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private static HttpClient _client;

        public HttpRequestMessage GetRequestMessage(HttpMethod method, string requestURI, HttpContent content = null)
        {
            var message = new HttpRequestMessage(method, requestURI);
            message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (content != null)
            {
                message.Content = content;
            }
            return message;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }
            return await _client.SendAsync(message);
        }
    }
}
