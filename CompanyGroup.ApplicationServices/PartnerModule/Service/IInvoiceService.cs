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
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        //CompanyGroup.Dto.PartnerModule.InvoiceInfo GetById(string invoiceId);

        /// <summary>
        /// vevőhöz tartozó számla lista kiolvasása
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetList(CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request);

        /// <summary>
        /// összes számla kiolvasása
        /// </summary>
        /// <returns></returns>
        //List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetAll();

    }
}
