using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    CreatedDate             LineNum	ShippingDateRequested	ItemId	        Name	                                                                            SalesPrice	CurrencyCode	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	Payment
    /// VR605202	2012-10-03 00:00:00.000 1	    2012-10-10 00:00:00.000	N76VMV2GT1095V	ASUS N76VM-V2G-T1095V  17.3" FHD,i5-3210M,6GB,750GB,B-Ray Combo,GT630M 2G, W7 HP	213140	HUF	                1	        213140	    0	            1	                4	        KULSO	            Átutalás 8 nap
    /// </summary>
    public class OrderDetailedLineInfo
    {
        public OrderDetailedLineInfo(string salesId, DateTime createdDate, int lineNum, DateTime shippingDateRequested, string itemId, string name, decimal salesPrice, string currencyCode, 
                                     int quantity, decimal lineAmount, int salesDeliverNow, int remainSalesPhysical, int statusIssue, string inventLocationId, string payment)
        {
            this.SalesId = salesId;
            this.CreatedDate = createdDate;
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
            this.StatusIssue = (StatusIssue)statusIssue;
            this.InventLocationId = inventLocationId;
            this.Payment = payment;
        }

        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public int LineNum { set; get; }

        public DateTime ShippingDateRequested { set; get; }

        public string ItemId { set; get; }

        public string Name { set; get; }

        public decimal SalesPrice { set; get; }

        public string CurrencyCode { set; get; }

        public int Quantity { set; get; }

        public decimal LineAmount { set; get; }

        public int SalesDeliverNow { set; get; }

        public int RemainSalesPhysical { set; get; }

        public StatusIssue StatusIssue { set; get; }

        public string InventLocationId { set; get; }

        public string Payment { set; get; }
    }
}
