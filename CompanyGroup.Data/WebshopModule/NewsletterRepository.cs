using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.WebshopModule
{
    public class NewsletterRepository : CompanyGroup.Domain.WebshopModule.INewsletterRepository
    {
        public NewsletterRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

        /// <summary>
        /// hírlevél lista
        /// </summary>
        /// <remarks>
        /// [InternetUser].[cms_NewsletterList]( @TopN INT = 0, 
		///												@DataAreaId nvarchar(3) = 'hrp', --vallalat azonosito
		///												@BusinessUnitId nvarchar(16) = '', -- hozzarendelt uzletag
		///												@ManufacturerId nvarchar(16) = '' -- hozzarendelt gyarto
		///											  )
        /// </remarks>
        /// <param name="topN"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="businessUnitId"></param>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.Newsletter> GetNewsletterList(int topN, string dataAreaId, string businessUnitId, string manufacturerId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.NewsletterSelect")
                                             .SetInt32("TopN", topN)
                                             .SetString("DataAreaId", dataAreaId)
                                             .SetString("BusinessUnitId", businessUnitId)
                                             .SetString("ManufacturerId", manufacturerId)
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Newsletter).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.Newsletter>() as List<CompanyGroup.Domain.WebshopModule.Newsletter>;
        }

        /// <summary>
        /// [InternetUser].[NewsletterSelectCondition]( @TopN INT = 0, @DataAreaId nvarchar(3) = '', @NewsletterIdList nvarchar (250) = '' )
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="newsletterIdList"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.Newsletter> GetNewsletterListByFilter(int topN, string dataAreaId, string newsletterIdList)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.NewsletterSelectCondition")
                                             .SetInt32("TopN", topN)
                                             .SetString("DataAreaId", dataAreaId)
                                             .SetString("NewsletterIdList", newsletterIdList)
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Newsletter).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.Newsletter>() as List<CompanyGroup.Domain.WebshopModule.Newsletter>;
        }
    }
}
