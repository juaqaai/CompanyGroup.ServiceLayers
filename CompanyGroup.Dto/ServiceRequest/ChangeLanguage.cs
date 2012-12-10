using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class ChangeLanguage
    {
        public ChangeLanguage() : this(String.Empty, String.Empty) { }

        public ChangeLanguage(string visitorId, string language)
        {
            VisitorId = visitorId;

            Language = language;
        }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }
    }
}
