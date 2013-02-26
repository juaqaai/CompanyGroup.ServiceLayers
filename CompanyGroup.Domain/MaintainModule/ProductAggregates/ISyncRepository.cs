using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public interface ISyncRepository
    {
        /// <summary>
        /// készletváltozás lekérdezés ERP adatbázisból
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        int GetStockChange(string dataAreaId, string inventLocationId, string productId);
    }
}
