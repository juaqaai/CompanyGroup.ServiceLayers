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
                ContactPersonId = invoiceInfo.ContactPersonId;
                CurrencyCode = invoiceInfo.CurrencyCode;
                CusomerRef = invoiceInfo.CusomerRef;
                DueDate = invoiceInfo.DueDate;
                InvoiceAmount = invoiceInfo.InvoiceAmount;
                InvoiceCredit = invoiceInfo.InvoiceCredit;
                InvoiceDate = invoiceInfo.InvoiceDate;
                InvoiceId = invoiceInfo.InvoiceId;
                InvoicingAddress = invoiceInfo.InvoicingAddress;
                InvoicingName = invoiceInfo.InvoicingName;
                Payment = invoiceInfo.Payment;
                Printed = invoiceInfo.Printed;
                ReturnItemId = invoiceInfo.ReturnItemId;
                SalesType = invoiceInfo.SalesType;
                SalesId = invoiceInfo.SalesId;
                Lines = invoiceInfo.Lines.ConvertAll(x => new InvoiceLineInfo(x));      
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
