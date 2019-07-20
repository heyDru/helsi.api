using System;
using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.Enums;
using Nest;

namespace Services.DtoModels
{
    public class PatientDto
    {
        public string UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [AdultValidate(ErrorMessage = "Age must be greater than 18")]
        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [PhoneValidate(ErrorMessage = "Incorrect phone format")]
        public string Phone { get; set; }

        public bool Activated { get; set; }
    }
}
