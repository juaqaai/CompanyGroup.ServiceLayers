using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// finanszírozási ajánlat
    /// </summary>
    public class FinanceOffer : CompanyGroup.Domain.Core.EntityBase
    {
        /// <summary>
        /// egyedi azonosító
        /// </summary>
        public int Id { get; set; }

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
        /// bérbevevő név
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// bérbevevő cím
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// bérbevevő telefon
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// bérbevevő cégjegyzékszám
        /// </summary>
        public string StatNumber { get; set; }

        /// <summary>
        /// futamidő
        /// </summary>
        public int NumOfMonth { get; set; }

        /// <summary>
        /// valutanem
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
        /// </summary>
        public OfferStatus Status { get; set; }

        /// <summary>
        /// létrehozás dátuma, ideje
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// kosár elemei
        /// </summary>
        public IList<ShoppingCartItem> Items { get; set; }

        /// <summary>
        /// finanszírozandó összeg
        /// </summary>
        public int TotalAmount
        {
            get 
            {
                int result = 0;

                this.Items.ToList().ForEach(x => { result += x.CustomerPrice; });

                return result;
            }
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
    }
}
