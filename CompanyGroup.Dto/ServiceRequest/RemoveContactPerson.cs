using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{

    public class RemoveContactPerson
    {
        public RemoveContactPerson() : this(String.Empty, String.Empty, String.Empty, String.Empty) { }

        public RemoveContactPerson(string registrationId, string visitorId, string languageId, string id)
        {
            this.RegistrationId = registrationId;

            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.Id = id;
        }

        public string RegistrationId { get; set; }

        public string LanguageId { get; set; }

        public string Id { get; set; }

        public string VisitorId { get; set; }
    }
}
