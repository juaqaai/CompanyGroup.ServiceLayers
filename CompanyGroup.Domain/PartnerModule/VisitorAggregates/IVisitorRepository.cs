using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IVisitorRepository
    {
        CompanyGroup.Domain.PartnerModule.Visitor SignIn(string userName, string password, string dataAreaId);

        CompanyGroup.Domain.PartnerModule.Visitor GetItemById(string visitorId);

        void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor);

        void DisableStatus(string visitorId);

        void ChangeLanguage(string visitorId, string language);

        void ChangeCurrency(string visitorId, string currency);
    }
}
