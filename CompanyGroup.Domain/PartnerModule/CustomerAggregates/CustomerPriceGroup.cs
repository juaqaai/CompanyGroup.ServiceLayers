using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő - árcsoport besorolás
    /// </summary>
    public class CustomerPriceGroup : CompanyGroup.Domain.Core.EntityBase   //CompanyGroup.Domain.Core.ValueObject<CustomerPriceGroup>
    {
        /// <summary>
        /// kulcs
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public int VisitorKey { get; set; }

        /// <summary>
        /// árcsoport azonosító
        /// </summary>
        public string PriceGroupId { get; set; }

        /// <summary>
        /// gyártó, jelleg1, jelleg2, jelleg3 kiválasztása
        /// </summary>
        public string ManufacturerId { get; set; }

        /// <summary>
        /// gyártó, jelleg1, jelleg2, jelleg3 kiválasztása
        /// </summary>
        public string Category1Id { get; set; }

        /// <summary>
        /// gyártó, jelleg1, jelleg2, jelleg3 kiválasztása
        /// </summary>
        public string Category2Id { get; set; }

        /// <summary>
        /// gyártó, jelleg1, jelleg2, jelleg3 kiválasztása
        /// </summary>
        public string Category3Id { get; set; }

        /// <summary>
        /// sorrendiség beállítás
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// vállalatkód
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// sorrendet meghatározó jellemző
        /// LineId	VisitorKey	PriceGroupId	ManufacturerId	Category1Id	Category2Id	Category3Id	Order	DataAreaId
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="visitorKey"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="priceGroupId"></param>
        /// <param name="order"></param>
        /// <param name="dataAreaId"></param>
        public CustomerPriceGroup(int lineId, int visitorKey, string priceGroupId, string manufacturerId, string category1Id, string category2Id, string category3Id, int order, string dataAreaId)
        {
            this.LineId = lineId;

            this.VisitorKey = visitorKey;

            this.PriceGroupId = priceGroupId;

            this.ManufacturerId = manufacturerId;

            this.Category1Id = category1Id;

            this.Category2Id = category2Id;

            this.Category3Id = category3Id;

            this.Order = order;

            this.DataAreaId = dataAreaId;
        }

        /// <summary>
        /// sorrendet meghatározó jellemző
        /// </summary>
        public CustomerPriceGroup() : this(0, 0, "", "", "", "", "", 0, "") { }

        #region "EntityBase"overrides"

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
            if (obj == null || !(obj is CustomerPriceGroup))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            CustomerPriceGroup item = (CustomerPriceGroup)obj;

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

        #endregion
    }
}
