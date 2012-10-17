using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// látogató profile, 
    ///     látogatott termékoldalak előzményeivel
    ///     vevői árbesorolás listával, 
    /// </summary>
    public class Profile : CompanyGroup.Domain.Core.ValueObject<Profile>
    {
        private HashSet<History> histories;

        private HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroup;

        /// <summary>
        /// konstruktor látogatott termékoldalak előzményeivel, vevői árbesorolás listával
        /// </summary>
        /// <param name="histories"></param>
        /// <param name="customerPriceGroup"></param>
        public Profile(HashSet<History> histories, HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroup)
        {
            this.histories = histories;

            this.customerPriceGroup = customerPriceGroup;
        }

        /// <summary>
        /// konstruktor vevői árbesorolás listával
        /// </summary>
        /// <param name="customerPriceGroup"></param>
        public Profile(HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroup) : this(new HashSet<History>(), customerPriceGroup)
        {
        }

        public Profile() : this(new HashSet<History>(), new HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>())
        {
        }

        /// <summary>
        /// előzmények lista kiolvasása, beállítása
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Histories", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]         
        public HashSet<History> Histories
        {
            get
            {
                if (histories == null)
                {
                    histories = new HashSet<History>();
                }
                return histories;
            }
            set
            {
                histories = new HashSet<History>(value);
            }
        }

        /// <summary>
        /// history elem hozzáadás
        /// </summary>
        /// <param name="customerPriceGroup"></param>
        public void AddHistory(CompanyGroup.Domain.PartnerModule.History history)
        {
            if (history == null)
            {
                throw new ArgumentNullException("History");
            }

            this.Histories.Add(history);
        }

        /// <summary>
        /// vevő árcsoportok kiolvasása, beállítása
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CustomerPriceGroups", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired] 
        public virtual ICollection<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> CustomerPriceGroups
        {
            get
            {
                if (customerPriceGroup == null)
                {
                    customerPriceGroup = new HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>();
                }
                return customerPriceGroup;
            }
            set
            {
                customerPriceGroup = new HashSet<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>(value);
            }
        }

        /// <summary>
        /// vevő árcsoport elem hozzáadás
        /// </summary>
        /// <param name="customerPriceGroup"></param>
        public void AddCustomerPriceGroup(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup customerPriceGroup)
        {
            if (customerPriceGroup == null)
            {
                throw new ArgumentNullException("CustomerPriceGroup");
            }

            this.CustomerPriceGroups.Add(customerPriceGroup);
        }
    }
}
