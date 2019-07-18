using System.ComponentModel;

namespace Common.Enums
{
    public enum UpdateStatus
    {
        [Description("Patient has been updated")]
        Ok = 0,
        [Description("Patient doesn't exist")]
        NotFound = 1,
        [Description("An error occurred")]
        Failed = 2
    }
}
