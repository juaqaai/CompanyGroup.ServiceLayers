using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.ServiceRequest
{
    //[System.Runtime.Serialization.DataContract(Name = "GetInvoiceInfo", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetInvoiceInfo
    {
        public GetInvoiceInfo() : this(String.Empty, String.Empty, 0, DateTime.MinValue, DateTime.MaxValue) { }

        public GetInvoiceInfo(string visitorId, string languageId, int paymentType, DateTime fromDate, DateTime toDate)
        {
            this.VisitorId = visitorId;

            this.LanguageId = languageId;

            this.PaymentType = paymentType;

            this.FromDate = fromDate;

            this.ToDate = toDate;
        }

        //[System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "PaymentType", Order = 3)]
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
