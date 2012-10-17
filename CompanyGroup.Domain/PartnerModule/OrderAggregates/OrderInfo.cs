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
        public OrderInfo(string salesId, DateTime createdDate)
        { 
            this.SalesId = salesId;

            this.CreatedDate = createdDate;

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
        /// rendelés sorok
        /// </summary>
        public List<OrderLineInfo> OrderLines { set; get; }

        /// <summary>
        /// rendelés sorok összesen 
        /// </summary>
        public decimal SumAmount
        {
            get { return this.OrderLines.Sum(x => x.LineAmount); }
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
                    orderInfo = new OrderInfo(x.SalesId, x.CreatedDate);
                }

                OrderLineInfo orderLineInfo = new OrderLineInfo(x.LineNum, x.ShippingDateRequested, x.ItemId, x.Name, x.SalesPrice, x.CurrencyCode, x.Quantity, x.LineAmount, x.SalesDeliverNow, x.RemainSalesPhysical, x.StatusIssue, x.InventLocationId, x.Payment);

                orderInfo.AddOrderLine(orderLineInfo);
            });

            return orderInfo;
        }

    }


}
