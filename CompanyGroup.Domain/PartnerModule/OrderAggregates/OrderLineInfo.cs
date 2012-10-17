using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// LineNum	ShippingDateRequested	ItemId	        Name	                                                                            SalesPrice	CurrencyCode	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	Payment
    /// 1	    2012-10-10 00:00:00.000	N76VMV2GT1095V	ASUS N76VM-V2G-T1095V  17.3" FHD,i5-3210M,6GB,750GB,B-Ray Combo,GT630M 2G, W7 HP	213140	HUF	                1	        213140	    0	            1	                4	        KULSO	            Átutalás 8 nap
    /// </summary>
    public class OrderLineInfo
    {
        public OrderLineInfo(int lineNum, DateTime shippingDateRequested, string itemId, string name, decimal salesPrice, string currencyCode,
                              int quantity, decimal lineAmount, int salesDeliverNow, int remainSalesPhysical, StatusIssue statusIssue, string inventLocationId, string payment)
        { 
            this.LineNum = lineNum;
            this.ShippingDateRequested = shippingDateRequested;
            this.ItemId = itemId;
            this.Name = name;
            this.SalesPrice = salesPrice;
            this.CurrencyCode = currencyCode;
            this.Quantity = quantity;
            this.LineAmount = lineAmount;
            this.SalesDeliverNow = salesDeliverNow;
            this.RemainSalesPhysical = remainSalesPhysical;
            this.StatusIssue = statusIssue;
            this.InventLocationId = inventLocationId;
            this.Payment = payment;
        }

        /// <summary>
        /// sorszám, egy rendelésen belül
        /// </summary>
        public int LineNum { set; get; }

        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>
        public DateTime ShippingDateRequested { set; get; }

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
        public decimal SalesPrice { set; get; }

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
        public decimal LineAmount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int SalesDeliverNow { set; get; }

        public int RemainSalesPhysical { set; get; }

        public StatusIssue StatusIssue { set; get; }

        /// <summary>
        /// raktárkód
        /// </summary>
        public string InventLocationId { set; get; }

        /// <summary>
        /// fizetési mód
        /// </summary>
        public string Payment { set; get; }
    }
}
