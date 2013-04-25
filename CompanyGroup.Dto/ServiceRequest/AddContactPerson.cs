using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kapcsolattartó hozzáadása kérés adatokat összefogó osztály
    /// </summary>
    public class AddContactPerson
    {
        public AddContactPerson() : this(String.Empty, String.Empty, new CompanyGroup.Dto.RegistrationModule.ContactPerson(), String.Empty) { }

        public AddContactPerson(string registrationId, string languageId, CompanyGroup.Dto.RegistrationModule.ContactPerson contactPerson, string visitorId)
        {
            this.RegistrationId = registrationId;

            this.LanguageId = languageId;

            this.ContactPerson = contactPerson;

            this.VisitorId = visitorId;
        }

        /// <summary>
        /// regisztrációs azonosító
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// beállított nyelv
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// kapcsolattartó DTO
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.ContactPerson ContactPerson { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public string VisitorId { get; set; }
    }
}
