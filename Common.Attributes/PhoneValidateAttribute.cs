using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Attributes
{
    public class PhoneValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object incomePhone)
        {
            var phone = Convert.ToString(incomePhone);

            return Regex.Match(phone, @"^(\+380[0-9]{9})$").Success;
        }
    }
}