using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    /// <summary>
    /// számlák szerviz interfész 
    /// </summary>
    public interface IInvoiceService
    {
        /// <summary>
        /// cache újratöltése (törlés, feltöltés)
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        bool ReFillCache(string dataAreaId);

        /// <summary>
        /// cache feltöltése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        bool FillCache(string dataAreaId);
    }
}
