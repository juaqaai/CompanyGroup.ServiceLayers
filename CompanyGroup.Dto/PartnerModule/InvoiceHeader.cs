using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// számla fejléc adatok
    /// </summary>
    public class InvoiceHeader
    {
        /// <summary>
        /// azonosító
        /// </summary>
        public int Id { get; set; }	

        /// <summary>
        /// számla kiállítás dátuma
        /// </summary>
        public string InvoiceDate { get; set; }

        /// <summary>
        /// számla kiállító vállalat
        /// </summary>
        public string SourceCompany { get; set; }

        /// <summary>
        /// lejárati dátum
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// számla összege
        /// </summary>
        public string InvoiceAmount { get; set; }

        /// <summary>
        /// tartozás a számlán
        /// </summary>
        public string InvoiceCredit { get; set; }

        /// <summary>
        /// pénznem 
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// számlaszám
        /// </summary>
        public string InvoiceId { get; set; }

        /// <summary>
        /// sor teljes összege
        /// </summary>
        public string LineAmount { get; set; }

        /// <summary>
        /// áfa összege
        /// </summary>
        public string TaxAmount { get; set; }

        /// <summary>
        /// sor teljes összege saját pénznemben
        /// </summary>
        public string LineAmountMst { get; set; }

        /// <summary>
        /// áfa összege saját pénznemben
        /// </summary>
        public string TaxAmountMst { get; set; }

        public InvoiceHeader(int id, string invoiceDate, string sourceCompany, string dueDate, string invoiceAmount, string invoiceCredit, string currencyCode, string invoiceId, string lineAmount, string taxAmount, string lineAmountMst, string taxAmountMst)
        {
            this.Id = id;

            this.InvoiceDate = invoiceDate;

            this.SourceCompany = sourceCompany;

            this.DueDate = dueDate;

            this.InvoiceAmount = invoiceAmount;

            this.InvoiceCredit = invoiceCredit;

            this.CurrencyCode = currencyCode;

            this.InvoiceId = invoiceId;

            this.LineAmount = lineAmount;

            this.TaxAmount = taxAmount;

            this.LineAmountMst = lineAmountMst;

            this.TaxAmountMst = taxAmountMst;
        }
    }
}
