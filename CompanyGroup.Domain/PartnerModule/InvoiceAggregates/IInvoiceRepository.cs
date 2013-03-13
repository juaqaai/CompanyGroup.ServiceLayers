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
        //InvoiceInfo GetById(string invoiceId);

        /// <summary>
        /// számlalista kiolvasása vevőazonosító alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<InvoiceDetailedLineInfo> GetList(string customerId, bool debit, bool overdue, string itemId, string itemName,
                                              string salesId, string serialNumber, string invoiceId, int dateIntervall,
                                              int currentPageIndex, int itemsOnPage);

        /// <summary>
        /// összes számlalista kiolvasása
        /// </summary>
        /// <returns></returns>
        //List<InvoiceInfo> GetAll();
    }
}
