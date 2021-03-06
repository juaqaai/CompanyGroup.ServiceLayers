﻿using System;
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
        /// összes tartozás, lejárt tartozás, pénznem lista
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        List<CompanyGroup.Dto.PartnerModule.InvoiceSumAmount> InvoiceSumValues(string visitorId);
    }
}
