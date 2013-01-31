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
        /// <param name="sessionWebInterface"></param>
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
        public int Add(CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorInsert").SetString("LoginIP", visitor.LoginIP)
                        .SetInt64("RecId", visitor.RecId)
                        .SetString("CustomerId", visitor.CustomerId)
                        .SetString("CustomerName", visitor.CustomerName)
                        .SetString("PersonId", visitor.PersonId)
                        .SetString("PersonName", visitor.PersonName)
                        .SetString("Email", visitor.Email)
                        .SetBoolean("IsWebAdministrator", visitor.Permission.IsWebAdministrator)
                        .SetBoolean("InvoiceInfoEnabled", visitor.Permission.InvoiceInfoEnabled)
                        .SetBoolean("PriceListDownloadEnabled", visitor.Permission.PriceListDownloadEnabled)
                        .SetBoolean("CanOrder", visitor.Permission.CanOrder)
                        .SetBoolean("RecieveGoods", visitor.Permission.RecieveGoods)
                        .SetString("Currency", visitor.Currency)
                        .SetString("LanguageId", visitor.LanguageId)
                        .SetString("DefaultPriceGroupId", visitor.DefaultPriceGroupId)
                        .SetString("InventLocationId", visitor.InventLocationId)
                        .SetString("RepresentativeId", visitor.Representative.Id)
                        .SetString("DataAreaId", visitor.DataAreaId)
                        .SetEnum("LoginType", visitor.LoginType)
                        .SetEnum("PartnerModel", visitor.PartnerModel)
                        .SetBoolean("AutoLogin", visitor.AutoLogin)
                        .SetDateTime("LoginDate", visitor.LoginDate)
                        .SetDateTime("ExpireDate", visitor.ExpiredDate);

                int result = query.UniqueResult<int>();

                return result;
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

                query.UniqueResult();
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

                query.UniqueResult();
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

                query.UniqueResult();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
