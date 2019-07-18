using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Enums
{
    public enum Gender
    {
        [Description("Man")]
        Man = 1,

        [Description("Woman")]
        Woman = 2,

        [Description("Other")]
        Other = 3
    }
}
