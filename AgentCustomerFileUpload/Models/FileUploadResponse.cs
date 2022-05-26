namespace AgentCustomer.FileUpload
{
    public class FileUploadResponse
    {
        public FileUploadResponse(string trackingId)
        {
            TrackingId = trackingId;
        }

        public string TrackingId { get; set; }
    }
}
