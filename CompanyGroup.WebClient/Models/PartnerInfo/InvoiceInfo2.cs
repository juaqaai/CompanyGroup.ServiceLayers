using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számla info
    /// </summary>
    public class InvoiceInfo 
    {
        public InvoiceInfo(CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo)
        { 
                this.ContactPersonId = invoiceInfo.ContactPersonId;
                this.CurrencyCode = invoiceInfo.CurrencyCode;
                this.CusomerRef = invoiceInfo.CusomerRef;
                this.DueDate = invoiceInfo.DueDate;
                this.InvoiceAmount = invoiceInfo.InvoiceAmount;
                this.InvoiceCredit = invoiceInfo.InvoiceCredit;
                this.InvoiceDate = invoiceInfo.InvoiceDate;
                this.InvoiceId = invoiceInfo.InvoiceId;
                this.InvoicingAddress = invoiceInfo.InvoicingAddress;
                this.InvoicingName = invoiceInfo.InvoicingName;
                this.Payment = invoiceInfo.Payment;
                this.Printed = invoiceInfo.Printed;
                this.ReturnItemId = invoiceInfo.ReturnItemId;
                this.SalesType = invoiceInfo.SalesType;
                this.SalesId = invoiceInfo.SalesId;
                this.Lines = invoiceInfo.Lines.ConvertAll(x => new InvoiceLineInfo(x));  
        }

        /// <summary>
        /// VR, vagy BR azonosító
        /// </summary>
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        public string InvoiceDate { set; get; }

        /// <summary>
        /// bizonylat lejárati ideje
        /// </summary>
        public string DueDate { set; get; }

        /// <summary>
        /// számla végösszege
        /// </summary>
        public string InvoiceAmount { set; get; }

        /// <summary>
        /// számlán lévő tartozás
        /// </summary>
        public string InvoiceCredit { set; get; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string CurrencyCode { set; get; }

        /// <summary>
        /// számla azonosító
        /// </summary>
        public string InvoiceId { set; get; }

        /// <summary>
        /// fizetési feltétel
        /// </summary>
        public string Payment { set; get; }

        /// <summary>
        /// számla típusa (0 napló, 1 árajánlat, 2 előfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet)
        /// </summary>
        public int SalesType { set; get; }

        /// <summary>
        /// vevői hivatkozás
        /// </summary>
        public string CusomerRef { set; get; }

        /// <summary>
        /// számlán szereplő név
        /// </summary>
        public string InvoicingName { set; get; }

        /// <summary>
        /// számlázási cím
        /// </summary>
        public string InvoicingAddress { set; get; }

        /// <summary>
        /// kapcsolattartó
        /// </summary>
        public string ContactPersonId { set; get; }

        /// <summary>
        /// nyomtatva
        /// </summary>
        public bool Printed { set; get; }

        /// <summary>
        /// visszáru 
        /// </summary>
        public string ReturnItemId { set; get; }

        /// <summary>
        /// számla sorok     
        /// </summary>
        public List<CompanyGroup.WebClient.Models.InvoiceLineInfo> Lines { get; set; }
    }
}
