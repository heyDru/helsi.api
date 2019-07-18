using System;
using System.ComponentModel.DataAnnotations;
using Common.Attributes;
using Common.Enums;
using Nest;

namespace Services.DtoModels
{
    [ElasticsearchType(RelationName = "patient")]
    public class PatientDto
    {
        [Text(Name = "userId")]
        public string UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [Text(Name = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [Text(Name = "lastName")]
        public string LastName { get; set; }

        [Required]
        [AdultValidate(ErrorMessage = "Age must be greater than 18")]
        [Text(Name = "birthday")]
        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [Text(Name = "phone")]
        public string Phone { get; set; }

        public bool Activated { get; set; }
    }
}
