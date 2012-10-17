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
    }
}
