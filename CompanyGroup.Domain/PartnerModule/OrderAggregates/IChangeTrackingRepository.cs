using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IChangeTrackingRepository
    {
        List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT> SalesLineCT(int lastVersion);
    }
}
