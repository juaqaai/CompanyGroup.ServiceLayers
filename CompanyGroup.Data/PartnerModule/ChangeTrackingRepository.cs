using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// változásokat lekövető repository
    /// </summary>
    public class ChangeTrackingRepository : CompanyGroup.Domain.PartnerModule.IChangeTrackingRepository
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
        /// InternetUser.SalesLineCT( @LastVersion BIGINT = 0 )
        /// </summary>
        /// <param name="lastVersion"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT> SalesLineCT(int lastVersion)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.SalesLineCT").SetInt32("LastVersion", lastVersion).SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT>() as List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT>;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
