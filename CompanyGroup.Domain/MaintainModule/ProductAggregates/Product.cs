using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// NoSql cikk adatok,
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Product
    {
        public Product(string productId, string axStructCode, string manufacturerId, 
            string category1Id, string category2Id, string category3Id, 
            string standardConfigId, string partNumber, string itemName, 
            string garantyTime, string garantyMode, int itemState, bool discount, bool newItem,
            int averageInventory, int price1, int price2, int price3, int price4, int price5, string currency,
            DateTime createdDate, int createdTime, DateTime modifiedDate, int modifiedTime, string dataAreaId)
        { 
            this.Id = MongoDB.Bson.ObjectId.Empty;
            this.ProductId = productId;
            this.AxStructCode = axStructCode;
            this.ManufacturerId = manufacturerId;
            this.Category1Id = category1Id;
            this.Category2Id = category2Id;
            this.Category3Id = category3Id;
            this.StandardConfigId = standardConfigId;
            this.PartNumber = partNumber;
            this.ItemName = itemName;
            this.GarantyTime = garantyTime;
            this.GarantyMode = garantyMode;
            this.ItemState = itemState;
            this.Discount = discount;
            this.New = newItem;
            this.CannotCancel = false;
            this.AverageInventory = averageInventory;
            this.Pictures = new List<CompanyGroup.Domain.MaintainModule.Picture>();
//            this.ProductManager = new CompanyGroup.Domain.MaintainModule.ProductManager(productManagerId, "", "", "", "");
            this.SecondHandList = new List<CompanyGroup.Domain.MaintainModule.SecondHand>();
            this.Price1 = price1;
            this.Price2 = price2;
            this.Price3 = price3;
            this.Price4 = price4;
            this.Price5 = price5;
            this.Currency = currency;
            this.CreatedDate = createdDate;
            this.CreatedTime = createdTime;
            this.ModifiedDate = modifiedDate;
            this.ModifiedTime = modifiedTime;
            this.DataAreaId = dataAreaId;
        }

        public Product() { }

        /// <summary>
        /// egyedi azonosító 
        /// </summary>
        public MongoDB.Bson.ObjectId Id { get; set; }

        /// <summary>
        /// cikkazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// struktúra azonosító
        /// </summary>
        public string AxStructCode { get; set; }

        /// <summary>
        /// gyártóazonosító
        /// </summary>
        public string ManufacturerId { get; set; }

        /// <summary>
        /// gyártó megnevezése
        /// </summary>
        public string ManufacturerName { get; set; }

        /// <summary>
        /// gyártó angol megnevezése
        /// </summary>
        public string ManufacturerNameEnglish { get; set; }

        /// <summary>
        /// elsődleges kategória azonosító
        /// </summary>
        public string Category1Id { get; set; }

        /// <summary>
        /// elsődleges kategória vállalatkódtól függő megnevezése
        /// hrp, bsc esetén magyar, ser esetén szerb
        /// </summary>
        public string Category1Name { get; set; }

        /// <summary>
        /// másodlagos kategória angol megnevezése
        /// </summary>
        public string Category1NameEnglish { get; set; }

        /// <summary>
        /// másoddlagos kategória azonosító
        /// </summary>
        public string Category2Id { get; set; }

        /// <summary>
        /// másodlagos kategória vállalatkódtól függő megnevezése
        /// hrp, bsc esetén magyar, ser esetén szerb
        /// </summary>
        public string Category2Name { get; set; }

        /// <summary>
        /// másodlagos kategória angol megnevezése
        /// </summary>
        public string Category2NameEnglish { get; set; }

        /// <summary>
        /// harmadlagos kategória azonosító
        /// </summary>
        public string Category3Id { get; set; }

        /// <summary>
        /// harmadlagos kategória vállalatkódtól függő megnevezése
        /// hrp, bsc esetén magyar, ser esetén szerb
        /// </summary>
        public string Category3Name { get; set; }

        /// <summary>
        /// harmadlagos kategória angol megnevezése
        /// </summary>
        public string Category3NameEnglish { get; set; }
        
        /// <summary>
        /// konfiguráció azonosítója
        /// </summary>
        public string StandardConfigId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// megnevezés
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// cikk angol neve 
        /// </summary>
        public string ItemNameEnglish { get; set; }

         /// <summary>
        /// belső raktár készlete
        /// </summary>
        public int InnerStock { get; set; }

        /// <summary>
        /// külső raktár készlete
        /// </summary>
        public int OuterStock { get; set; }

        /// <summary>
        /// egyes, azaz a legjobb ár
        /// </summary>
        public int Price1 { get; set; }

        /// <summary>
        /// kettes, azaz a legrosszabb ár
        /// </summary>
        public int Price2 { get; set; }

        /// <summary>
        /// hármas ár
        /// </summary>
        public int Price3 { get; set; }

        /// <summary>
        /// négyes ár
        /// </summary>
        public int Price4 { get; set; }

        /// <summary>
        /// ötös ár
        /// </summary>
        public int Price5 { get; set; }

        /// <summary>
        /// árhoz tartozó valutanem
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
        /// vállalt szállítási idő
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// cikk státusza (0: aktív, 1:kifutó, 2:kifutott)
        /// </summary>
        public int ItemState { get; set; }

        /// <summary>
        /// akciós cikk
        /// </summary>
        public bool Discount { get; set; }

        /// <summary>
        /// újdonság 
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// cikkhez tartozó képek listája
        /// </summary>
        public List<CompanyGroup.Domain.MaintainModule.Picture> Pictures { get; set; }

        /// <summary>
        /// cikk leírása magyar nyelven
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// cikk angol leírása
        /// </summary>
        public string DescriptionEnglish { get; set; }

        /// <summary>
        /// nem lemonható termék megrendelés esetén
        /// </summary>
        public bool CannotCancel { get; set; }

        /// <summary>
        /// átlagos készletkor
        /// </summary>
        public int AverageInventory { get; set; }

        /// <summary>
        /// beépülő objektum, a cikkhez tartozó képviselő adatait tartalmazza
        /// </summary>
        //public CompanyGroup.Domain.MaintainModule.ProductManager ProductManager { get; set; }

        /// <summary>
        /// használt készlet lista
        /// </summary>
        public List<CompanyGroup.Domain.MaintainModule.SecondHand> SecondHandList { get; set; }

        /// <summary>
        /// létrehozás dátuma
        /// </summary>
        public System.DateTime CreatedDate { get; set; }

        /// <summary>
        /// létrehozás ideje
        /// </summary>
        public int CreatedTime { get; set; }

        /// <summary>
        /// legutolsó módosítás dátuma
        /// </summary>
        public System.DateTime ModifiedDate { get; set; }

        /// <summary>
        /// legutolsó módosítás ideje
        /// </summary>
        public int ModifiedTime { get; set; }

        /// <summary>
        /// vállalatkód (hrp, bsc, ser)
        /// </summary>
        public string DataAreaId { get; set; }
    }
}
