using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    //[Serializable]
    //[System.Runtime.Serialization.DataContract(Name = "BannerProduct", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class BannerProduct
    {
        public BannerProduct()
        {
           this.ProductId = String.Empty;
           this.PartNumber = String.Empty;
           this.ItemName = String.Empty;
           this.ItemNameEnglish = String.Empty;
           this.Price = String.Empty;
           this.Currency = String.Empty;
           this.PictureId = 0;
           this.DataAreaId = String.Empty;
        }

        //[System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 1)]
        public string ProductId { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "PartNumber", Order = 2)]
        public string PartNumber { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "ItemName", Order = 3)]
        public string ItemName { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "ItemNameEnglish", Order = 4)]
        public string ItemNameEnglish { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "Price", Order = 5)]
        public string Price { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "Currency", Order = 6)]
        public string Currency { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "PictureId", Order = 7)]
        public int PictureId { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 8)]
        public string DataAreaId { get; set; }
    }

    //[Serializable]
    //[System.Runtime.Serialization.DataContract(Name = "BannerList", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class BannerList
    {
        public BannerList()
        {
            this.Items = new List<BannerProduct>();
        }

        //[System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<BannerProduct> Items { get; set; }
    }
}
