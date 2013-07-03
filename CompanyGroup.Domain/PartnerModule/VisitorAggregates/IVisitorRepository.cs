using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IVisitorRepository
    {
        CompanyGroup.Domain.PartnerModule.Visitor GetItemById(string visitorId);

        List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> GetCustomerPriceGroups(int id);

        void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor);

        void DisableStatus(string visitorId);

        void ChangeLanguage(string visitorId, string language);

        void ChangeCurrency(string visitorId, string currency);

        /// <summary>
        /// vevő árcsoport hozzáadás
        /// </summary>
        /// <param name="customerPriceGroup"></param>
        void AddCustomerPriceGroup(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup customerPriceGroup);

        /// <summary>
        /// speciális vevői ár hozzáadása
        /// </summary>
        /// <param name="customerSpecialPrice"></param>
        void AddCustomerSpecialPrice(CompanyGroup.Domain.PartnerModule.CustomerSpecialPrice customerSpecialPrice);
    }
}
