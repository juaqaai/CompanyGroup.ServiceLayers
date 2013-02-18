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
            this.CreatedDate = invoiceInfo.CreatedDate;

            this.SalesId = invoiceInfo.SalesId;

            this.Lines = invoiceInfo.Lines.ConvertAll(x => new OrderLineInfo(x));      
        }

        public string SalesId { set; get; }

        public DateTime CreatedDate { set; get; }

        public List<CompanyGroup.WebClient.Models.OrderLineInfo> Lines { get; set; }
    }
}
