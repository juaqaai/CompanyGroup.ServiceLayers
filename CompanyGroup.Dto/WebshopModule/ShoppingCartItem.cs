using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCartItem", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCartItem
    {
        /// <summary>
        /// kosár elem azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 2)]
        public string ProductId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PartNumber", Order = 3)]
        public string PartNumber { get; set; }

        /// <summary>
        /// terméknév
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ProductName", Order = 4)]
        public string ProductName { get; set; }

        /// <summary>
        /// vevő ára
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CustomerPrice", Order = 5)]
        public double CustomerPrice { get; set; }

        /// <summary>
        /// ár valutaneme
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 6)]
        public string Currency { get; set; }

        ///// <summary>
        ///// készletek
        ///// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "Stock", Order = 6)]
        public Stock Stock { get; set; }

        /// <summary>
        /// elem státusza
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ItemState", Order = 7)]
        public int ItemState { get; set; }

        ///// <summary>
        ///// szállítás várható időpontja
        ///// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "ShippingDate", Order = 8)]
        //public DateTime ShippingDate { get; set; }

        /// <summary>
        /// vállalat
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 8)] 
        public string DataAreaId { get; set; }

        ///// <summary>
        ///// garancia ideje, módja
        ///// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "Garanty", Order = 10)]
        //public Garanty Garanty { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Quantity", Order = 10)]
        public int Quantity { get; set; }

        /// <summary>
        /// kosár elem összesen (menyiség * egységár)
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ItemTotal", Order = 11)]
        public double ItemTotal { get; set; }

        ///// <summary>
        ///// készleten van-e a cikk
        ///// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "IsInStock", Order = 13)]
        //public bool IsInStock { get; set; }

        ///// <summary>
        ///// van-e a cikkre beszerzési rendelés?
        ///// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "PurchaseInProgress", Order = 14)]
        //public bool PurchaseInProgress { get; set; }
    }
}
