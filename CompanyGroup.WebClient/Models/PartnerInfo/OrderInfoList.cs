using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés info lista
    /// </summary>
    public class OrderInfoList
    {
        public OrderInfoList(List<CompanyGroup.WebClient.Models.OrderInfo> items)
        {
            this.Items = items;
        }

        public List<CompanyGroup.WebClient.Models.OrderInfo> Items { get; set; }
    }
}
