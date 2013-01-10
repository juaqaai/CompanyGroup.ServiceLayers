using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// bevásárló kosár elem
    /// </summary>
    public class ShoppingCartItem : CompanyGroup.Domain.Core.EntityBase, IValidatableObject
    {
        /// <summary>
        /// konstruktor, üres kosár elem létrehozása (SetProduct értékadások előkészítése)
        /// </summary>
        public ShoppingCartItem()
        {
            this.ProductId = String.Empty;

            this.ProductName = String.Empty;

            this.ProductNameEnglish = String.Empty;

            this.PartNumber = String.Empty;

            //this.Structure = new Structure()
            //{
            //    Category1 = new Category() { CategoryEnglishName = "", CategoryId = "", CategoryName = "" },
            //    Category2 = new Category() { CategoryEnglishName = "", CategoryId = "", CategoryName = "" },
            //    Category3 = new Category() { CategoryEnglishName = "", CategoryId = "", CategoryName = "" },
            //    Manufacturer = new Manufacturer() { ManufacturerEnglishName = "", ManufacturerId = "", ManufacturerName = "" }
            //};

            this.CustomerPrice = 0;

            //this.Currency = String.Empty;

            //this.Pictures = new Pictures();

            //this.Stock = new Stock() { Inner = 0, Outer = 0, Serbian = 0 };

            this.ItemState = ItemState.Active;

            //this.ShippingDate = DateTime.MinValue;

            this.DataAreaId = "";

            //this.Garanty = new Garanty() { Mode = String.Empty, Time = String.Empty };

            this.Quantity = 0;

            this.Status = CartItemStatus.Created;

            this.CreatedDate = DateTime.Now;

            //this.ModifiedDate = DateTime.MinValue;
        }

        public int LineId { get; set; }

        public int CartId { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ProductId", Order = 2)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductId { get; set; }

        /// <summary>
        /// terméknév
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ProductName", Order = 3)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductName { get; set; }

        /// <summary>
        /// terméknév angolul
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ProductNameEnglish", Order = 4)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductNameEnglish { set; get; }

        /// <summary>
        /// cikkszám
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("PartNumber", Order = 5)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string PartNumber { get; set; }

        ///// <summary>
        ///// termékstruktúra
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Structure", Order = 6)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public Structure Structure { set; get; }

        /// <summary>
        /// konfiguráció, ahonnan a termék származik
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ConfigId", Order = 7)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ConfigId { set; get; }

        /// <summary>
        /// vevő ára
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("CustomerPrice", Order = 8)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int CustomerPrice { get; set; }

        /// <summary>
        /// ár valutaneme
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Currency", Order = 9)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public string Currency { get; set; }

        ///// <summary>
        ///// azért van, hogy listában lehessen elkérni a képeket
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        //public List<Picture> Pictures
        //{
        //    get { return this.PictureArray.ToList(); }
        //    set { this.PictureArray = value.ToArray(); }
        //}

        ///// <summary>
        ///// képek tárolása tömbben
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Pictures", Order = 10)]
        //public Picture[] PictureArray { get; set; }

        ///// <summary>
        ///// elsődleges kép, kalkulált érték
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        //public Picture PrimaryPicture
        //{
        //    get
        //    {
        //        CompanyGroup.Domain.WebshopModule.Picture picture = this.Pictures.Find(x => x.Primary);

        //        // ha nincs elsődleges beállítás, akkor az első elemet kell visszaadni
        //        if (picture == null)
        //        {
        //            picture = this.Pictures.FirstOrDefault();

        //            //ha nincsen egyetlen elem sem, akkor új kép objektumot kell visszaadni
        //            if (picture == null)
        //            {
        //                return new Picture();
        //            }
        //        }
        //        return picture;
        //    }
        //}

        ///// <summary>
        ///// flag-ek
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Flags", Order = 11)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public Flags Flags { get; set; }

        /// <summary>
        /// készlet - hrp, bsc, külső, belső, szerbiai
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Stock", Order = 12)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Stock Stock { get; set; }

        /// <summary>
        /// cikk státusza (aktív, passzív, kifutó)
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ItemState", Order = 13)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public ItemState ItemState { get; set; }

        /// <summary>
        /// szállítás dátuma, ha nincs készleten és van rá beszerzési rendelés
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ShippingDate", Order = 14)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public DateTime ShippingDate { get; set; }

        /// <summary>
        /// vállalat
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("DataAreaId", Order = 15)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string DataAreaId { get; set; }

        /// <summary>
        /// garancia ideje és módja
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Garanty", Order = 16)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public Garanty Garanty { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Quantity", Order = 17)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int Quantity { get; set; }

        /// <summary>
        /// kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Status", Order = 18)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CartItemStatus Status { get; set; }

        /// <summary>
        /// adatbázis bejegyzés keletkezésének dátuma
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("CreatedDate", Order = 19)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// adatbázis bejegyzés módosításának dátuma
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ModifiedDate", Order = 20)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// termék beállítása
        /// </summary>
        /// <param name="product"></param>
        public void SetProduct(Product product)
        {
            this.ProductId = product.ProductId;

            this.ProductName = product.ProductName;

            this.ProductNameEnglish = product.ProductNameEnglish;

            this.PartNumber = product.PartNumber;

            this.CustomerPrice = Convert.ToInt32(product.CustomerPrice);

            //this.Pictures.AddRange(product.Pictures);

            //this.Flags = product.Flags;

            this.Stock = product.Stock;

            this.ItemState = product.ItemState;

            //this.ShippingDate = product.ShippingDate;

            this.DataAreaId = product.DataAreaId;

            //this.Garanty = product.Garanty;

            this.Quantity = 1;

            //this.Structure = product.Structure;

            this.ConfigId = product.StandardConfigId;

            //this.Currency = product.Prices.Currency;

        }

        ///// <summary>
        ///// beszerzési rendelés folyamatban
        ///// </summary>
        ///// <returns></returns>
        //public bool PurchaseInProgress()
        //{
        //    return !(this.ShippingDate.Year.Equals(1899) && this.ShippingDate.Month.Equals(12) && this.ShippingDate.Day.Equals(31));
        //}

        ///// <summary>
        ///// készleten van-e?
        ///// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        //public bool IsInStock
        //{
        //    get
        //    {
        //        if (this.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc) || this.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp))
        //        {
        //            return (this.Stock.Inner + this.Stock.Outer > 0);
        //        }
        //        else
        //        {
        //            return (this.Stock.Inner + this.Stock.Outer + this.Stock.Serbian > 0);
        //        }
        //    }
        //}

        /// <summary>
        /// cikk ára összesen (egységár * mennyiség)
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public double ItemTotal 
        {
            get { return this.CustomerPrice * this.Quantity; }
        }

        /// <summary>
        /// érvényesség ellenőrzés  
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(this.ProductId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ProductId" }));
            }

            return validationResults;
        }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return this.LineId == 0;
        }


        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ShoppingCartItem))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            ShoppingCartItem item = (ShoppingCartItem)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.LineId == this.LineId;
            }
        }

        /// <summary>
        /// hash code előállítás
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.LineId.GetHashCode() ^ 31;
        }
    }
}
