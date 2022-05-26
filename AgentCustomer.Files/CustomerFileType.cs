
using System.ComponentModel;

namespace AgentCustomer.Files
{
    public enum CustomerFileType
    {
        [Description("Unknown")]
        Unknown,
        [Description("Car")]
        CarInsurancePolicy,
        [Description("Home Owners")]
        HomeOwnersPolicy,
        [Description("Liability")]
        LiabilityPolicy 
    }
}
