using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// webadminisztrátor adatainak módosítása
    /// </summary>
    public class UpdateWebAdministrator
    {
        /// <summary>
        /// regisztrációs azonosító
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// webadminisztrátor, (kapcsolattartó)
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.WebAdministrator WebAdministrator { get; set; }

        /// <summary>
        /// látogató azonosítója    
        /// </summary>
        public string VisitorId { get; set; }
    }
}
