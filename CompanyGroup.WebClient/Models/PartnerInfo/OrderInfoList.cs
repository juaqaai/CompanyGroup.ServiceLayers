using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés info lista
    /// </summary>
    public class OrderInfoList
    {
        public OrderInfoList(List<CompanyGroup.WebClient.Models.OrderInfo> items, Visitor visitor)
        {
            this.Items = items;

            this.Visitor = visitor;
        }

        public List<CompanyGroup.WebClient.Models.OrderInfo> Items { get; set; }

        public bool HasItems { get { return Items.Count > 0; } }

        public Visitor Visitor { get; set; }
    }
}
