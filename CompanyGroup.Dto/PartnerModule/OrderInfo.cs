using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// megrendelés információ  
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// VR, vagy BR azonosító
        /// </summary>
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        public string CreatedDate { set; get; }

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
        /// rendelés netto összesen
        /// </summary>
        public string SumAmountNetto { get; set; }

        /// <summary>
        /// rendelés brutto összesen
        /// </summary>
        public string SumAmountBrutto { get; set; }

        /// <summary>
        /// sorok
        /// </summary>
        public List<OrderLineInfo> Lines { get; set; }
    }

    /// <summary>
    /// megrendelés lista
    /// </summary>
    public class OrderInfoList
    {
        public OrderInfoList(decimal openOrderAmount, List<OrderInfo> items)
        {
            this.OpenOrderAmount = openOrderAmount;

            this.Items = items;
        }

        public decimal OpenOrderAmount { get; set; }

        public List<OrderInfo> Items { get; set; }
    }
}
