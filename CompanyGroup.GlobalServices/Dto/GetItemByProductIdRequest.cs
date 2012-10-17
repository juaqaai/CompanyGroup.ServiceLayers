using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    /// <summary>
    /// termékelem kérés elemeit összefogó osztály
    /// </summary>
    public class GetItemByProductIdRequest
    {
        /// <summary>
        /// az a vállalat, amelyikre a kérés vonatkozik 
        /// </summary>
        /// <remarks>hrp / bsc. Alapértelmezett érték hrp</remarks>
        public string DataAreaId { get; set; }

        /// <summary>
        /// hozzáférési kód
        /// </summary>
        /// <remarks>64 hosszú partnerhez rendelt egyedi azonosító, melyet minden kérésnél el kell küldeni</remarks>
        public string AuthenticationCode { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        public GetItemByProductIdRequest()
        { 
            this.DataAreaId = String.Empty;

            this.AuthenticationCode = String.Empty;

            this.ProductId = String.Empty;
        }
    }
}
