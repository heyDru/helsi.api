using System;
using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Data.Models
{
    public class Patient
    {
        [Key]
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public bool Activated { get; set; }
    }
}
