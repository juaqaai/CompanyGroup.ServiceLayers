using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ApplyForJobRequest
    {
        public string Position { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PlaceOfBirth { get; set; }

        public string DayfBirth { get; set; }

        public string PermanentAddress { get; set; }

        public string TemporaryAddress { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string UploadFileName { get; set; }

        public bool CheckReference { get; set; }
    }
}