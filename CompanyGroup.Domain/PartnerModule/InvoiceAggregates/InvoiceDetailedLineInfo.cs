using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// </summary>
    public class InvoiceDetailedLineInfo
    {
        public InvoiceDetailedLineInfo(string customerId, string dataAreaId, string salesId, DateTime invoiceDate, DateTime dueDate, decimal invoiceAmount, decimal invoiceCredit, string currencyCode, string invoiceId, 
                                       string payment, int salesType, string cusomerRef, string invoicingName, string invoicingAddress, string contactPersonId,
                                       bool printed, string returnItemId, DateTime itemDate, int lineNum, string itemId, string name, int quantity, decimal salesPrice, decimal lineAmount,
                                       int quantityPhysical, int remain, int deliveryType, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, string detailCurrencyCode, 
                                       string description, long recId, bool pictureExists)
        {
            this.CustomerId = customerId;
            this.DataAreaId = dataAreaId;
            this.SalesId	= salesId;
            this.InvoiceDate = invoiceDate;
            this.DueDate = dueDate;
            this.InvoiceAmount = invoiceAmount;
            this.InvoiceCredit = invoiceCredit;
            this.CurrencyCode = currencyCode;
            this.InvoiceId = invoiceId;
            this.Payment = payment;
            this.SalesType = (CompanyGroup.Domain.PartnerModule.InvoiceSalesType)salesType;
            this.CusomerRef = cusomerRef;
            this.InvoicingName = invoicingName;
            this.InvoicingAddress = invoicingAddress;
            this.ContactPersonId = contactPersonId;
            this.Printed = printed;
            this.ReturnItemId = returnItemId;
            this.ItemDate = itemDate;
            this.LineNum = lineNum;
            this.ItemId = itemId;
            this.Name = name;
            this.Quantity = quantity;
            this.SalesPrice = salesPrice;
            this.LineAmount = lineAmount;
            this.QuantityPhysical = quantityPhysical;
            this.Remain = remain;
            this.DeliveryType = deliveryType;
            this.TaxAmount = taxAmount;
            this.LineAmountMst = lineAmountMst;
            this.TaxAmountMst = taxAmountMst;
            this.DetailCurrencyCode = detailCurrencyCode;
            this.Description = description;
            this.RecId = recId;
            this.PictureExists = pictureExists;
        }

        public string CustomerId { set; get; }

        public string DataAreaId { set; get; }

        public string SalesId { set; get; }

        public DateTime InvoiceDate { set; get; }

        public DateTime DueDate { set; get; }

        public decimal InvoiceAmount { set; get; }

        public decimal InvoiceCredit { set; get; }

        public string CurrencyCode { get; set; }

        public string InvoiceId { set; get; }

        public string Payment { set; get; }

        public CompanyGroup.Domain.PartnerModule.InvoiceSalesType SalesType { set; get; }

        public string CusomerRef { set; get; }

        public string InvoicingName { set; get; }

        public string InvoicingAddress { set; get; }

        public string ContactPersonId { set; get; }

        public bool Printed { set; get; }

        public string ReturnItemId { set; get; }
        
        public DateTime ItemDate { set; get; }

        public int LineNum { set; get; }
                
        public string ItemId { set; get; }	    
                
        public string Name { set; get; }                                                            
                    
        public int Quantity	{ set; get; }

        public decimal SalesPrice { set; get; }

        public decimal LineAmount { set; get; }	
                        
        public int QuantityPhysical	{ set; get; }

        public int Remain { set; get; }

        public int DeliveryType { set; get; }

        public decimal TaxAmount { set; get; }

        public decimal LineAmountMst { set; get; }

        public decimal TaxAmountMst { set; get; }

        public string DetailCurrencyCode { set; get; }

        public string Description { set; get; }

		public string FileName { set; get; }

        public long RecId { set; get; }

        public bool PictureExists{ set; get; }
    }
}
