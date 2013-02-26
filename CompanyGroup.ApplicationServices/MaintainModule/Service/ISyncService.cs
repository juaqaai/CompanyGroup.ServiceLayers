using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    public interface ISyncService
    {
        /// <summary>
        /// aktuális készlet frissítése
        /// </summary>
        /// <returns></returns>
        void StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request);
    }
}
