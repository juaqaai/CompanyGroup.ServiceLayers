using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetActiveCart
    {
        public GetActiveCart() : this("", "") { }

        public GetActiveCart(string language, string visitorId)
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
