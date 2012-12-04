using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    InvoiceDate	            DueDate	                InvoiceAmount	InvoiceCredit	InvoiceId	Payment	        SalesType	CusomerRef	InvoicingName	InvoicingAddress	        ContactPersonId	Printed	VisszaruId	ItemDate	            LineNum	ItemId	    Name	                                                            Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst
    /// VR605656	2012-10-04 00:00:00.000	2012-11-03 00:00:00.000	193345	        193345	        HI043793/12	Átutalás 30 nap	3		                SERCO KFT.	    1037 Budapest Bécsi út 314.		            1		            2012-10-04 00:00:00.000	0	    ESPP400-9	Fujitsu Esprimo P400 PC, Intel Core i5-2320, 2GB, 500GB, W7Prof.	1	        132230	    132230	    1	                0	    0	            35702	    132230	        35702
    /// </summary>
    public class InvoiceLineInfo
    {
        public InvoiceLineInfo(DateTime itemDate, int lineNum, string itemId, string name, int quantity, decimal salesPrice, decimal lineAmount,
                               int quantityPhysical, int remain, int deliveryType, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, string currencyCode)
        {
            ItemDate = itemDate;
            LineNum = lineNum;
            ItemId = itemId;
            Name = name;
            Quantity = quantity;
            SalesPrice = salesPrice;
            LineAmount = lineAmount;
            QuantityPhysical = quantityPhysical;
            Remain = remain;
            DeliveryType = deliveryType;
            TaxAmount = taxAmount;
            LineAmountMst = lineAmountMst;
            TaxAmountMst = taxAmountMst;
            this.CurrencyCode = currencyCode;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ItemDate")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime ItemDate { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("LineNum")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public int LineNum { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ItemId")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string ItemId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Name")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string Name { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Quantity")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public int Quantity	{ set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("SalesPrice")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public decimal SalesPrice { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("LineAmount")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public decimal LineAmount { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("QuantityPhysical")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]              
        public int QuantityPhysical	{ set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Remain")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]  
        public int Remain { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("DeliveryType")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)] 
        public int DeliveryType { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("TaxAmount")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)] 
        public decimal TaxAmount { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("LineAmountMst")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)] 
        public decimal LineAmountMst { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("TaxAmountMst")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public decimal TaxAmountMst { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CurrencyCode")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string CurrencyCode { set; get; }
    }
}
