using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő - árcsoport besorolás
    /// </summary>
    public class CustomerPriceGroup : CompanyGroup.Domain.Core.Entity   //CompanyGroup.Domain.Core.ValueObject<CustomerPriceGroup>
    {
        /// <summary>
        /// kulcs
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public int VisitorId { get; set; }

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
        /// sorrendet meghatározó jellemző
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="priceGroupId"></param>
        /// <param name="order"></param>
        public CustomerPriceGroup(int id, int visitorId, string priceGroupId, string manufacturerId, string category1Id, string category2Id, string category3Id, int order)
        {
            this.Id = id;

            this.VisitorId = visitorId;

            this.PriceGroupId = priceGroupId;

            this.ManufacturerId = manufacturerId;

            this.Category1Id = category1Id;

            this.Category2Id = category2Id;

            this.Category3Id = category3Id;

            this.Order = order;
        }

        /// <summary>
        /// sorrendet meghatározó jellemző
        /// </summary>
        public CustomerPriceGroup() : this(0, 0, "", "", "", "", "", 0) { }

    }
}
