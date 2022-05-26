using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentCustomer.FileDataAccess
{
    public class FileTracking:BaseEntity
    {
        public long FileTrackingId { get; set; }
        public string PulicTrackingId { get; set; }
        public int FileCount   { get; set; }        
        public string Status { get; set; }

        public string TempLocationPath { get; set; }
    }
}
