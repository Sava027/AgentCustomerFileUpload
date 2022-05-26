
namespace AgentCustomer.Files
{
    public class TrackingInfo
    {
        public TrackingInfo() { }

        public TrackingInfo(string trackingid, int fileCount, string path) {
                      
            PulicTrackingId = trackingid;
            FileCount = fileCount;
            Status = FileTrackingInfoStatus.New;
            TempLocationPath = path;
        }

        public long Id { get; set; }
        public string PulicTrackingId { get; set; }
        public int FileCount { get; set; }
        public FileTrackingInfoStatus Status { get; set; }

        public string TempLocationPath { get; set; }

    }
}
