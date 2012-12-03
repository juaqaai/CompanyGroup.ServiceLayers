using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// SalesId	    InvoiceDate	            DueDate	                InvoiceAmount	InvoiceCredit	CurrencyCode	InvoiceId	Payment	    SalesType	CusomerRef	    InvoicingName	                InvoicingAddress	                    ContactPersonId	Printed	ReturnItemId	ItemDate	            LineNum	ItemId	Name	                                Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst	DetailCurrencyCode
    /// VR421801	2012-11-23 00:00:00.000	2012-11-23 00:00:00.000	15276	        15276	        HUF	            HI002399/11	készpénz	3		                    CARRIER CR MAGYARORSZÁG Kft.	5123 Jászárokszállás Jászberényi út 2.		            0		                2012-11-23 00:00:00.000	0	    ATFTBIZ	Acer TFT pixelhiba-mentesség biztosítás	1	        12222	    12222	    1	                0	    0	            3055	    12222	        3055	        HUF
    /// </summary>
    public class InvoiceDetailedLineInfo
    {
        public InvoiceDetailedLineInfo(string salesId, DateTime invoiceDate, DateTime dueDate, Int64 invoiceAmount, Int64 invoiceCredit, string currencyCode, string invoiceId, 
                                       string payment, int salesType, string cusomerRef, string invoicingName, string invoicingAddress, string contactPersonId,
                                       bool printed, string returnItemId, DateTime itemDate, int lineNum, string itemId, string name, int quantity, Int64 salesPrice, Int64 lineAmount,
                                       int quantityPhysical, int remain, int deliveryType, Int64 taxAmount, Int64 lineAmountMst, Int64 taxAmountMst, string detailCurrencyCode)
        { 
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
        }

        public string SalesId { set; get; }

        public DateTime InvoiceDate { set; get; }

        public DateTime DueDate { set; get; }

        public Int64 InvoiceAmount { set; get; }

        public Int64 InvoiceCredit { set; get; }

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

        public Int64 SalesPrice { set; get; }

        public Int64 LineAmount { set; get; }	
                        
        public int QuantityPhysical	{ set; get; }

        public int Remain { set; get; }

        public int DeliveryType { set; get; }

        public Int64 TaxAmount { set; get; }

        public Int64 LineAmountMst { set; get; }

        public Int64 TaxAmountMst { set; get; }

        public string DetailCurrencyCode { set; get; }

    }
}
