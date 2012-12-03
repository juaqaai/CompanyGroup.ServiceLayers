using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IInvoiceRepository
    {
        /// <summary>
        /// számla kiolvasása azonosító alapján
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        InvoiceInfo GetById(string invoiceId);

        /// <summary>
        /// számlalista kiolvasása vevőazonosító alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<InvoiceInfo> GetList(string customerId, string dataAreaId);

        /// <summary>
        /// számlalista kiolvasása
        /// </summary>
        /// <returns></returns>
        List<InvoiceInfo> GetAll();

        /// <summary>
        /// számlalista hozzáadása  
        /// </summary>
        /// <param name="invoiceInfoList"></param>
        void AddList(List<InvoiceInfo> invoiceInfoList);
    }
}
