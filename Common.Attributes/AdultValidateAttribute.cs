using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Attributes
{
    public class AdultValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            var birthDate = Convert.ToDateTime(date);
            var today = DateTime.Now;
            var expectedDate = today.AddYears(-18);

            return birthDate <= expectedDate;
        }
    }
}
