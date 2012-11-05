using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés info
    /// </summary>
    public class OrderInfo
    {
        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public List<CompanyGroup.WebClient.Models.OrderLineInfo> Lines { get; set; }
    }
}
