using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// 
    /// Id	    InvoiceDate	            SourceCompany	DueDate	                InvoiceAmount	InvoiceCredit	CurrencyCode	InvoiceId	OverDue	
    /// 1614982	2013-05-31 00:00:00.000	bsc	            2013-06-21 00:00:00.000	42570.00	    42570.00	    HUF	            BI005452/13	0	    
    /// LineNum	ItemDate	            ItemId	    ItemName	                                                                Quantity	SalesPrice	QuantityPhysical	Remain	DeliveryType	LineAmount	TaxAmount	LineAmountMst	TaxAmountMst	CurrencyCode	Description	                                                                FileName	    RecId	    PictureExists	InStock	AvailableInWebShop	DataAreaId
    /// 0	    2013-05-31 00:00:00.000	DNS-320LW	D-Link ShareCenter® 2-Bay Cloud Network Storage Enclosure, SATA II, White	1	        18650.00	1	                0	    0	            18650.00	5035.50	    18650.00	    5035.50	        HUF	            D-Link ShareCenter® 2-Bay Cloud Network Storage Enclosure, SATA II, White	DNS-320LW.jpg	5637954653	1	            1	    1	                bsc
    /// </summary>
    public class InvoiceDetailedLineInfo
    {

        public InvoiceDetailedLineInfo(int id, DateTime invoiceDate, string sourceCompany, DateTime dueDate, decimal invoiceAmount, decimal invoiceCredit, string currencyCode, string invoiceId, bool overDue, 
                                       int lineNum, DateTime itemDate, string itemId, string itemName, int quantity, decimal salesPrice,
                                       int quantityPhysical, int remain, int deliveryType, decimal lineAmount, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, 
                                       string description, string fileName, long recId, bool pictureExists, bool inStock, bool availableInWebShop, string dataAreaId)
        {

            this.Id = id;
            this.InvoiceDate = invoiceDate;
            this.SourceCompany = sourceCompany;
            this.DueDate = dueDate;
            this.InvoiceAmount = invoiceAmount;
            this.InvoiceCredit = invoiceCredit;
            this.CurrencyCode = currencyCode;
            this.InvoiceId = invoiceId;
            this.OverDue = overDue;
            this.LineNum = lineNum;
            this.ItemDate = itemDate;
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.SalesPrice = salesPrice;
            this.QuantityPhysical = quantityPhysical;
            this.Remain = remain;
            this.DeliveryType = deliveryType;
            this.LineAmount = lineAmount;
            this.TaxAmount = taxAmount;
            this.LineAmountMst = lineAmountMst;
            this.TaxAmountMst = taxAmountMst;
            this.Description = description;
            this.FileName = fileName; 
            this.RecId = recId;
            this.PictureExists = pictureExists;
            this.InStock = inStock;
            this.AvailableInWebShop = availableInWebShop;
            this.DataAreaId = dataAreaId;
        }

         public int Id { get; set; }	

        public DateTime InvoiceDate { get; set; }	            
        
        public string SourceCompany	 { get; set; }
            
        public DateTime DueDate	{ get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal InvoiceCredit { get; set; }  	
                
        public string CurrencyCode	{ get; set; }  	
                    
        public string InvoiceId	{ get; set; }  	
                    
        public bool OverDue { get; set; }         

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

        public string Description { set; get; }

        public string FileName { set; get; }

        public long RecId { set; get; }

        public bool PictureExists { set; get; }

        public bool InStock { set; get; }

        public bool AvailableInWebShop { set; get; }

        public string DataAreaId { set; get; }         

    }
}
