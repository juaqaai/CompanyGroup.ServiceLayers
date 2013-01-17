using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IVisitorRepository
    {
        CompanyGroup.Domain.PartnerModule.Visitor SignIn(string userName, string password, string dataAreaId);

        CompanyGroup.Domain.PartnerModule.Visitor GetItemById(string visitorId);

        void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor);

        void DisableStatus(string id, string dataAreaId);

        void ChangeLanguage(string id, string language);

        void ChangeCurrency(string id, string currency);
    }
}
