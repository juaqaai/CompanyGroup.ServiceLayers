using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{

    /// <summary>
    /// AmountCredit	AmountOverdue	CurrencyCode
    /// 72246465.00	72246465.00	HUF
    /// </summary>
    public class InvoiceSumAmount
    {
        /// <summary>
        /// összes tartozás
        /// </summary>
        public decimal AmountCredit { get; set; }

        /// <summary>
        /// összes lejárt tartozás
        /// </summary>
        public decimal AmountOverdue { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// számlaösszesítések
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="invoiceType"></param>
        /// <param name="currencyCode"></param>
        public InvoiceSumAmount(decimal amountCredit, decimal amountOverdue, string currencyCode)
        {
            this.AmountCredit = amountCredit;

            this.AmountOverdue = amountOverdue;

            this.CurrencyCode = currencyCode;
        }

        public InvoiceSumAmount() : this(0, 0, "") { }
    }

    public class InvoiceSumAmounts : List<InvoiceSumAmount> {}
}
