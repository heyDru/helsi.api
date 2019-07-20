using System;
using System.Collections.Generic;
using System.Text;
using Common.Enums;
using Nest;

namespace Common.Models.SearchModels
{
    [ElasticsearchType(RelationName = "patient")]
    public class PatientSearchDocument
    {
        [Text(Name = "userid")]
        public string UserId { get; set; }

        [Text(Name = "firstName")]
        public string FirstName { get; set; }

        [Text(Name = "lastName")]
        public string LastName { get; set; }

        [Text(Name = "birthday")]
        public DateTime Birthday { get; set; }

        [Text(Name = "gender")]
        public Gender Gender { get; set; }

        [Text(Name = "phone")]
        public string Phone { get; set; }

        [Text(Name = "activated")]
        public bool Activated { get; set; }
    }
}