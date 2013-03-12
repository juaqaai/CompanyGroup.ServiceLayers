using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// regisztrációs adatok mentése
    /// </summary>
    public class PostRegistration
    {
        /// <summary>
        /// regisztrációs azonosító
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// nyelv
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// látogató azonosító (üres, ha nincs belépve)
        /// </summary>
        public string VisitorId { get; set; }

        public PostRegistration(string registrationId, string languageId, string visitorId)
        { 
            this.RegistrationId = registrationId;

            this.LanguageId = languageId;

            this.VisitorId = visitorId;
        }

        public PostRegistration() : this("", "", "") { }
    }
}
