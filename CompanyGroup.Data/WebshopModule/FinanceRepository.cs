using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.WebshopModule
{
    public class FinanceRepository : CompanyGroup.Domain.WebshopModule.IFinanceRepository
    {
        public FinanceRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

        /// <summary>
        /// átváltási rátát visszaadó lekérdezés
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ExchangeRate> GetCurrentRates()
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ExchangeRate")
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
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.MinMaxFinanceLeasingValues")
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
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.LeasingByFinancedAmount")
                                             .SetInt32("FinancedAmount", amount)
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.LeasingOption).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.LeasingOption>() as List<CompanyGroup.Domain.WebshopModule.LeasingOption>;
        }

        /// <summary>
        /// ajánlat kiolvasása azonosító alapján 
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.FinanceOffer GetFinanceOffer(int offerId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((offerId > 0), "The offerId parameter must be greather than zero!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetFinanceOffer").SetInt32("OfferId", offerId);

                CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer = query.UniqueResult<CompanyGroup.Domain.WebshopModule.FinanceOffer>();

                return financeOffer;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár hozzáadása kollekcióhoz, új kosárazonosítóval tér vissza
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public int Add(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((financeOffer != null), "The financeOffer cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.FinanceOfferInsert").SetString("VisitorId", financeOffer.VisitorId)
                                                                                                  .SetString("LeasingPersonName", financeOffer.PersonName)
                                                                                                  .SetString("LeasingAddress", financeOffer.Address)
                                                                                                  .SetString("LeasingPhone", financeOffer.Phone)
                                                                                                  .SetString("LeasingStatNumber", financeOffer.StatNumber)
                                                                                                  .SetInt32("NumOfMonth", financeOffer.NumOfMonth)
                                                                                                  .SetString("Currency", financeOffer.Currency);
                int offerId = query.UniqueResult<int>();

                return offerId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void Remove(int offerId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((offerId > 0), "The id parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.FianceOfferSetStatus").SetInt32("OfferId", offerId)
                                                                                                     .SetEnum("Status", CartStatus.Deleted);

                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár feladása, WaitingForAutoPost státusz beállítás történik
        /// </summary>
        /// <param name="financeOffer"></param>
        public void Post(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((financeOffer != null), "The financeOffer cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartUpdate").SetInt32("OfferId", financeOffer.Id)
                                                                                                  .SetString("LeasingPersonName", financeOffer.PersonName)
                                                                                                  .SetString("LeasingAddress", financeOffer.Address)
                                                                                                  .SetString("LeasingPhone", financeOffer.Phone)
                                                                                                  .SetString("LeasingStatNumber", financeOffer.StatNumber)
                                                                                                  .SetInt32("NumOfMonth", financeOffer.NumOfMonth)
                                                                                                  .SetString("Currency", financeOffer.Currency);
                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem mennyiség frissítése
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(int lineId, int quantity)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((quantity > 0), "The quantity parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.FinanceOfferLineUpdate").SetInt32("LineId", lineId)
                                                                                                      .SetInt32("Quantity", quantity);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        public int AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((item != null), "The item cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.FinanceOfferLineInsert").SetInt32("OfferId", item.CartId)
                                                                                                      .SetString("ProductId", item.ProductId)
                                                                                                      .SetInt32("Quantity", item.Quantity)
                                                                                                      .SetInt32("Price", item.CustomerPrice)
                                                                                                      .SetString("DataAreaId", item.DataAreaId)
                                                                                                      .SetEnum("Status", item.Status);
                int lineId = query.UniqueResult<int>();

                return lineId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="lineId"></param>
        public void RemoveLine(int lineId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.FinanceOfferSetLineStatus").SetInt32("LineId", lineId)
                                                                                                         .SetEnum("Status", CartItemStatus.Deleted);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
