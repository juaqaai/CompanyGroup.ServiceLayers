using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "PriceListItem", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class PriceListItem
    {
        [System.Runtime.Serialization.DataMember(Name = "Manufacturer", Order = 1)]
        public Manufacturer Manufacturer { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "FirstLevelCategory", Order = 2)]
        public Category FirstLevelCategory { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "SecondLevelCategory", Order = 3)]
        public Category SecondLevelCategory { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ThirdLevelCategory", Order = 4)]
        public Category ThirdLevelCategory { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 5)]
        public string ProductId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PartNumber", Order = 6)]
        public string PartNumber { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ItemName", Order = 7)]
        public string ItemName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InnerStock", Order = 8)]
        public int InnerStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OuterStock", Order = 9)]
        public int OuterStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 10)]
        public decimal Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 11)]
        public string Currency { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyTime", Order = 12)]
        public string GarantyTime { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyMode", Order = 13)]
        public string GarantyMode { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ShippingDate", Order = 14)]
        public DateTime ShippingDate { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "EndOfSales", Order = 15)]
        public bool EndOfSales { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "New", Order = 16)]
        public bool New { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CannotCancel", Order = 17)]
        public bool CannotCancel { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Description", Order = 18)]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 19)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PurchaseInProgress", Order = 20)]
        public bool PurchaseInProgress { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "PriceList", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class PriceList
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<PriceListItem> Items { get; set; }
    }
}
