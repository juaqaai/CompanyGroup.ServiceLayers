using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Model
{
    public class SalesOrder
    {
        public string SalesId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string  InventLocationId { get; set; }

        public string  Payment { get; set; }

        public int PartialDischarge { get; set; }

        public int AutoDelete { get; set; }

        public int SalesSource { get; set; }

        public int HeaderSalesStatus { get; set; }

        DateTime ShippingDateRequested { get; set; }

        public int? LineNum { get; set; }

        public int SalesStatus { get; set; }

        public string  ProductId { get; set; }

        public string  ProductName { get; set; }

        public int? Quantity { get; set; }

        public int? SalesPrice { get; set; }

        public int LineAmount { get; set; }

        public int? SalesDeliverNow { get; set; }

        public int? RemainSalesPhysical { get; set; }

        public int StatusIssue { get; set; }

        public int? RecId { get; set; }

        public string  ItemInventLocationId { get; set; }

        public string  CurrencyCod { get; set; }
    }
}
