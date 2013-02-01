using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IVisitorRepository
    {
        CompanyGroup.Domain.PartnerModule.Visitor GetItemById(string visitorId);

        void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor);

        void DisableStatus(string visitorId);

        void ChangeLanguage(string visitorId, string language);

        void ChangeCurrency(string visitorId, string currency);

        /// <summary>
        /// vevő árcsoport hozzáadás
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="order"></param>
        void AddCustomerPriceGroup(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup customerPriceGroup);
    }
}
