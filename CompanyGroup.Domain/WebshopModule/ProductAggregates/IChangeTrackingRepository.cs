using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IChangeTrackingRepository
    {
        /// <summary>
        /// készletváltozás lekérdezés ERP adatbázisból
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.InventSumList InventSumCT(int lastVersion);

        /// <summary>
        /// árváltozás lekérdezése ERP adatbázisból
        /// </summary>
        /// <param name="lastVersion"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.PriceDiscTableList PriceDiscTableCT(int lastVersion);
    }
}
