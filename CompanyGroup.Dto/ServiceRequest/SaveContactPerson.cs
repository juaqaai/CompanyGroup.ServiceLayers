using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kapcsolattartó mentése
    /// </summary>
    public class SaveContactPerson
    {
        public SaveContactPerson(string registrationId, string languageId, CompanyGroup.Dto.RegistrationModule.ContactPerson contactPerson, string visitorId)
        {
            this.RegistrationId = registrationId;

            this.LanguageId = languageId;

            this.ContactPerson = contactPerson;

            this.VisitorId = visitorId;
        }

        public string RegistrationId { get; set; }

        public string LanguageId { get; set; }

        public CompanyGroup.Dto.RegistrationModule.ContactPerson ContactPerson { get; set; }

        public string VisitorId { get; set; }
    }
}
