using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "CompatibleProduct", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class CompatibleProduct
    {
        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 1)]
        public string ProductId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PartNumber", Order = 2)]
        public string PartNumber { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ItemName", Order = 3)]
        public string ItemName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ItemNameEnglish", Order = 4)]
        public string ItemNameEnglish { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InnerStock", Order = 5)]
        public int InnerStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OuterStock", Order = 6)]
        public int OuterStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 7)]
        public string Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 8)]
        public string Currency { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ShippingDate", Order = 9)]
        public DateTime ShippingDate { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Description", Order = 10)]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DescriptionEnglish", Order = 11)]
        public string DescriptionEnglish { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 12)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "IsInStock", Order = 13)]
        public bool IsInStock { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PurchaseInProgress", Order = 14)]
        public bool PurchaseInProgress { get; set; }
    }
}
