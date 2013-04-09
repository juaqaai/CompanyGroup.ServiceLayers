using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.WebshopModule
{
    /// <summary>
    /// sync repository
    /// </summary>
    public class ChangeTrackingRepository : CompanyGroup.Domain.WebshopModule.IChangeTrackingRepository
    {
        public ChangeTrackingRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetLiveSystemSession(); }
        }

        /// <summary>
        /// [InternetUser].[InventSumCT] (@LastVersion int)
        /// </summary>
        /// <param name="lastVersion"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.InventSumList InventSumCT(int lastVersion)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InventSumCT").SetInt32("LastVersion", lastVersion).SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.InventSum).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.InventSum> inventSumList = query.List<CompanyGroup.Domain.WebshopModule.InventSum>() as List<CompanyGroup.Domain.WebshopModule.InventSum>;

                CompanyGroup.Domain.WebshopModule.InventSumList result = new CompanyGroup.Domain.WebshopModule.InventSumList(inventSumList);

                return result; 
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// InternetUser.PriceDiscTableCT (@LastVersion int)
        /// </summary>
        /// <param name="lastVersion"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.PriceDiscTableList PriceDiscTableCT(int lastVersion)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.PriceDiscTableCT").SetInt32("LastVersion", lastVersion).SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.PriceDiscTable).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.PriceDiscTable> priceDiscTableList = query.List<CompanyGroup.Domain.WebshopModule.PriceDiscTable>() as List<CompanyGroup.Domain.WebshopModule.PriceDiscTable>;

                CompanyGroup.Domain.WebshopModule.PriceDiscTableList result = new CompanyGroup.Domain.WebshopModule.PriceDiscTableList(priceDiscTableList);

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
