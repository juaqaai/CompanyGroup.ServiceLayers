using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    public class CompatibleProduct
    {
        public string ProductId { get; set; }

        public string PartNumber { get; set; }

        public string ItemName { get; set; }

        public string ItemNameEnglish { get; set; }

        public int Stock { get; set; }

        public string Price { get; set; }

        public string Currency { get; set; }

        public DateTime ShippingDate { get; set; }

        public string Description { get; set; }

        public string DescriptionEnglish { get; set; }

        public string DataAreaId { get; set; }

        public bool IsInStock { get; set; }

        public bool PurchaseInProgress { get; set; }
    }
}
