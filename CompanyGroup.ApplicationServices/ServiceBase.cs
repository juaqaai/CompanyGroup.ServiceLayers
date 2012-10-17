using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices
{
    /// <summary>
    /// ősosztály a közös service-k összefogására
    /// </summary>
    public class ServiceBase : ServiceCoreBase
    {
        protected CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository;

        private const string CACHEKEY_EXCHANGERATE = "exchangerate";

        private static readonly string ExchangeRateCacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_EXCHANGERATE, String.Format("{0}_{1}_{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

        /// <summary>
        /// 120 percig cache-be kerül az exchange rate objektum
        /// </summary>
        private const double CACHE_EXPIRATION_EXCHANGERATE = 120d;

        /// <summary>
        /// konstruktor financeRepository -val
        /// </summary>
        /// <param name="financeRepository"></param>
        /// <param name="visitorRepository"></param>
        public ServiceBase(CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository, CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (financeRepository == null)
            {
                throw new ArgumentNullException("FinanceRepository");
            }

            this.financeRepository = financeRepository;
        }

        /// <summary>
        /// FROMDATE	            CURRENCYCODE	EXCHRATEMNB	        DATAAREAID
        /// 2012-07-26 00:00:00.000	EUR	            28753.000000000000	mst         1 EUR = 287.53 HUF
        /// 2012-07-26 00:00:00.000	GBP	            36715.000000000000	mst
        /// 2012-07-26 00:00:00.000	RSD	            242.000000000000	mst
        /// 2012-07-26 00:00:00.000	USD	            23716.000000000000	mst
        /// 2012-07-26 00:00:00.000	EUR	            11855.390000000000	ser
        /// </summary>
        /// <param name="prices">pénz értéke, amit váltani kell (FT-ban megadva)</param>
        /// <param name="currency">pénznem, amire váltani kell</param>
        /// <returns></returns>
        protected decimal ChangePrice(decimal price, string currency)
        {
            if (CompanyGroup.Domain.Core.Constants.CurrencyHuf.Equals(currency) || String.IsNullOrEmpty(currency))
            {
                return Math.Round(price, 2, MidpointRounding.AwayFromZero);
            }

            try
            {
                int year = DateTime.Now.Year;

                int month = DateTime.Now.Month;

                int day = DateTime.Now.Day;

                List<CompanyGroup.Domain.WebshopModule.ExchangeRate> exchangeRates = CompanyGroup.Helpers.CacheHelper.Get<List<CompanyGroup.Domain.WebshopModule.ExchangeRate>>(ServiceBase.ExchangeRateCacheKey);

                if (exchangeRates == null)
                {
                    exchangeRates = GetExchangeRatesFromRepository();
                }

                CompanyGroup.Domain.WebshopModule.ExchangeRate exchangeRate = exchangeRates.Find(x => x.CurrencyCode.Equals(currency));

                if (exchangeRate == null)
                {
                    return Math.Round(price, 2, MidpointRounding.AwayFromZero);
                }

                //Pl.: (ennyi Ft 1 EUR) * price
                decimal value = (exchangeRate.Rate / 100);  // 1 EUR = value (287.53 HUF) 

                return Math.Round((price / value), 2, MidpointRounding.AwayFromZero);                     // x EUR = price
            }
            catch { return Math.Round(price, 2, MidpointRounding.AwayFromZero); }
        }

        /// <summary>
        /// átváltási ráta kiolvasása adatbázisból
        /// </summary>
        /// <returns></returns>
        private List<CompanyGroup.Domain.WebshopModule.ExchangeRate> GetExchangeRatesFromRepository()
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.ExchangeRate> exchangeRates = financeRepository.GetCurrentRates();

                int year = DateTime.Now.Year;

                int month = DateTime.Now.Month;

                int day = DateTime.Now.Day;

                //csak akkor rakjuk cache-be, ha friss az adat
                CompanyGroup.Domain.WebshopModule.ExchangeRate exchangeRate = exchangeRates.Find(x => x.FromDate.Year.Equals(year) && x.FromDate.Month.Equals(month) && x.FromDate.Day.Equals(day));

                if (exchangeRate != null)
                {
                    CompanyGroup.Helpers.CacheHelper.Add<List<CompanyGroup.Domain.WebshopModule.ExchangeRate>>(ServiceBase.ExchangeRateCacheKey, exchangeRates, DateTime.Now.AddMinutes(CACHE_EXPIRATION_EXCHANGERATE));
                }

                return exchangeRates;
            }
            catch { return new List<CompanyGroup.Domain.WebshopModule.ExchangeRate>(); }
        }

    }
}
