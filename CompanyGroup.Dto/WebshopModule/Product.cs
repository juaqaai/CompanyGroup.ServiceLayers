using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// termék DTO
    /// </summary>
    public class Product
    {
        /// <summary>
        /// gyártó
        /// </summary>
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// jelleg1
        /// </summary>
        public Category FirstLevelCategory { get; set; }

        /// <summary>
        /// jelleg2
        /// </summary>
        public Category SecondLevelCategory { get; set; }

        /// <summary>
        /// jelleg3
        /// </summary>
        public Category ThirdLevelCategory { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// cikk neve
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// cikk neve angolul
        /// </summary>
        public string ItemNameEnglish { get; set; }

        /// <summary>
        /// készlet
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// termék ára
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// garancia ideje
        /// </summary>
        public string GarantyTime { get; set; }

        /// <summary>
        /// garancia módja 
        /// </summary>
        public string GarantyMode { get; set; }

        /// <summary>
        /// várhetó szállítás időpontja
        /// </summary>
        public string ShippingDate { get; set; }

        /// <summary>
        /// kifutó
        /// </summary>
        public bool EndOfSales { get; set; }

        /// <summary>
        /// új
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// akciós
        /// </summary>
        public bool Discount { get; set; }

        /// <summary>
        /// hírlevélben van-e a termék?
        /// </summary>
        public bool IsInNewsletter { get; set; }

        /// <summary>
        /// kosárban van-e a termék
        /// </summary>
        public bool IsInCart { get; set; }

        /// <summary>
        /// összehasonlítható-e a termék
        /// </summary>
        public bool Comparable { get; set; }

        //public bool CannotCancel { get; set; }

        /// <summary>
        /// elsődleges termékazonosító
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Képeket tartalmazó lista (csak a termék adatlap lekérdezésekor tartalmaz elemeket)
        /// </summary>
        public Pictures Pictures { get; set; }

        /// <summary>
        /// termékleírás
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// angol termékleírás
        /// </summary>
        public string DescriptionEnglish { get; set; }

        //public CompanyGroup.Dto.PartnerModule.ProductManager ProductManager { get; set; }

        /// <summary>
        /// vállalatkód (ahonnan a terméket meg lehet vásárolni)
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// készleten van-e
        /// </summary>
        public bool IsInStock { get; set; }

        //public int SequenceNumber { get; set; }

        /// <summary>
        /// beszerzési rendelés folyamatban
        /// </summary>
        public bool PurchaseInProgress { get; set; }

        /// <summary>
        /// használt lista
        /// </summary>
        public SecondHandList SecondHandList { get; set; }

        /// <summary>
        /// elérhető-e a termék kereskedelmi raktáron (ha kifutott, de van még eladható használt akkor FALSE)
        /// </summary>
        public bool Available { get; set; }
    }
}
