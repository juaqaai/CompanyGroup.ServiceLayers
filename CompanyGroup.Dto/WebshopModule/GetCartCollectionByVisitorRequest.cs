using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    public class GetCartCollectionByVisitorRequest
    {
        public GetCartCollectionByVisitorRequest() : this("", "") { }

        public GetCartCollectionByVisitorRequest(string language, string visitorId)
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
