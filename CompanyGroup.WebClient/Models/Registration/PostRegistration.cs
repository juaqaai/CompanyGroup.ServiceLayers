using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class PostRegistration
    {
        public PostRegistration(CompanyGroup.Dto.ServiceResponse.PostRegistration registration)
        {
            this.Message = registration.Message;

            this.Successed = registration.Successed;
        }

        public PostRegistration() : this(new CompanyGroup.Dto.ServiceResponse.PostRegistration()) { }

        /// <summary>
        /// sikeres volt-e a művelet, vagy nem
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        public string Message { get; set; }
    }
}