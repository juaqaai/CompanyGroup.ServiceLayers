using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő - árcsoport besorolás
    /// </summary>
    public class CustomerPriceGroup : CompanyGroup.Domain.Core.ValueObject<CustomerPriceGroup>
    {
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
        /// árcsoport azonosító
        /// </summary>
        public string PriceGroupId { get; set; }

        /// <summary>
        /// sorrendiség beállítás
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// sorrendet meghatározó jellemző
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="priceGroupId"></param>
        /// <param name="order"></param>
        public CustomerPriceGroup(string manufacturerId, string category1Id, string category2Id, string category3Id, string priceGroupId, int order)
        {
            this.ManufacturerId = manufacturerId;

            this.Category1Id = category1Id;

            this.Category2Id = category2Id;

            this.Category3Id = category3Id;

            this.PriceGroupId = priceGroupId;

            this.Order = order;
        }

    }
}
