using System;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetActiveCartRequest
    {
        public GetActiveCartRequest() : this("", "", "") { }

        public GetActiveCartRequest(string language, string visitorId, string currency)
        { 
            this.Language = language;

            this.VisitorId = visitorId;

            this.Currency = currency;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
