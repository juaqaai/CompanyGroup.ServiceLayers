using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// megrendelés információk lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetOrderInfoRequest
    {
        public GetOrderInfoRequest(string visitorId, string languageId)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }
    }
}
