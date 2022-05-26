
using System.ComponentModel;

namespace AgentCustomer.Files
{
    public enum FileTrackingInfoStatus
    {
        [Description("New")]
        New,
        [Description("In Progress")]
        InProgress,
        [Description("Failed")]
        Failed,
        [Description("Completed")]
        Completed

    }
}
