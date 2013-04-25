using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kapcsolattartó módosítás
    /// </summary>
    public class UpdateContactPerson
    {
        public UpdateContactPerson() : this(String.Empty, String.Empty, new CompanyGroup.Dto.RegistrationModule.ContactPerson(), String.Empty){}

        public UpdateContactPerson(string registrationId, string visitorId, CompanyGroup.Dto.RegistrationModule.ContactPerson contactPerson, string languageId)
        {
            this.RegistrationId = registrationId;

            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.ContactPerson = contactPerson;
        }

        public string RegistrationId { get; set; }

        public string LanguageId { get; set; }

        public CompanyGroup.Dto.RegistrationModule.ContactPerson ContactPerson { get; set; }

        public string VisitorId { get; set; }
    }
}
