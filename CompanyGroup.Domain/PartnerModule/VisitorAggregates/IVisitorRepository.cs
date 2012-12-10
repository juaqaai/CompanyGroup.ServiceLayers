using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IVisitorRepository
    {
        void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor);

        CompanyGroup.Domain.PartnerModule.Visitor GetItemByKey(string id);

        void DisableStatus(string id, string dataAreaId);

        void Disconnect();

        CompanyGroup.Domain.PartnerModule.Visitor ChangeLanguage(string id, string language);

        CompanyGroup.Domain.PartnerModule.Visitor ChangeCurrency(string id, string currency);
    }
}
