using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// 
    ///SalesId	CreatedDate	LineNum	SalesStatus	ShippingDateRequested	ProductId	ProductName	SalesPrice	CurrencyCode	Quantity	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	Payment	SalesHeaderType	HeaderSalesStatus
    ///BVR118466	2011-01-13 00:00:00.000	1	1	2011-01-20 00:00:00.000	942-000005	Extreme 3D Pro New Packaging (Logitech)	7130	HUF	1	7130	0	1	4	1000	Átutalás 21 napra	Standard	3    /// </summary>
    public class OrderDetailedLineInfo
    {
        public OrderDetailedLineInfo(string salesId, DateTime createdDate, int lineNum, int salesStatus, DateTime shippingDateRequested, string productId, string productName, 
                                     decimal salesPrice, string currencyCode, int quantity, decimal lineAmount, int salesDeliverNow, int remainSalesPhysical, int statusIssue, string inventLocationId,
                                     string payment, string salesHeaderType, int headerSalesStatus, string dataAreaId) 
        {
            this.SalesId = salesId;
            this.CreatedDate = createdDate;
            this.LineNum = lineNum;
            this.SalesStatus = salesStatus;
            this.ShippingDateRequested = shippingDateRequested;
            this.ProductId = productId;
            this.ProductName = productName;
            this.SalesPrice = salesPrice;
            this.CurrencyCode = currencyCode;
            this.Quantity = quantity;
            this.LineAmount = lineAmount;
            this.SalesDeliverNow = salesDeliverNow;
            this.RemainSalesPhysical = remainSalesPhysical;
            this.StatusIssue = (StatusIssue)statusIssue;
            this.InventLocationId = inventLocationId;
            this.Payment = payment;
            this.SalesHeaderType = salesHeaderType;
            this.HeaderSalesStatus = headerSalesStatus;
            this.DataAreaId = dataAreaId;
        }

        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public string Payment { set; get; }

        public string SalesHeaderType { set; get; }

        public int HeaderSalesStatus { set; get; }

        public DateTime ShippingDateRequested { set; get; }

        public string CurrencyCode { set; get; }

        public int LineNum { set; get; }

        public int SalesStatus { set; get; }

        public string ProductId { set; get; }

        public string ProductName { set; get; }

        public int Quantity { set; get; }

        public decimal SalesPrice { set; get; }

        public decimal LineAmount { set; get; }

        public int SalesDeliverNow { set; get; }

        public int RemainSalesPhysical { set; get; }

        public StatusIssue StatusIssue { set; get; }

        public string InventLocationId { set; get; }

        public string DataAreaId { set; get; }

    }
}
