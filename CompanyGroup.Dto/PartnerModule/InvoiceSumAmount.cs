using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class InvoiceSumAmount
    {
        public InvoiceSumAmount(string amountCredit, string amountOverdue, string currencyCode)
        {
            this.AmountCredit = amountCredit;

            this.AmountOverdue = amountOverdue;

            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// tartozás
        /// </summary>
        public string AmountCredit { get; set; }

        /// <summary>
        /// lejárt tartozás
        /// </summary>
        public string AmountOverdue { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
