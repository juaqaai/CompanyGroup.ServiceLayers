using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// termék entitás
    /// </summary>
    public class Product : CompanyGroup.Domain.Core.EntityBase, IValidatableObject
    {

        /// <summary>
        /// Id	ProductId	AxStructCode	DataAreaId	StandardConfigId	Name	EnglishName	PartNumber	
        /// ManufacturerId	ManufacturerName	ManufacturerEnglishName	Category1Id	Category1Name	Category1EnglishName	Category2Id	Category2Name	Category2EnglishName	Category3Id	Category3Name	Category3EnglishName	
        /// Stock	AverageInventory	Price1	Price2	Price3	Price4	Price5	Garanty	GarantyMode	Discount	New	ItemState	Description	EnglishDescription	ProductManagerId	
        /// ShippingDate	CreatedDate	Updated	Available	PictureId	SecondHand	Valid
        /// </summary> 
        public Product(int id, string productId, string axStructCode, string dataAreaId, string standardConfigId, string name, string englishName, string partNumber, 	
                       string manufacturerId, string manufacturerName, string manufacturerEnglishName, 
                       string category1Id, string category1Name, string category1EnglishName, 
                       string category2Id, string category2Name, string category2EnglishName, 
                       string category3Id, string category3Name, string category3EnglishName,	
                       int stock, int averageInventory, int price1, int price2, int price3, int price4, int price5, 
                       string garantyTime, string garantyMode, 
                       bool discount, bool newItem, int itemState, string description, string englishDescription, int productManagerId,
                       DateTime shippingDate, bool isPurchaseOrdered, DateTime createdDate, DateTime updated, bool available, int pictureId, bool secondHand, bool valid)
        {

            this.Id = id;

            this.ProductId = productId;

            this.AxStructCode = axStructCode;

            this.DataAreaId = dataAreaId;

            this.StandardConfigId = standardConfigId;

            this.ProductName = name;

            this.ProductNameEnglish = englishName;

            this.PartNumber = partNumber;

            this.Structure = new Structure(manufacturerId, manufacturerName, manufacturerEnglishName,
                                           category1Id, category1Name, category1EnglishName,
                                           category2Id, category2Name, category2EnglishName,
                                           category3Id, category3Name, category3EnglishName);

            this.Stock = stock;

            this.AverageInventory = averageInventory;

            this.Prices = new Prices(price1, price2, price3, price4, price5, "HUF");

            this.Garanty = new Garanty(garantyTime, garantyMode);

            this.Discount = discount;

            this.New = newItem;

            this.ItemState = (ItemState)itemState;

            this.Description = description;

            this.DescriptionEnglish = englishDescription;

            this.ProductManagerId = productManagerId;

            this.ShippingDate = shippingDate;

            this.IsPurchaseOrdered = isPurchaseOrdered;

            this.CreatedDate = createdDate;

            this.ModifiedDate = updated;

            this.Available = available;

            this.PictureId = pictureId;

            this.SecondHand = secondHand;

            this.Valid = valid;

            this.Comparable = false;

            this.CustomerPrice = 0;

            this.IsInCart = false;

            this.IsInNewsletter = false;
        }

        public Product() : this(0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                                   String.Empty, String.Empty, String.Empty,
                                   String.Empty, String.Empty, String.Empty,
                                   String.Empty, String.Empty, String.Empty,
                                   String.Empty, String.Empty, String.Empty,
                                   0, 0, 0, 0, 0, 0, 0,
                                   String.Empty, String.Empty,
                                   false, false, 0, String.Empty, String.Empty, 0,
                                   DateTime.MinValue, false, DateTime.MinValue, DateTime.MinValue, false, 0, false, false) { }

        /// <summary>
        /// egyedi azonosító
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        /// axapta struktúra kód
        /// </summary>
        public string AxStructCode { get; set; }

        /// <summary>
        /// axapta termékazonosító
        /// </summary>
        public string ProductId { set; get; }

        /// <summary>
        /// vállalatkód, hrp - bsc - ser
        /// </summary>
        public string DataAreaId { get; set; }

        //public int SequenceNumber { get; set; }

        /// <summary>
        /// alapértelmezett konfiguráció
        /// </summary>
        public string StandardConfigId { set; get; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        /// angol terméknév
        /// </summary>
        public string ProductNameEnglish { set; get; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { set; get; }

        /// <summary>
        /// termékstruktúra gyártó - jelleg1 - jelleg2 - jelleg3
        /// </summary>
        public Structure Structure { set; get; }

        /// <summary>
        /// készlet
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// átlagos készletkor
        /// </summary>
        public int AverageInventory { set; get; }

        /// <summary>
        /// árak 1-5 és a valutanem, melyben értelmezve van
        /// </summary>
        public Prices Prices { set; get; }

        /// <summary>
        /// garancia ideje és módja
        /// </summary>
        public Garanty Garanty { get; set; }

        /// <summary>
        /// akciós
        /// </summary>
        public bool Discount { get; set; }

        /// <summary>
        /// új flag
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// aktív, kifutó, passzív státusz az axaptából
        /// </summary>
        public ItemState ItemState { get; set; }

        /// <summary>
        /// magyar nyelvű termékleírás
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// angol nyelvű termékleírás
        /// </summary>
        public string DescriptionEnglish { set; get; }

        /// <summary>
        /// termékmanager azonosítója a Representative (képviselő) táblából
        /// </summary>
        public int ProductManagerId { set; get; }

        /// <summary>
        /// beszerzési rendelésen a szállítás dátuma
        /// </summary>
        public DateTime ShippingDate { get; set; }

        /// <summary>
        /// van-e a cikkre beszerzési rendelés?
        /// </summary>
        public bool IsPurchaseOrdered { get; set; }

        /// <summary>
        /// adatbázis bejegyzés keletkezésének dátuma
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// adatbázis bejegyzés módosításának dátuma
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// elérhető-e a termék kereskedelmi raktáron (ha kifutott, de van még eladható használt akkor FALSE)
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// elsődleges kép azonosítója a Picture táblából
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// képek lista
        /// </summary>
        public List<Picture> Pictures { get; set; }

        /// <summary>
        /// igaz, ha van a cikkből használt (SecondHand tábla), egyébként hamis
        /// </summary>
        public bool SecondHand { get; set; }	
        
        /// <summary>
        /// törölt flag (fizikai törlés nincs)
        /// </summary>
        public bool Valid { get; set; }

        #region "nem tárolt tagok"

        /// <summary>
        /// vevőre érvényes ár, kalkulált érték
        /// </summary>
        public decimal CustomerPrice { get; set; }

        /// <summary>
        /// kifutó cikk
        /// </summary>
        public bool EndOfSales
        {
            get { return CompanyGroup.Domain.Core.Adapter.ConvertItemStateEnumToBool(this.ItemState); }
        }

        /// <summary>
        /// Törlésre kerüljön-e a cikk a legutolsó betöltéshez képest (kifutó és nincs belőle készlet)
        /// </summary>
        public bool EndOfSalesNoStock
        {
            get
            {
                bool result = (this.EndOfSales) && (!this.IsInStock);

                return result;
            }
        }

         /// <summary>
        /// képek tárolása tömbben
        /// </summary>
        //public Picture[] PictureArray { get; set; }

        /// <summary>
        /// elsődleges kép, kalkulált érték
        /// </summary>
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
        //                return Factory.CreatePicture("", false, 0);
        //            }
        //        }
        //        return picture;
        //    }
        //}

        /// <summary>
        /// van-e készleten a cikk, kalkulált érték
        /// </summary>
        public bool IsInStock
        {
            get { return (this.Stock > 0); }
        }

        /// <summary>
        /// benne van-e a kosárban a termék?
        /// </summary>
        public bool IsInCart { get; set; }

        /// <summary>
        /// összehasonlítható-e a termék?
        /// </summary>
        public bool Comparable { get; set; }

        /// <summary>
        /// hírlevélben benne van-e a termék?
        /// </summary>
        public bool IsInNewsletter { get; set; }

        /// <summary>
        /// ha nincs készleten a cikk, akkor beszerzési rendelés van-e a cikkre, vagy nincs?
        /// </summary>
        /// <returns></returns>
        public bool PurchaseInProgress() 
        {
            return (!this.IsInStock) && (this.IsPurchaseOrdered); //!(this.ShippingDate.Equals(DateTime.MinValue)); //!(this.ShippingDate.Year.Equals(1) && this.ShippingDate.Month.Equals(1) && this.ShippingDate.Day.Equals(1));
        }

        /// <summary>
        /// beérkezés információ
        /// </summary>
        /// <returns></returns>
        public string ShippingInfo()
        {
            string result = "Több mint négy hét";

            //nincs készlet, de van beszerzési rendelés
            if (this.PurchaseInProgress())
            { 
                DateTime now = DateTime.Now;

                double substract = (this.ShippingDate - now).TotalDays;

                if (substract > 0 && substract < 4)
                {
                    result = "3 napon belül";
                }
                else if (substract > 4 && substract < 8)
                {
                    result = "1 héten belül";
                }
                else if (substract > 7 && substract < 15)
                {
                    result = "2 héten belül";
                }
                else if (substract > 14 && substract < 22)
                {
                    result = "3 héten belül";
                }
                else if (substract > 21 && substract < 29)
                {
                    result = "4 héten belül";
                }
                else
                {
                    result = "több mint 4 hét";
                }

            }
            else  //nincs beszerzési rendelés 
            {
                result = this.StockInfo();
            }

            return result;
        }

        /// <summary>
        /// készletinformáció
        /// </summary>
        /// <returns></returns>
        public string StockInfo()
        {
            string tmp = (this.Stock > 19) ? "> 20" : String.Format("{0}", this.Stock);

            return (!this.IsInStock) ? "Rendelhető" : tmp;
        }

        /// <summary>
        /// visszaadja a termékhez tartozó raktárkódot
        /// </summary>
        public string InventLocationId
        {
            get 
            {
                string inventLocationId = String.Empty; 
                //hrp-s cikk
                if (this.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, StringComparison.OrdinalIgnoreCase))
                {
                    inventLocationId = (this.SecondHand) ? CompanyGroup.Domain.Core.Constants.SecondhandStoreHrp : CompanyGroup.Domain.Core.Constants.OuterStockHrp;
                }
                else
                {
                    inventLocationId = (this.SecondHand) ? CompanyGroup.Domain.Core.Constants.SecondhandStoreBsc : CompanyGroup.Domain.Core.Constants.OuterStockBsc;
                }
                return inventLocationId;
            }
        }

        /// <summary>
        /// nem lemonható termék megrendelés esetén
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("CannotCancel", Order = 18)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public bool CannotCancel { get; set; }

        /// <summary>
        /// cikk termékmenedzser
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("ProductManager", Order = 25)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public CompanyGroup.Domain.PartnerModule.ProductManager ProductManager { get; set; }

        public List<CompanyGroup.Domain.WebshopModule.SecondHand> SecondHandList 
        {
            get { return this.SecondHandArray.ToList(); }
            set { this.SecondHandArray = value.ToArray(); } 
        }

        /// <summary>
        /// használt cikkek
        /// </summary>
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

        #endregion

        #region "EntityBase metódusok"

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return String.IsNullOrWhiteSpace(this.ProductId);
        }


        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Product))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            Product item = (Product)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.ProductId == this.ProductId;
            }
        }

        /// <summary>
        /// hash code előállítás
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.ProductId.GetHashCode() ^ 31;
        }

        #endregion
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
        /// eltávolítja a logikailag törölt elemeket a listából
        /// </summary>
        public void RemoveEndOfSalesNoStock()
        {
            this.RemoveAll(x => x.EndOfSalesNoStock);
        }

        public void RemoveNoSecondHand()
        {
            this.RemoveAll(x => (!x.SecondHand));
        } 

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
