using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Product", Namespace = "CompanyGroup.GlobalServices.Dto")]
    public class Product
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

        [System.Runtime.Serialization.DataMember(Name = "Stock", Order = 9)]
        public int Stock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 11)]
        public decimal Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyTime", Order = 13)]
        public string GarantyTime { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyMode", Order = 14)]
        public string GarantyMode { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ShippingDate", Order = 15)]
        public string ShippingDate { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "EndOfSales", Order = 16)]
        public bool EndOfSales { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "New", Order = 17)]
        public bool New { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInNewsletter", Order = 18)]
        public bool IsInNewsletter { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "CannotCancel", Order = 21)]
        //public bool CannotCancel { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Pictures", Order = 23)]
        public Pictures Pictures { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Description", Order = 24)]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 27)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PurchaseInProgress", Order = 30)]
        public bool PurchaseInProgress { get; set; }
    }
}
