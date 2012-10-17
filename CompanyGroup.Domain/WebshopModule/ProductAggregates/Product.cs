using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// termék entitás, ősosztály definiálja az ObjectId-t
    /// </summary>
    public class Product : CompanyGroup.Domain.Core.NoSqlEntity, IValidatableObject
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ProductId", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public int SequenceNumber { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ProductName", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductName { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("ProductNameEnglish", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ProductNameEnglish { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("PartNumber", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string PartNumber { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Description", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Description { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("DescriptionEnglish", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string DescriptionEnglish { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Structure", Order = 8)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Structure Structure { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("StandardConfigId", Order = 9)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string StandardConfigId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("AverageInventory", Order = 10)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int AverageInventory { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Prices", Order = 11)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Prices Prices { set; get; }

        /// <summary>
        /// vevőre érvényes ár, kalkulált érték
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public decimal CustomerPrice { get; set; }

        /// <summary>
        /// azért van, hogy listában lehessen elkérni a képeket
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public List<Picture> Pictures 
        { 
            get { return this.PictureArray.ToList(); }
            set { this.PictureArray = value.ToArray(); } 
        }

        /// <summary>
        /// képek tárolása tömbben
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Pictures", Order = 12)]
        public Picture[] PictureArray { get; set; }

        /// <summary>
        /// elsődleges kép, kalkulált érték
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public Picture PrimaryPicture
        { 
            get  
            {
                CompanyGroup.Domain.WebshopModule.Picture picture = this.Pictures.Find(x => x.Primary);

                // ha nincs elsődleges beállítás, akkor az első elemet kell visszaadni
                if (picture == null)
                {
                    picture = this.Pictures.FirstOrDefault();

                    //ha nincsen egyetlen elem sem, akkor új kép objektumot kell visszaadni
                    if (picture == null)
                    {
                        return new Picture();
                    }
                }
                return picture;
            }
        }

        /// <summary>
        /// benne van a hírlevélben, új cikk, készleten van
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Flags", Order = 13)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public Flags Flags { get; set; }

        /// <summary>
        /// új flag
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("New", Order = 13)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool New { get; set; }

        /// <summary>
        /// akciós
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Discount", Order = 14)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool Discount { get; set; }

        /// <summary>
        /// készletek, hrp - bsc - ser
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Stock", Order = 15)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Stock Stock { get; set; }

        /// <summary>
        /// van-e készleten a cikk, kalkulált érték
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsInStock
        {
            get
            {
                if (this.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc) || this.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp))
                {
                    return (this.Stock.Inner + this.Stock.Outer > 0);
                }
                else
                {
                    return (this.Stock.Inner + this.Stock.Outer + this.Stock.Serbian > 0);
                }
            }
        }

        /// <summary>
        /// aktív, kifutó, passzív
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ItemState", Order = 16)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public ItemState ItemState { get; set; }

        ///// <summary>
        ///// kosárban benne van-e a cikk, kalkulált érték
        ///// </summary>
        //public bool IsInCart(ShoppingCartCollection shoppingCartCollection)
        //{
        //    if (shoppingCartCollection == null || !shoppingCartCollection.ExistsItem)
        //    {
        //        return false;
        //    }

        //    return shoppingCartCollection.IsInCart(this);
        //}

        ///// <summary>
        ///// összehasonlítható-e az adott cikk, kalkulált érték
        ///// </summary>
        //public bool Comparable(ComparableCollection comparableCollection)
        //{
        //    return comparableCollection.Comparable(this);
        //}

        ///// <summary>
        ///// benne van-e az adott cikk a hírlevél listában, vagy sem
        ///// </summary>
        ///// <param name="newsletterCollection"></param>
        ///// <returns></returns>
        //public bool IsInNewsletter(NewsletterCollection newsletterCollection)
        //{
        //    return newsletterCollection.IsInNewsletter(this);
        //}

        /// <summary>
        /// benne van-e a kosárban a termék?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsInCart { get; set; }

        /// <summary>
        /// összehasonlítható-e a termék?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool Comparable { get; set; }

        /// <summary>
        /// hírlevélben benne van-e a termék?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsInNewsletter { get; set; }

        /// <summary>
        /// beszerzési rendelésen a szállítás dátuma
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ShippingDate", Order = 17)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// beszerzési rendelés van-e a cikkre, vagy nincs?
        /// </summary>
        /// <returns></returns>
        public bool PurchaseInProgress() 
        {
            return !(this.ShippingDate.Equals(DateTime.MinValue)); //!(this.ShippingDate.Year.Equals(1) && this.ShippingDate.Month.Equals(1) && this.ShippingDate.Day.Equals(1));
        }

        /// <summary>
        /// nem lemonható termék megrendelés esetén
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CannotCancel", Order = 18)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool CannotCancel { get; set; }

        /// <summary>
        /// adatbázis bejegyzés keletkezésének dátuma
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CreatedDate", Order = 19)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// adatbázis bejegyzés keletkezésének ideje
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CreatedTime", Order = 20)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int CreatedTime { get; set; }

        /// <summary>
        /// adatbázis bejegyzés módosításának dátuma
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ModifiedDate", Order = 21)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// adatbázis bejegyzés módosításának ideje
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ModifiedTime", Order = 22)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public int ModifiedTime { get; set; }

        /// <summary>
        /// vállalatkód, hrp (0) - bsc (1) - ser (2)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("DataAreaId", Order = 23)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string DataAreaId { get; set; }

        /// <summary>
        /// garancia dátuma, ideje és módja
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Garanty", Order = 24)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Garanty Garanty { get; set; }

        /// <summary>
        /// cikk termékmenedzser
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ProductManager", Order = 25)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CompanyGroup.Domain.PartnerModule.ProductManager ProductManager { get; set; }

        /// <summary>
        /// használt cikk (állapottól függ, több is lehet)
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("SecondHandList", Order = 25)]
        
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public List<CompanyGroup.Domain.WebshopModule.SecondHand> SecondHandList 
        {
            get { return this.SecondHandArray.ToList(); }
            set { this.SecondHandArray = value.ToArray(); } 
        }

        /// <summary>
        /// használt cikkek
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SecondHandList", Order = 26)]
        public CompanyGroup.Domain.WebshopModule.SecondHand[] SecondHandArray { get; set; }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(ProductId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ProductId" }));
            }

            return validationResults;
        }

        public Product()
        {
            Comparable = false;

            CustomerPrice = 0;

            IsInCart = false;

            IsInNewsletter = false;
        }
    }

    /// <summary>
    /// domain product entitás lista
    /// </summary>
    public class Products : List<Product> 
    {
        /// <summary>
        /// listaelem számláló
        /// </summary>
        public long ListCount { get; set; }

        /// <summary>
        /// lista lapozó
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// aktuális sorrend 
        /// </summary>
        public Sequence CurrentSequence { get; set; }

        /// <summary>
        /// lista nyitott állapotban van-e?
        /// </summary>
        public bool ListStatusOpen { get; set; }

        /// <summary>
        /// konstruktor lapozóval   
        /// </summary>
        /// <param name="pager"></param>
        public Products(Pager pager)
        {
            this.Pager = pager;
        }

    }


}
