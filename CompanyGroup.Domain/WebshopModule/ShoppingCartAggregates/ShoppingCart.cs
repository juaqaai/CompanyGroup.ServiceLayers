using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// bevásárló kosár
    /// regisztrált felhasználónként egy lehet aktív, de kosár számosságára tekintve nincs megkötés.
    /// </summary>
    public class ShoppingCart : CompanyGroup.Domain.Core.EntityBase, IValidatableObject
    {

        /// <summary>
        ///  konstruktor, létrehozza az elemek listát üresen
        ///  beállítja a látogató azonosítót, cégazonosítót, személ azonosítót, kosár nevét, aktív státuszát    
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorId"></param>
        /// <param name="companyId"></param>
        /// <param name="personId"></param>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <param name="active"></param>
        public ShoppingCart(int id, string visitorId, string companyId, string personId, string name, string currency, bool active)
        {
            this.Id = id;

            this.Items = new List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem>();

            this.VisitorId = visitorId;

            this.CompanyId = companyId;

            this.PersonId = personId;

            this.Name = name;

            this.PaymentTerms = global::PaymentTerms.None;

            this.DeliveryTerms = global::DeliveryTerms.None;

            DateTime dateRequested;

            if (!DateTime.TryParse("1900.01.01", out dateRequested))
            {
                dateRequested = DateTime.Now.AddDays(-7d);
            }

            this.Shipping = new CompanyGroup.Domain.WebshopModule.Shipping() 
                                { 
                                    AddrRecId = 0, 
                                    City = String.Empty, 
                                    Country = String.Empty,
                                    DateRequested = dateRequested, 
                                    InvoiceAttached = false, 
                                    Street = String.Empty, 
                                    ZipCode = String.Empty 
                                };

            this.Currency = currency;

            this.Active = active;

            this.Status = CartStatus.Created;
        }

        public ShoppingCart() : this(0, "", "", "", "", "", false)
        { 
            
        }

        /// <summary>
        /// azonosító
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// elemek a kosárban
        /// </summary>
        public IList<ShoppingCartItem> Items { get; set; }

        /// <summary>
        /// látogató azonosító, egyedi, bejelentkezéshez kötött
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// vevő vállalat azonosítója
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// vevő személy azonosítója
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// kosár neve
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// KP, ÁTUT, Előre ut., Utánvét (Cash = 1, Transfer = 2, ForwardTransfer = 3, CashOnDelivery = 4)
        /// </summary>
        public PaymentTerms PaymentTerms { get; set; }

        /// <summary>
        /// szállítás, vagy raktárból (Delivery = 1, Warehouse = 2)
        /// </summary>
        public DeliveryTerms DeliveryTerms { get; set; }

        /// <summary>
        /// kiszállítási információk (időpont, cím)
        /// </summary>
        public Shipping Shipping { get; set; }

        /// <summary>
        /// aktív-e a kosár, vagy nem (kollekción belül egy lehet aktív)
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// rendelésre beállított valutanem
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3, WaitingForAutoPost = 4)
        /// </summary>
        public CartStatus Status { get; set; }

        /// <summary>
        /// létrehozás dátuma, ideje
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// kosárban lévő elemek összesen értéke 
        /// </summary>
        public int SumTotal
        {
            get 
            { 
                int total = 0;

                this.Items.ToList().ForEach(item => total += item.Status.Equals(CartItemStatus.Created) || item.Status.Equals(CartItemStatus.Stored) ? item.CustomerPrice : 0);

                return total;
            }
        }

        /// <summary>
        /// aktív elemek száma a kosárban
        /// </summary>
        public int ActiveItemCount
        {
            get { return this.Items.Where(x => x.Status.Equals(CartItemStatus.Created) || x.Status.Equals(CartItemStatus.Stored)).Count(); }
        }

        /// <summary>
        /// üres-e a kosár?
        /// </summary>
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

            return this.Items.ToList().Exists(x => x.ProductId.Equals(productId));
        }

        /// <summary>
        /// törli a tranziens elemeket
        /// </summary>
        public void RemoveTransientItems()
        {
            this.Items.ToList().RemoveAll(x => x.IsTransient());
        }

        #region "EntityBase override metódusok"

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return this.Id == 0;
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
                return item.CartId == this.Id;
            }
        }

        /// <summary>
        /// hash code előállítás
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.Id.GetHashCode() ^ 31;
        }

        #endregion

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
