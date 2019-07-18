using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Enums
{
    public enum PatientActivatingStatus
    {
        [Description("Patient Activated")]
        Activated = 1,
        [Description("Patient Not Activated")]
        NotActive = 2,
        [Description("Patient Diactivated")]
        Diactivated = 3,
        [Description("Patient not found")]
        NotFound = 4
    }
}
