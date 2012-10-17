using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "BannerProduct", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class BannerProduct
    {
        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 1)]
        public string ProductId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PartNumber", Order = 2)]
        public string PartNumber { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ItemName", Order = 3)]
        public string ItemName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ItemNameEnglish", Order = 4)]
        public string ItemNameEnglish { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Price", Order = 5)]
        public string Price { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 6)]
        public string Currency { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PrimaryPicture", Order = 7)]
        public Picture PrimaryPicture { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 8)]
        public string DataAreaId { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "BannerList", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class BannerList
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<BannerProduct> Items { get; set; }
    }
}
