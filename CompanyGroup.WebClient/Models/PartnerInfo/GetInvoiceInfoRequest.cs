using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class GetInvoiceInfoRequest
    {
        public GetInvoiceInfoRequest()
        {
            this.Debit = true;

            this.Overdue = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public bool Debit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Overdue { get; set; }

    }
}
