using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    InvoiceDate	            DueDate	                InvoiceAmount	InvoiceCredit	InvoiceId	Payment	        SalesType	CusomerRef	InvoicingName	InvoicingAddress	        ContactPersonId	Printed	VisszaruId	ItemDate	            LineNum	ItemId	    Name	                                                            Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst
    /// VR605656	2012-10-04 00:00:00.000	2012-11-03 00:00:00.000	193345	        193345	        HI043793/12	Átutalás 30 nap	3		                SERCO KFT.	    1037 Budapest Bécsi út 314.		            1		            2012-10-04 00:00:00.000	0	    ESPP400-9	Fujitsu Esprimo P400 PC, Intel Core i5-2320, 2GB, 500GB, W7Prof.	1	        132230	    132230	    1	                0	    0	            35702	    132230	        35702
    /// </summary>
    public class InvoiceDetailedLineInfo
    {
        public InvoiceDetailedLineInfo(string salesId, DateTime invoiceDate, DateTime dueDate, decimal invoiceAmount, decimal invoiceCredit, string currencyCode, string invoiceId, 
                                       string payment, int salesType, string cusomerRef, string invoicingName, string invoicingAddress, string contactPersonId, 
                                       bool printed, string returnItemId, DateTime itemDate, int lineNum, string itemId, string name, int quantity, decimal salesPrice, decimal lineAmount,
                                       int quantityPhysical, int remain, int deliveryType, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, string detailCurrencyCode)
        { 
            SalesId	= salesId;
            InvoiceDate	= invoiceDate;
            DueDate = dueDate;	                
            InvoiceAmount = invoiceAmount;
            InvoiceCredit = invoiceCredit;
            CurrencyCode = currencyCode;
            InvoiceId = invoiceId;
            Payment = payment;
            SalesType = (InvoiceSalesType) salesType;
            CusomerRef = cusomerRef;	
            InvoicingName = invoicingName;
            InvoicingAddress = invoicingAddress;	        
            ContactPersonId	= contactPersonId;
            Printed = printed;	
            ReturnItemId = returnItemId;	
            ItemDate = itemDate;
            LineNum	= lineNum;
            ItemId = itemId;  
            Name = name;	                                                            
            Quantity = quantity;
            SalesPrice = salesPrice;
            LineAmount = lineAmount;
            QuantityPhysical = quantityPhysical;
            Remain= remain;
            DeliveryType = deliveryType;
            TaxAmount = taxAmount;
            LineAmountMst = lineAmountMst;
            TaxAmountMst = taxAmountMst;
            DetailCurrencyCode = detailCurrencyCode;
        }

        public string SalesId { set; get; }

        public DateTime InvoiceDate { set; get; }

        public DateTime DueDate { set; get; }

        public decimal InvoiceAmount { set; get; }

        public decimal InvoiceCredit { set; get; }

        public string CurrencyCode { get; set; }

        public string InvoiceId { set; get; }

        public string Payment { set; get; }

        public InvoiceSalesType SalesType { set; get; }

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
                               
        public decimal  LineAmountMst { set; get; }	
                                    
        public decimal TaxAmountMst { set; get; }

        public string DetailCurrencyCode { set; get; }

    }
}
