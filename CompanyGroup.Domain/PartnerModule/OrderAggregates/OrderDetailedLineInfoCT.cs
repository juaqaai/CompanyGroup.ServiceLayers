using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// CustomerId, DataAreaId,	SalesId, CreatedDate, ShippingDateRequested, CurrencyCode, Payment,	SalesHeaderType, SalesHeaderStatus,	CustomerOrderNo, DlvTerm, 
    /// LineNum, SalesStatus, ProductId, ProductName, SalesPrice, Quantity, LineAmount, SalesDeliverNow, RemainSalesPhysical, StatusIssue, InventLocationId, ItemDate, [FileName]
    /// </summary>
    public class OrderDetailedLineInfoCT
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="salesId"></param>
        /// <param name="createdDate"></param>
        /// <param name="shippingDateRequested"></param>
        /// <param name="currencyCode"></param>
        /// <param name="payment"></param>
        /// <param name="salesHeaderType"></param>
        /// <param name="salesHeaderStatus"></param>
        /// <param name="customerOrderNo"></param>
        /// <param name="dlvTerm"></param>
        /// <param name="lineNum"></param>
        /// <param name="salesStatus"></param>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="salesPrice"></param>
        /// <param name="quantity"></param>
        /// <param name="lineAmount"></param>
        /// <param name="salesDeliverNow"></param>
        /// <param name="remainSalesPhysical"></param>
        /// <param name="statusIssue"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="itemDate"></param>
        /// <param name="fileName"></param>
        public OrderDetailedLineInfoCT(string customerId, string dataAreaId, string salesId, DateTime createdDate, DateTime shippingDateRequested, string currencyCode, string payment, string salesHeaderType, int salesHeaderStatus, string customerOrderNo, string dlvTerm, 
                                       int lineNum, int salesStatus, string productId, string productName, decimal salesPrice, int quantity, decimal lineAmount, int salesDeliverNow, int remainSalesPhysical, int statusIssue, string inventLocationId,
                                       DateTime itemDate, string fileName) 
        {
            this.CustomerId = customerId;
            this.DataAreaId = dataAreaId;
            this.SalesId = salesId;
            this.CreatedDate = createdDate;
            this.ShippingDateRequested = shippingDateRequested;
            this.CurrencyCode = currencyCode;
            this.Payment = payment;
            this.SalesHeaderType = salesHeaderType;
            this.SalesHeaderStatus = salesHeaderStatus;
            this.CustomerOrderNo = customerOrderNo;
            this.WithDelivery = dlvTerm.Equals("KISZALL");

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
            this.InStock = true;
            this.AvailableInWebShop = true;
        }

        public string CustomerId { set; get; }

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
