using System;
using Common.Enums;

namespace Data.Models
{
    public class Patient
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public string Phone { get; set; }

        public bool Activated { get; set; }
    }
}
