using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// kosár lista sor
    /// </summary>
    public class ShoppingCartItem
    {
        /// <summary>
        /// kosár elem azonosító
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// vevő ára
        /// </summary>
        public double CustomerPrice { get; set; }

        /// <summary>
        /// ár valutaneme
        /// </summary>
        public string Currency { get; set; }

        ///// <summary>
        ///// készletérték
        ///// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// elem státusza
        /// </summary>
        public int ItemState { get; set; }

        ///// <summary>
        ///// szállítás várható időpontja
        ///// </summary>
        //public DateTime ShippingDate { get; set; }

        /// <summary>
        /// vállalat
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// konfiguráció, ahonnan a termék származik (ALAP, XX)
        /// </summary>
        public string ConfigId { set; get; }

        /// <summary>
        /// raktárkód (KULSO, vagy 7000, HASZNALT)
        /// </summary>
        public string InventLocationId { set; get; }

        /// <summary>
        /// használt konfigon van-e a termék?
        /// </summary>
        public bool IsInSecondHand { set; get; }

        ///// <summary>
        ///// garancia ideje, módja
        ///// </summary>
        //public Garanty Garanty { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// kosár elem összesen (menyiség * egységár)
        /// </summary>
        public double ItemTotal { get; set; }

        ///// <summary>
        ///// készleten van-e a cikk
        ///// </summary>
        //public bool IsInStock { get; set; }

        ///// <summary>
        ///// van-e a cikkre beszerzési rendelés?
        ///// </summary>
        //public bool PurchaseInProgress { get; set; }
    }
}
