using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// árlista elem
    /// </summary>
    public class PriceListItem
    {
        public Manufacturer Manufacturer { get; set; }

        public Category FirstLevelCategory { get; set; }

        public Category SecondLevelCategory { get; set; }

        public Category ThirdLevelCategory { get; set; }

        public string ProductId { get; set; }

        public string PartNumber { get; set; }

        public string ItemName { get; set; }

        public int Stock { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string GarantyTime { get; set; }

        public string GarantyMode { get; set; }

        /// <summary>
        /// készleten van-e
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// szállítási info 
        /// </summary>
        public string ShippingInfo { get; set; }

        /// <summary>
        /// készlet info
        /// </summary>
        public string StockInfo { get; set; }

        public bool EndOfSales { get; set; }

        public bool New { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "CannotCancel", Order = 17)]
        //public bool CannotCancel { get; set; }

        public string Description { get; set; }

        public string DataAreaId { get; set; }

        public bool PurchaseInProgress { get; set; }
    }

    /// <summary>
    /// árlista
    /// </summary>
    public class PriceList
    {
        public List<PriceListItem> Items { get; set; }
    }
}
