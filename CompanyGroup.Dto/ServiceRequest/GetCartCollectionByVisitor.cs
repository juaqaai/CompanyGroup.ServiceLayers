using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{

    public class GetCartCollectionByVisitor
    {
        public GetCartCollectionByVisitor() : this("", "") { }

        public GetCartCollectionByVisitor(string language, string visitorId)
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
