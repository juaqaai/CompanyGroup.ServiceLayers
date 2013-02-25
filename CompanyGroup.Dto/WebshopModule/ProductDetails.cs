using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class ProductDetails
    {
        public Manufacturer Manufacturer { get; set; }

        public Category FirstLevelCategory { get; set; }

        public Category SecondLevelCategory { get; set; }

        public Category ThirdLevelCategory { get; set; }

        public string ProductId { get; set; }

        public string PartNumber { get; set; }

        public string ItemName { get; set; }

        public string ItemNameEnglish { get; set; }

        public int Stock { get; set; }

        public string Price { get; set; }

        public string Currency { get; set; }

        public string GarantyTime { get; set; }

        public string GarantyMode { get; set; }

        public DateTime ShippingDate { get; set; }

        public bool EndOfSales { get; set; }

        public bool New { get; set; }

        public bool IsInNewsletter { get; set; }

        public bool IsInCart { get; set; }

        public bool Comparable { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "CannotCancel", Order = 21)]
        //public bool CannotCancel { get; set; }

        public int PictureId { get; set; }

        public Pictures Pictures { get; set; }

        public string Description { get; set; }

        public string DescriptionEnglish { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "ProductManager", Order = 26)]
        //public CompanyGroup.Dto.PartnerModule.ProductManager ProductManager { get; set; }

        public string DataAreaId { get; set; }

        public bool IsInStock { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "SequenceNumber", Order = 29)]
        //public int SequenceNumber { get; set; }

        public bool PurchaseInProgress { get; set; }

        public SecondHandList SecondHandList { get; set; }

        public List<CompatibleProduct> CompatibilityList { get; set; }

        public List<CompatibleProduct> ReverseCompatibilityList { get; set; }
    }
}
