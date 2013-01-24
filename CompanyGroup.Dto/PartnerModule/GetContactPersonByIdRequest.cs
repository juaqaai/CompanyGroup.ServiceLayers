using System;

namespace CompanyGroup.Dto.PartnerModule
{
    public class GetContactPersonByIdRequest
    {
        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        public GetContactPersonByIdRequest(string visitorId, string languageId)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;
        }

        public GetContactPersonByIdRequest() : this(String.Empty, String.Empty) { }
    }
}
