namespace AgentCustomer.Files
{
    public class CustomerFile
    {
        public CustomerFile() { }
        public CustomerFile(long id, string customerid, string agentid, string trackingid, CustomerFileType customerFileType, string path)
        {
            Id = id;
            CustomerId = customerid;
            AgentId = agentid;
            TrackingId = trackingid;
            FileName = path.Substring(path.LastIndexOf('/') + 1);
            Path = path;
            FileType = customerFileType;
        }

        public long Id { get; set; }
        public string CustomerId { get; set; }
        public string AgentId { get; set; }
        public string TrackingId { get; set; }
        public CustomerFileType FileType { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime UploadedDate { get; set; }

    }
}