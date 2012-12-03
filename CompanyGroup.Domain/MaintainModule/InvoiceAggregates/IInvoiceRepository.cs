using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.MaintainModule
{
    public interface IInvoiceRepository
    {
        /// <summary>
        /// részletes számla sorok listája
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> GetInvoiceDetailedLineInfo(string customerId, string dataAreaId);
    }
}
