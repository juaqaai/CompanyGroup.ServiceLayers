using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// részletes termékadatlap látogatottsági lista lekérdezés paraméterét tartalmazó osztály
    /// </summary>
    public class CatalogueDetailsLogListRequest
    {
        public CatalogueDetailsLogListRequest()
        {
            this.VisitorId = String.Empty;
        }

        public string VisitorId { get; set; }
    }
}
