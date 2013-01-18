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
        /// bejelentkezés
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor SignIn(string userName, string password, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(userName), "User name may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(password), "Password may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.SignIn")
                                            .SetString("UserName", userName)
                                            .SetString("Password", password)
                                            .SetString("DataAreaId", dataAreaId);    //.SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.LoginInfo).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.Visitor visitor = query.UniqueResult<CompanyGroup.Domain.PartnerModule.Visitor>();

            return visitor;
        }

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
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorInsert").SetString("CustomerId", visitor.CustomerId);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// bejelentkezett státusz állapot törlése, kijelentkezés 
        /// </summary>
        /// <param name="visitorId"></param>
        public void DisableStatus(string visitorId)
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
        public void ChangeLanguage(string visitorId, string language)
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
        public void ChangeCurrency(string visitorId, string currency)
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
