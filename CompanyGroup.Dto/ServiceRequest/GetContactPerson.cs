using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetContactPerson
    {
        public GetContactPerson() : this(String.Empty, String.Empty, String.Empty) { }

        /// <summary>
        /// kapcsolattartók lekérdezése
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="visitorId"></param>
        /// <param name="languageId"></param>
        public GetContactPerson(string registrationId, string visitorId, string languageId)
        {
            this.RegistrationId = registrationId;

            this.VisitorId = visitorId;

            this.LanguageId = languageId;
        }

        public string RegistrationId { get; set; }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }
    }
}
