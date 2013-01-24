using System;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetActiveCartRequest
    {
        public GetActiveCartRequest() : this("", "") { }

        public GetActiveCartRequest(string language, string visitorId)
        { 
            Language = language;

            VisitorId = visitorId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
