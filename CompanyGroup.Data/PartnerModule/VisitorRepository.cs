using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// látogató repository
    /// </summary>
    public class VisitorRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.PartnerModule.IVisitorRepository
    {
        /// <summary>
        /// látogatóhoz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public VisitorRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// látogató kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor GetItemById(string visitorId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(visitorId), "The visitorId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorSelect").SetString("VisitorId", visitorId);

                CompanyGroup.Domain.PartnerModule.Visitor visitor = query.UniqueResult<CompanyGroup.Domain.PartnerModule.Visitor>();

                return visitor;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// új látogató hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        public void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorInsert").SetString("VisitorId", visitor.visitorId);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// bejelentkezett státusz állapot törlése 
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="dataAreaId"></param>
        public void DisableStatus(string visitorId, string dataAreaId)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorSetStatus").SetString("VisitorId", visitorId).SetEnum("Status", LoginStatus.Passive);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// nyelv csere
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor ChangeLanguage(string visitorId, string language)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorSetLanguage").SetString("VisitorId", visitorId).SetString("LanguageId", language);

            }
            catch (Exception ex)
            {
                throw (ex);
            }        
        }

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor ChangeCurrency(string visitorId, string currency)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorSetCurrency").SetString("VisitorId", visitorId).SetString("Currency", currency);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
