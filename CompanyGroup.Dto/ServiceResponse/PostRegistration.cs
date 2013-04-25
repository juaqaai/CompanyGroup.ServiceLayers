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
        public bool Succeeded { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// vevőregisztráció azonosító (ERP)
        /// </summary>
        public string CustomerRegistrationId { get; set; }

        public PostRegistration(bool succeeded, string message, string customerRegistrationId)
        {
            this.Succeeded = succeeded;

            this.Message = message;

            this.CustomerRegistrationId = customerRegistrationId;
        }

        public PostRegistration() : this(false, String.Empty, String.Empty) { }
    }
}
