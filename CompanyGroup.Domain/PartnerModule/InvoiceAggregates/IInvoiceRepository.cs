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
        /// számlalista elemszám kiolvasása
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="debit"></param>
        /// <param name="overdue"></param>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="salesId"></param>
        /// <param name="serialNumber"></param>
        /// <param name="invoiceId"></param>
        /// <param name="dateIntervall"></param>
        /// <returns></returns>
        int GetListCount(string customerId, bool debit, bool overdue, string itemId, string itemName,
                         string salesId, string serialNumber, string invoiceId, int dateIntervall);

        /// <summary>
        /// számlalista kiolvasása paraméterek alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="debit"></param>
        /// <param name="overdue"></param>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="invoiceId"></param>
        /// <param name="serialNumber"></param>
        /// <param name="salesId"></param>
        /// <param name="dateIntervall"></param>
        /// <param name="sequence"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> GetList(string customerId, bool debit, bool overdue, string itemId, string itemName,
                                                                                string invoiceId, string serialNumber, string salesId, int dateIntervall,
                                                                                int sequence, int currentPageIndex, int itemsOnPage);

        /// <summary>
        /// Összes tartozás, összes lejárt tartozás
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.InvoiceSumAmount> InvoiceSumValues(string customerId);
    }
}
