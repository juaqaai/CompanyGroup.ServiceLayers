using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// számlainformációk lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetInvoiceInfoRequest
    {
        public GetInvoiceInfoRequest() : this(String.Empty, String.Empty, 0, DateTime.MinValue, DateTime.MaxValue) { }

        public GetInvoiceInfoRequest(string visitorId, string languageId, int paymentType, DateTime fromDate, DateTime toDate)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.PaymentType = paymentType;

            this.FromDate = fromDate;

            this.ToDate = toDate;
        }

        public string VisitorId { get; set; }

        public string LanguageId { get; set; }

        public int PaymentType { get; set; }

        /// <summary>
        /// kezdő dátum
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// lezáró dátum
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
