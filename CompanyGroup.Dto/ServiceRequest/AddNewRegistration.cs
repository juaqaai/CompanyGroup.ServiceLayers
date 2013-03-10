using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// új regisztráció hozzáadása kérés adatai
    /// </summary>
    public class AddNewRegistration
    {
        /// <summary>
        /// látogató azonosítója (lehet üres)
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// nyelvi kód
        /// </summary>
        public string LanguageId { get; set; }
    }
}
