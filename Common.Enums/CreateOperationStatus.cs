using System.ComponentModel;

namespace Common.Enums
{
    public enum CreateOperationStatus
    {
        [Description("Patient has been created")]
        Ok = 0,
        [Description("Patient with phone <PHONE> already exists")]
        PatientExists = 1,
        [Description("An error occurred")]
        Failed = 2
    }
}
