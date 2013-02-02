using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// hírlevél lista lekérdezés paramétereit összefogó adattípus
    /// </summary>
    public class GetNewsletterCollectionRequest
    {
        public GetNewsletterCollectionRequest() : this("", "", "") { }

        public GetNewsletterCollectionRequest(string language, string visitorId, string manufacturerId)
        { 
            this.Language = language;

            this.VisitorId = visitorId;

            this.ManufacturerId = manufacturerId;
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
        /// gyártó
        /// </summary>
        public string ManufacturerId { get; set; }
    }
}
