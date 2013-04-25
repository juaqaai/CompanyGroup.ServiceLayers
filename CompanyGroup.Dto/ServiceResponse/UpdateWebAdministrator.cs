using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceResponse
{
    /// <summary>
    /// webadminisztrátor osztály
    /// </summary>
    public class UpdateWebAdministrator
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
        /// látogató
        /// </summary>
        public CompanyGroup.Dto.PartnerModule.Visitor Visitor { get; set; }
    }
}
