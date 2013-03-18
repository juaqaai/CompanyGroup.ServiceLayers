using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// számlák szerviz partner modul
    /// </summary>
    public interface IInvoiceService
    {

        /// <summary>
        /// számla információ kiolvasása
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        CompanyGroup.Dto.PartnerModule.InvoiceInfo GetInvoiceInfo(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request);

        /// <summary>
        /// számla kiolvasása
        /// </summary>
        /// <returns></returns>
        CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed GetDetails(CompanyGroup.Dto.PartnerModule.GetDetailedInvoiceInfoRequest request);

    }
}
