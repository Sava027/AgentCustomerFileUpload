
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentCustomer.FileDataAccess
{
    public class BaseEntity
    {
        public DateTime DateCreated { get; set; }
        public string UserCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string UserUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }   
        public string? UserDeleted { get; set; }
        public bool IsDeleted { get; set; }
      
    }
}
