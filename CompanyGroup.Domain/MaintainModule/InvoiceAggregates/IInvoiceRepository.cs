using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// AX számlaműveleteket végző interfész
    /// </summary>
    public interface IInvoiceRepository
    {
        /// <summary>
        /// részletes számla sorok listája AX-ből
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> GetInvoiceDetailedLineInfo(string dataAreaId);
    }
}
