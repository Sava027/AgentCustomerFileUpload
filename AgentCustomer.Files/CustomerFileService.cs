
using AgentCustomer.Files.AgentAdapter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AgentCustomer.Files
{
    public class CustomerFileService : ICustomerFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IAgentAdapter _agentAdapter;
        private readonly IFileService _fileService;
        private readonly ICloudFileService _cloudFileService;
        private readonly ICustomerFileValidator _validator;
        private readonly ICustomerNotificationService _customerNotificationService;
        private readonly ILogger _logger;

        public CustomerFileService(IAgentAdapter agentAdapter, IFileService fileService, ICloudFileService cloudFileService,
            IFileRepository fileRepository, ICustomerFileValidator validator, ICustomerNotificationService customerNotificationService,
            ILogger<CustomerFileService> logger)
        {
            _fileRepository = fileRepository;
            _agentAdapter = agentAdapter;
            _fileService = fileService;
            _cloudFileService = cloudFileService;
            _customerNotificationService = customerNotificationService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<string> CustomerFileUpload(long agentId, long customerId, List<IFormFile> files)
        {
            var agent = await _agentAdapter.GetAgent(agentId.ToString());

            if (!_validator.IsAgentCustomerInfoValid(agent, customerId))
                throw new Exception("The provided agent or customer identifier are not valid for uploading files."); 


            var publicId = $"Track-{CreatePublicId(agentId.ToString(), customerId.ToString())}";

            var tempPath = _fileService.GetTemporaryDirectory(publicId);

            var trackInfo = new TrackingInfo(publicId, files.Count, tempPath);

            _fileRepository.AddTracking(trackInfo);
            await _fileRepository.SaveChangesAsync();

            var tempFileSave = await _fileService.SaveFilesToTemporary(tempPath, files);

            if (tempFileSave == null)
                throw new Exception("The files were not able to be uploaded."); 

            return publicId;

        }
       
        public async Task<IEnumerable<CustomerFileType>> ProcessFiles(long agentId, long customerId, string trackingId)
        {

            if (string.IsNullOrEmpty(trackingId))
                return null;

            var trackingInfo = await _fileRepository.GetTracking(trackingId);
            if (trackingInfo == null)
                return null;

            var files = _fileService.GetFiles(trackingInfo.TempLocationPath);
            if (files == null | !files.Any())
            {
                await _fileRepository.UpdateTrackingStatus(trackingInfo.Id, FileTrackingInfoStatus.Failed);
                return null;

            }
            var result = new List<CustomerFileType>();
            await _fileRepository.UpdateTrackingStatus(trackingInfo.Id, FileTrackingInfoStatus.InProgress);
            foreach (var file in files)
            {
                var newPath = await _cloudFileService.SaveFileToCloud(file);

                var fileNameNoExtension = file.Name.Replace(Path.GetExtension(file.Name), "");
                var type = EnumExtension.GetValueFromDescription<CustomerFileType>(fileNameNoExtension);

                var customerFile = new CustomerFile(0, customerId.ToString(), agentId.ToString(), trackingId, type, newPath);
                _fileRepository.AddFileInfo(customerFile);
                await _fileRepository.SaveChangesAsync();

                _fileService.DeleteFile(file.FullName);
                result.Add(type);

            }
            await _fileRepository.UpdateTrackingStatus(trackingInfo.Id, FileTrackingInfoStatus.Completed);

            _customerNotificationService.CustomerNotification(agentId, customerId, result);

            return result;
        }

        private string CreatePublicId(string agentid, string customerId)
        {

            return $"{agentid}-{customerId}-{DateTime.Now.ToFileTimeUtc()}";

        }
    }
}
