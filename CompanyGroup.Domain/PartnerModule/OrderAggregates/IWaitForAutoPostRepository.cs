using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IWaitForAutoPostRepository
    {
        /// <summary>
        /// várakozó sor lekérdezése
        /// </summary>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.WaitingForAutoPost> WaitingForAutoPostSelect();

        /// <summary>
        /// új elem hozzáadása a várakozó sorhoz
        /// </summary>
        /// <param name="foreignKey">vagy a ShoppingCart.Id, vagy a Registration.Id</param>
        /// <param name="foreignKeyType">1: kosár, 2: regisztráció</param>
        /// <param name="content"></param>
        /// <returns></returns>
        int WaitingForAutoPostSalesOrderInsert(int foreignKey, int foreignKeyType, CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate);

        /// <summary>
        /// új elem hozzáadása a várakozó sorhoz
        /// </summary>
        /// <param name="foreignKey"></param>
        /// <param name="foreignKeyType">vagy a ShoppingCart.Id, vagy a Registration.Id</param>
        /// <param name="secondHandOrderCreate">1: kosár, 2: regisztráció</param>
        /// <returns></returns>
        int WaitingForAutoPostSecondhandOrderInsert(int foreignKey, int foreignKeyType, CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate secondHandOrderCreate);

        /// <summary>
        /// beállítja a WaitingForAutoPost rekord státuszát
        /// 0: törölt, 1: aktív (autopost-ra vár), 2: beküldött
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int WaitingForAutoPostSetStatus(int id, int status);
    }
}
