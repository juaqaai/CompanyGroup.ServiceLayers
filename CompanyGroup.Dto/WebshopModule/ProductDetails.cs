using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ProductDetails", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ProductDetails
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

        [System.Runtime.Serialization.DataMember(Name = "ItemNameEnglish", Order = 8)]
        public string ItemNameEnglish { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InnerStock", Order = 9)]
        public int InnerStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OuterStock", Order = 10)]
        public int OuterStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 11)]
        public string Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 12)]
        public string Currency { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyTime", Order = 13)]
        public string GarantyTime { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "GarantyMode", Order = 14)]
        public string GarantyMode { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ShippingDate", Order = 15)]
        public DateTime ShippingDate { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "EndOfSales", Order = 16)]
        public bool EndOfSales { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "New", Order = 17)]
        public bool New { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInNewsletter", Order = 18)]
        public bool IsInNewsletter { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInCart", Order = 19)]
        public bool IsInCart { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Comparable", Order = 20)]
        public bool Comparable { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CannotCancel", Order = 21)]
        public bool CannotCancel { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PrimaryPicture", Order = 22)]
        public Picture PrimaryPicture { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Pictures", Order = 23)]
        public Pictures Pictures { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Description", Order = 24)]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DescriptionEnglish", Order = 25)]
        public string DescriptionEnglish { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ProductManager", Order = 26)]
        public CompanyGroup.Dto.PartnerModule.ProductManager ProductManager { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 27)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInStock", Order = 28)]
        public bool IsInStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "SequenceNumber", Order = 29)]
        public int SequenceNumber { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PurchaseInProgress", Order = 30)]
        public bool PurchaseInProgress { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "SecondHandList", Order = 31)]
        public SecondHandList SecondHandList { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CompatibilityList", Order = 32)]
        public CompatibleProduct[] CompatibilityList { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ReverseCompatibilityList", Order = 33)]
        public CompatibleProduct[] ReverseCompatibilityList { get; set; }
    }
}
