using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceResponse
{
    /// <summary>
    /// válasz objektum osztály
    /// </summary>
    public class PostRegistration
    {
        /// <summary>
        /// sikeres volt-e a művelet, vagy nem
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        public string Message { get; set; }

        public PostRegistration(bool successed, string message)
        {
            this.Successed = successed;

            this.Message = message;
        }

        public PostRegistration() : this(false, "") { }
    }
}
