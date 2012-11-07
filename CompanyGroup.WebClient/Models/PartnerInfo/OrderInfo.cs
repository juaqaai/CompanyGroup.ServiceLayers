using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// megrendelés info
    /// </summary>
    public class OrderInfo
    {
        public OrderInfo(CompanyGroup.Dto.PartnerModule.OrderInfo invoiceInfo)
        { 
            CreatedDate = invoiceInfo.CreatedDate;
            SalesId = invoiceInfo.SalesId;
            Lines = invoiceInfo.Lines.ConvertAll(x => new OrderLineInfo(x));      
        }

        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public List<CompanyGroup.WebClient.Models.OrderLineInfo> Lines { get; set; }
    }
}
