using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    InvoiceDate	            DueDate	                InvoiceAmount	InvoiceCredit	InvoiceId	Payment	        SalesType	CusomerRef	InvoicingName	InvoicingAddress	        ContactPersonId	Printed	VisszaruId	ItemDate	            LineNum	ItemId	    Name	                                                            Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst
    /// VR605656	2012-10-04 00:00:00.000	2012-11-03 00:00:00.000	193345	        193345	        HI043793/12	Átutalás 30 nap	3		                SERCO KFT.	    1037 Budapest Bécsi út 314.		            1		            2012-10-04 00:00:00.000	0	    ESPP400-9	Fujitsu Esprimo P400 PC, Intel Core i5-2320, 2GB, 500GB, W7Prof.	1	        132230	    132230	    1	                0	    0	            35702	    132230	        35702
    /// </summary>
    public class InvoiceLine
    {
        public InvoiceLine(int id, string invoiceId, DateTime itemDate, int lineNum, string itemId, string itemName, int quantity, decimal salesPrice, decimal lineAmount,
                           int quantityPhysical, int remain, int deliveryType, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, string currencyCode,
                           string description, long recId, bool pictureExists, bool inStock, bool availableInWebShop)
        {
            this.Id = id;
            this.InvoiceId = invoiceId;
            this.ItemDate = itemDate;
            this.LineNum = lineNum;
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.SalesPrice = salesPrice;
            this.LineAmount = lineAmount;
            this.QuantityPhysical = quantityPhysical;
            this.Remain = remain;
            this.DeliveryType = deliveryType;
            this.TaxAmount = taxAmount;
            this.LineAmountMst = lineAmountMst;
            this.TaxAmountMst = taxAmountMst;
            this.CurrencyCode = currencyCode;
            this.Description = description;
            this.RecId = recId;
            this.PictureExists = pictureExists;
            this.InStock = inStock;
            this.AvailableInWebShop = availableInWebShop;
        }

        public int Id { get; set; }	

        public string InvoiceId { get; set; }	

        public DateTime ItemDate { set; get; }

        public int LineNum { set; get; }

        public string ItemId { set; get; }

        public string ItemName { set; get; }

        public int Quantity	{ set; get; }

        public decimal SalesPrice { set; get; }

        public decimal LineAmount { set; get; }

        public int QuantityPhysical	{ set; get; }

        public int Remain { set; get; }

        public int DeliveryType { set; get; }

        public decimal TaxAmount { set; get; }

        public decimal LineAmountMst { set; get; }

        public decimal TaxAmountMst { set; get; }

        public string CurrencyCode { set; get; }

        public string Description { set; get; }

        public long RecId { set; get; }

        public bool PictureExists { set; get; }

        public bool InStock { set; get; }

        public bool AvailableInWebShop { set; get; }
    }
}
