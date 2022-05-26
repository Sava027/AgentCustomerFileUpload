
namespace AgentCustomer.FileDataAccess
{
    public class FileInfo:BaseEntity
    {
        public long FileInfoId { get; set; }
        public string CustomerId { get; set; }
        public string AgentId { get; set; }
        public string FileTrackingId { get; set; }
        public string FileType { get; set; }      
        public string FileName { get; set; }
        public string Path { get; set; }               

    }
}
