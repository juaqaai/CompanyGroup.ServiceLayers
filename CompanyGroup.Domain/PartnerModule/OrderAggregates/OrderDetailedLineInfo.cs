using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// Id	    CustomerId	DataAreaId	SalesId	    CreatedDate	            ShippingDateRequested	CurrencyCode	Payment	            SalesHeaderType	SalesHeaderStatus	CusomerOrderNo	
    /// 929278	V001446	    bsc	        BVR117021	2010-12-30 00:00:00.000	2011-01-06 00:00:00.000	HUF	            Átutalás 21 napra	Csereigazolás	3		            	            
    /// LineNum	SalesStatus	ItemId	    ItemName	                            Quantity	SalesPrice	LineAmount	SalesDeliverNow	RemainSalesPhysical	StatusIssue	InventLocationId	ItemDate	            FileName	        ExtractDate	                PackageLogKey
    /// 2       1	        981-000258	Wireless Gaming Headset G930 (Logitech)	1	        35600.00	35600.00	0	            1	                6	        1000	            2010-12-30 00:00:00.000	981-000258_1.jpg	2013-04-14 22:46:53.5600000	396
    /// </summary>
    public class OrderDetailedLineInfo
    {

        public OrderDetailedLineInfo(int id, string dataAreaId, string salesId, DateTime createdDate, DateTime shippingDateRequested, string currencyCode, string payment, string salesHeaderType, int salesHeaderStatus, string customerOrderNo, bool withDelivery, 
                                     int lineNum, int salesStatus, string productId, string productName, int quantity, decimal salesPrice, decimal lineAmount, int salesDeliverNow, int remainSalesPhysical, int statusIssue, string inventLocationId,
                                     DateTime itemDate, string fileName, bool inStock, bool availableInWebShop) 
        {
            this.Id = id;
            this.DataAreaId = dataAreaId;
            this.SalesId = salesId;
            this.CreatedDate = createdDate;
            this.ShippingDateRequested = shippingDateRequested;
            this.CurrencyCode = currencyCode;
            this.Payment = payment;
            this.SalesHeaderType = salesHeaderType;
            this.SalesHeaderStatus = salesHeaderStatus;
            this.CustomerOrderNo = customerOrderNo;
            this.WithDelivery = withDelivery;

            this.LineNum = lineNum;
            this.SalesStatus = salesStatus;
            this.ProductId = productId;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.SalesPrice = salesPrice;
            this.LineAmount = lineAmount;
            this.SalesDeliverNow = salesDeliverNow;
            this.RemainSalesPhysical = remainSalesPhysical;
            this.StatusIssue = (StatusIssue)statusIssue;
            this.InventLocationId = inventLocationId;
            this.ItemDate = itemDate;
            this.FileName = fileName;
            this.InStock = inStock;
            this.AvailableInWebShop = availableInWebShop;
        }

        public int Id { set; get; }

        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public string Payment { set; get; }

        public string SalesHeaderType { set; get; }

        public int SalesHeaderStatus { set; get; }

        public DateTime ShippingDateRequested { set; get; }

        public string CurrencyCode { set; get; }

        public string CustomerOrderNo { set; get; }

        public bool WithDelivery { set; get; }

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

        public DateTime ItemDate { set; get; }

        public string FileName { set; get; }

        public bool InStock { set; get; }

        public bool AvailableInWebShop { set; get; }
    }
}
