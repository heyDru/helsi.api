using System.ComponentModel;

namespace Common.Enums
{
    public enum SearchOperationStatus
    {
        [Description("No patients found")]
        NotFound = 0,
        [Description("Patients found")]
        Success = 1,
        [Description("Search request is incorrect")]
        IncorrectRequest = 2
    }
}
