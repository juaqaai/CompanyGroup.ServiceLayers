using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// hírlevél lista lekérdezés paramétereit összefogó adattípus
    /// </summary>
    public class GetNewsletterListByFilterRequest
    {
        public GetNewsletterListByFilterRequest() : this("", "", new List<string>()) { }

        public GetNewsletterListByFilterRequest(string language, string visitorId, List<string> newsletterIdList)
        { 
            this.Language = language;

            this.VisitorId = visitorId;

            this.NewsletterIdList = newsletterIdList;
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
        /// hírlevél lista
        /// </summary>
        public List<string> NewsletterIdList { get; set; }
    }
}
