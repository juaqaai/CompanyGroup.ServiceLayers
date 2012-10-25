using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.WebshopModule
{
    public class FinanceRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.WebshopModule.IFinanceRepository
    {
        public FinanceRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// átváltási rátát visszaadó lekérdezés
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ExchangeRate> GetCurrentRates()
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ExchangeRate")
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.ExchangeRate).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.ExchangeRate>() as List<CompanyGroup.Domain.WebshopModule.ExchangeRate>;        
        }

        /// <summary>
        /// tartós bérlet legkissebb és legnagyobb értékét tartalmazó lekérdezés 
        /// </summary>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue GetMinMaxLeasingValues()
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_MinMaxFinanceLeasingValues")
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue).GetConstructors()[0]));

            return query.UniqueResult<CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue>();
        }

        /// <summary>
        /// kalkuláció, tartósbérlet számítás finanszírozandó összeg alapján
        /// </summary>
        /// <remarks>
        /// bejövő paraméterek validálása, 
        /// lízing információk lekérdezése a felhasználó által megadott finanszírozandó összeg alapján 
        /// számítás: 30000000 Ft input esetén GetLeasingByFinancedAmount eredménye:
        /// FinanceParameterId	LeasingIntervalId	PaymentPeriodId	InterestRate	PresentValue	NumOfMonth	PercentValue
        /// 17	                5	                1	            2.940000000000	1.056000000000	24	        4.928800000000
        /// 18	                5	                2	            2.600000000000	1.062000000000	36	        3.450400000000
        /// 19	                5	                3	            2.600000000000	1.068000000000	48	        2.724500000000
        /// 20	                5	                4	            2.460000000000	1.074000000000	60	        2.285200000000
        /// Az eredményhalmazból a FinanceParameterId és a NumOfMonth értéke változatlanul jut a kimenetre, 
        /// a CalculatedValue output mező értéke: finanszírozandó összeg * (PercentValue / 100)
        /// </remarks>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.LeasingOption> GetLeasingByFinancedAmount(int amount)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_LeasingByFinancedAmount")
                                             .SetInt32("FinancedAmount", amount)
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.LeasingOption).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.LeasingOption>() as List<CompanyGroup.Domain.WebshopModule.LeasingOption>;
        }
    }
}
