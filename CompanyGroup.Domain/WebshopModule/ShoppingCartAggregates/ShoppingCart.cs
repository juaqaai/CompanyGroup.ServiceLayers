using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// bevásárló kosár
    /// regisztrált felhasználónként egy lehet aktív, de kosár számosságára tekintve nincs megkötés. (Felső limit 10 db)
    /// </summary>
    public class ShoppingCart : CompanyGroup.Domain.Core.NoSqlEntity, IValidatableObject
    {

        /// <summary>
        ///  konstruktor, létrehozza az elemek listát üresen
        ///  beállítja a látogató azonosítót, cégazonosítót, személ azonosítót, kosár nevét, aktív státuszát    
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="companyId"></param>
        /// <param name="personId"></param>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public ShoppingCart(string visitorId, string companyId, string personId, string name, bool active)
        {
            this.Items = new List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem>();

            this.VisitorId = visitorId;

            this.CompanyId = companyId;

            this.PersonId = personId;

            this.Name = name;

            this.PaymentTerms = global::PaymentTerms.None;

            this.DeliveryTerms = global::DeliveryTerms.None;

            this.Shipping = new CompanyGroup.Domain.WebshopModule.Shipping() 
                                { 
                                    AddrRecId = 0, 
                                    City = String.Empty, 
                                    Country = String.Empty, 
                                    DateRequested = DateTime.MinValue, 
                                    InvoiceAttached = false, 
                                    Street = String.Empty, 
                                    ZipCode = String.Empty 
                                };

            this.Active = active;

            this.Status = CartStatus.Created;

            this.FinanceOffer = new CompanyGroup.Domain.WebshopModule.FinanceOffer() 
                                    { 
                                        Address = String.Empty, 
                                        NumOfMonth = 0, 
                                        PersonName = String.Empty, 
                                        Phone = String.Empty, 
                                        StatNumber = String.Empty 
                                    };
        }

        /// <summary>
        /// elemek a kosárban
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Items", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public List<ShoppingCartItem> Items { get; set; }

        /// <summary>
        /// látogató azonosító, egyedi, bejelentkezéshez kötött
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("VisitorId", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string VisitorId { get; set; }

        /// <summary>
        /// vevő vállalat azonosítója
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CompanyId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CompanyId { get; set; }

        /// <summary>
        /// vevő személy azonosítója
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PersonId", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string PersonId { get; set; }

        /// <summary>
        /// kosár neve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Name", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// KP, ÁTUT, Előre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PaymentTerms", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public PaymentTerms PaymentTerms { get; set; }

        /// <summary>
        /// szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("DeliveryTerms", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DeliveryTerms DeliveryTerms { get; set; }

        /// <summary>
        /// kiszállítási információk (időpont, cím)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Shipping", Order = 8)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Shipping Shipping { get; set; }

        /// <summary>
        /// aktív-e a kosár, vagy nem (kollekción belül egy lehet aktív)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Active", Order = 9)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool Active { get; set; }

        /// <summary>
        /// kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3, WaitingForAutoPost = 4)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Status", Order = 10)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CartStatus Status { get; set; }

        /// <summary>
        /// finanszírozási ajánlat adatait összefogó osztály
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("FinanceOffer", Order = 11)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CompanyGroup.Domain.WebshopModule.FinanceOffer FinanceOffer { get; set; }

        ///// <summary>
        ///// kosár elem hozzáadása
        ///// </summary>
        ///// <param name="item"></param>
        //public void AddItem(ShoppingCartItem item)
        //{
        //    int index = this.Items.FindIndex(x => x.ProductId.Equals(item.ProductId));

        //    if (index == -1)
        //        this.Items.Add(item);
        //}

        ///// <summary>
        ///// kosár elem mennyiségének frissítése
        ///// </summary>
        ///// <param name="item"></param>
        //public void UpdateItem(ShoppingCartItem item)
        //{
        //    int index = this.Items.FindIndex(x => x.ProductId.Equals(item.ProductId));

        //    if (index == -1) { return; }

        //    this.Items[index].Quantity = item.Quantity;
        //}

        ///// <summary>
        ///// kosár elem eltávolítása
        ///// </summary>
        ///// <param name="item"></param>
        //public void RemoveItem(ShoppingCartItem item)
        //{
        //    int index = this.Items.FindIndex(x => x.ProductId.Equals(item.ProductId));

        //    if (index != -1)
        //        this.Items[index].Status = CartItemStatus.Deleted;
        //}

        ///// <summary>
        ///// összes kosár elem eltávolítása (státusz törölt-re állítása)
        ///// </summary>
        //public void RemoveAll()
        //{
        //    this.Items.ForEach(item => item.Status = CartItemStatus.Deleted);
        //}

        ///// <summary>
        ///// kosár feladása (státusz feladott-ra állítása)
        ///// </summary>
        //public void PostAll()
        //{
        //    this.Items.ForEach(item => item.Status = CartItemStatus.Posted);
        //}

        /// <summary>
        /// kosárban lévő elemek összesen értéke 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public double SumTotal
        {
            get 
            { 
                double total = 0;

                this.Items.ForEach(item => total += item.Status.Equals(CartItemStatus.Created) || item.Status.Equals(CartItemStatus.Stored) ? item.ItemTotal : 0);

                return total;
            }
        }

        /// <summary>
        /// aktív elemek száma a kosárban
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public int ActiveItemCount
        {
            get { return this.Items.Where(x => x.Status.Equals(CartItemStatus.Created) || x.Status.Equals(CartItemStatus.Stored)).Count(); }
        }

        /// <summary>
        /// üres-e a kosár?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsEmpty
        {
            get { return this.Items.Where(x => x.Status.Equals(CartItemStatus.Created) || x.Status.Equals(CartItemStatus.Stored)).Count() == 0; }
        }

        /// <summary>
        /// üres-e a kosár?
        /// </summary>
        public int ItemCountByDataAreaId(DataAreaId dataAreaId)
        {
            return this.Items.Where(x => (x.Status.Equals(CartItemStatus.Created) || x.Status.Equals(CartItemStatus.Stored)) && (x.DataAreaId.Equals(dataAreaId))).Count();
        }

        /// <summary>
        /// megvizsgála, hogy benne van-e a termékazonosító a kosárban lévő termékek között
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool IsInCart(string productId)
        {
            if (String.IsNullOrEmpty(productId))
            {
                return false;
            }

            return this.Items.Exists(x => x.ProductId.Equals(productId));
        }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (this.IsTransient())
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ObjectId" }));
            }

            return validationResults;
        }
    }
}
