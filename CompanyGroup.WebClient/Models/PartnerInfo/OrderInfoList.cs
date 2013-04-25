using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés info lista
    /// </summary>
    public class OrderInfoList
    {
        public OrderInfoList(List<CompanyGroup.Dto.PartnerModule.OrderInfo> items, decimal openOrderAmount, Visitor visitor)
        {
            this.OpenOrderAmount = openOrderAmount;

            this.Items = items;

            this.Visitor = visitor;
        }

        public decimal OpenOrderAmount { get; set; }

        public List<CompanyGroup.Dto.PartnerModule.OrderInfo> Items { get; set; }

        public bool HasItems { get { return Items.Count > 0; } }

        public Visitor Visitor { get; set; }
    }
}
