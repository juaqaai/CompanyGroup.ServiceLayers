using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// ItemDate	            LineNum	ItemId	    ItemName	                        Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst	CurrencyCode	Description	FileName	RecId	    PictureExists
    /// 2013-01-31 00:00:00.000	0	    S7J-00008	L2 Comfort Mouse 6000 USB EMEA EG	1	        4510.00	    4510.00	    1	                0	    0	            1217.70	    4510.00	        1217.70	        HUF			                            5637928067	0
    /// </summary>
    public class InvoiceLine
    {
        public InvoiceLine(int id, string invoiceId, string currencyCode, int deliveryType, string description, string itemDate, string itemId, string lineAmount, string lineAmountMst, 
                           string itemName, bool pictureExists, int quantity, string salesPrice, string taxAmount, string taxAmountMst, long recId)
        {
            this.Id = id;
            this.InvoiceId = invoiceId;
            this.ItemId = itemId;
            this.Name = itemName;
            this.Quantity = quantity;
            this.SalesPrice = salesPrice;
            this.LineAmount = lineAmount;
            this.DeliveryType = deliveryType;
            this.TaxAmount = taxAmount;
            this.LineAmountMst = lineAmountMst;
            this.TaxAmountMst = taxAmountMst;
            this.CurrencyCode = currencyCode;
            this.Description = description;
            this.RecId = recId;
            this.PictureExists = pictureExists;
        }

        public int Id { get; set; }

        /// <summary>
        /// azonosító
        /// </summary>
        public string InvoiceId { get; set; }	

        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>
        public string ItemDate { set; get; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ItemId { set; get; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// ár
        /// </summary>
        public string SalesPrice { set; get; }

        /// <summary>
        /// valutanem
        /// </summary>
        public string CurrencyCode { set; get; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { set; get; }

        /// <summary>
        /// sor összesen
        /// </summary>
        public string LineAmount { set; get; }

        /// <summary>
        /// sor összesen saját pénznemben
        /// </summary>
        public string LineAmountMst { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int DeliveryType { set; get; }

        /// <summary>
        /// adótartalom
        /// </summary>
        public string TaxAmount { set; get; }

        /// <summary>
        /// adótartalom saját pénznemben
        /// </summary>
        public string TaxAmountMst { set; get; }

        /// <summary>
        /// termékleírás
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// kép kikereséshez szükséges rekord azonosító
        /// </summary>
        public long RecId { set; get; }

        /// <summary>
        /// létezik-e kép a termékhez?
        /// </summary>
        public bool PictureExists { set; get; }
    }
}
