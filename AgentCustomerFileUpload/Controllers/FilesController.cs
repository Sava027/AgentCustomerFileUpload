using AgentCustomer.Files;
using AgentCustomer.FileUpload;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace AgentCustomerFileUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly ICustomerFileService _fileService;

        public FilesController(ICustomerFileService fileService, ILogger<FilesController> logger)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost]
        [Route("{agentId}/{customerId}/Upload")]
        [SwaggerResponse(200, "Agent Customer File Upload", typeof())]
        public async Task<IActionResult> Upload(long agentId, long customerId, List<IFormFile> files)
        {
            try
            {
                var trackingId = await _fileService.CustomerFileUpload(agentId, customerId, files);

                // this call would be replaced by a back end service like Azure fn
                _fileService.ProcessFiles(agentId, customerId, trackingId);

                return Ok(new FileUploadResponse(trackingId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Agent {agentId} File Upload for Customer {customerId} failed.");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An Error occurred: {ex.Message}");
            }
        }
    }
}