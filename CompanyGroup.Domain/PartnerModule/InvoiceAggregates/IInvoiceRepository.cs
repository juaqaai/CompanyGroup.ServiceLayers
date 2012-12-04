using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// számlaműveleteket végző interfész
    /// </summary>
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
        /// összes számlalista kiolvasása
        /// </summary>
        /// <returns></returns>
        List<InvoiceInfo> GetAll();

        /// <summary>
        /// számlalista hozzáadása  
        /// </summary>
        /// <param name="invoiceInfoList"></param>
        void InsertList(List<InvoiceInfo> invoiceInfoList);

        /// <summary>
        /// összes számla eltávolítása a cache-ből
        /// </summary>
        void RemoveAllItemsFromCollection();

        /// <summary>
        /// kiüríti az elemeket a megadott kollekcióból
        /// </summary>
        /// <param name="dataAreaId"></param>
        void RemoveItemsFromCollection(string dataAreaId);

        /// <summary>
        /// kollekcióhoz tartozó indexek készítése  
        /// </summary>
        void CreateIndexes();
    }
}
