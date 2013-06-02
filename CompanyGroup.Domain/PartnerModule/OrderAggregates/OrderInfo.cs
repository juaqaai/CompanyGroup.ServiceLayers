using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    CreatedDate
    /// VR605202	2012-10-03 00:00:00.000
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// megrendelés információ
        /// </summary>
        /// <param name="salesId"></param>
        /// <param name="createdDate"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="payment"></param>
        /// <param name="salesHeaderType"></param>
        /// <param name="headerSalesStatus"></param>
        /// <param name="withDelivery"></param>
        public OrderInfo(string salesId, DateTime createdDate, string dataAreaId, string payment, string salesHeaderType, int headerSalesStatus, bool withDelivery)
        { 
            this.SalesId = salesId;

            this.CreatedDate = createdDate;

            this.DataAreaId = dataAreaId;

            this.Payment = payment;

            this.SalesHeaderType = salesHeaderType;

            this.HeaderSalesStatus = headerSalesStatus;

            this.WithDelivery = withDelivery;

            this.OrderLines = new List<OrderLineInfo>();
        }

        /// <summary>
        /// VR, vagy BR
        /// </summary>
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        public DateTime CreatedDate { set; get; }

        /// <summary>
        /// vállalatkód, ahonnan a rendelés származik
        /// </summary>
        public string DataAreaId { set; get; }

        /// <summary>
        /// fizetés módja (Átutalás 8 nap)
        /// </summary>
        public string Payment { set; get; }

        /// <summary>
        /// Standard / Csereigazolás
        /// </summary>
        public string SalesHeaderType { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int HeaderSalesStatus { set; get; }

        /// <summary>
        /// szállítással rendelve?
        /// </summary>
        public bool WithDelivery { set; get; }

        /// <summary>
        /// rendelés sorok
        /// </summary>
        public List<OrderLineInfo> OrderLines { set; get; }

        /// <summary>
        /// rendelés sorok netto összesen 
        /// </summary>
        public decimal SumAmountNetto
        {
            get { return this.OrderLines.Sum(x => x.LineAmount); }
        }

        /// <summary>
        /// rendelés sorok brutto összesen 
        /// </summary>
        public decimal SumAmountBrutto
        {
            get { return this.OrderLines.Sum(x => (x.LineAmount * Convert.ToDecimal(1.27))); }
        }

        /// <summary>
        /// rendelés sor hozzáadása
        /// </summary>
        /// <param name="line"></param>
        public void AddOrderLine(OrderLineInfo line)
        {
            this.OrderLines.Add(line);
        }

        /// <summary>
        /// létrehoz egy rendelés info-t tételeivel együtt  
        /// </summary>
        /// <param name="lineInfos"></param>
        /// <returns></returns>
        public static OrderInfo Create(List<OrderDetailedLineInfo> lineInfos)
        {
            OrderInfo orderInfo = null;

            lineInfos.ForEach(x => 
            {
                if (orderInfo == null)
                {
                    orderInfo = new OrderInfo(x.SalesId, x.CreatedDate, x.DataAreaId, x.Payment, x.SalesHeaderType, x.SalesHeaderStatus, x.WithDelivery);
                }

                OrderLineInfo orderLineInfo = new OrderLineInfo(x.Id, x.LineNum, x.ShippingDateRequested, x.ProductId, x.ProductName, x.SalesPrice, x.CurrencyCode, x.Quantity, x.LineAmount, x.SalesDeliverNow, x.RemainSalesPhysical, x.StatusIssue, x.InventLocationId, x.Payment, x.InStock, x.AvailableInWebShop, !String.IsNullOrEmpty(x.FileName), x.DataAreaId);

                orderInfo.AddOrderLine(orderLineInfo);
            });

            return orderInfo;
        }

    }

    /// <summary>
    /// teljes megrendelés lista
    /// </summary>
    public class OrderInfoList : List<OrderInfo>
    {
        public decimal OpenOrderAmount { get; set; }

        public OrderInfoList(decimal openOrderAmount, List<OrderInfo> orderInfos)
        {
            this.OpenOrderAmount = openOrderAmount;

            this.AddRange(orderInfos);
        }
    }


}
