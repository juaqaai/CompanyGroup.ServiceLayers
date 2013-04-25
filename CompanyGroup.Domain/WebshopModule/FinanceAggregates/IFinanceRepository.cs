using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IFinanceRepository
    {
        /// <summary>
        /// átváltási ráta lista    
        /// </summary>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.ExchangeRate> GetCurrentRates();

        /// <summary>
        /// tartós bérlet legkissebb és legnagyobb értékét tartalmazó lekérdezés 
        /// </summary>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue GetMinMaxLeasingValues();

        /// <summary>
        /// kalkuláció, tartósbérlet számítás finanszírozandó összeg alapján
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.LeasingOption> GetLeasingByFinancedAmount(int amount);

        /// <summary>
        /// ajánlat kiolvasása azonosító alapján 
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.FinanceOffer GetFinanceOffer(int offerId);

        /// <summary>
        /// ajánlat hozzáadása kollekcióhoz
        /// </summary>
        /// <param name="shoppingCart"></param>
        int Add(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer);

        /// <summary>
        /// ajánlat eltávolítása kollekcióból
        /// </summary>
        /// <param name="offerId"></param>
        void Remove(int offerId);

        /// <summary>
        /// ajánlat feladás,
        /// </summary>
        void Post(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer);

        /// <summary>
        /// ajánlat elem mennyiség módosítása
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="quantity"></param>
        void UpdateLineQuantity(int lineId, int quantity);

        /// <summary>
        /// ajánlat elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        int AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item);

        /// <summary>
        ///  ajánlat elem eltávolítás
        /// </summary>
        /// <param name="lineId"></param>
        void RemoveLine(int lineId);

        /// <summary>
        /// a megadott file elérési úttal elkészíti a pdf dokumentumot
        /// </summary>
        /// <param name="financedAmount"></param>
        /// <param name="calcValues"></param>
        /// <param name="pdfFileWithPath"></param>
        void CreateLeasingDocument(string financedAmount, System.Collections.Specialized.StringCollection calcValues, string pdfFileWithPath);
    }
}
