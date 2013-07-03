using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// látogató repository
    /// </summary>
    public class VisitorRepository : CompanyGroup.Domain.PartnerModule.IVisitorRepository
    {
        /// <summary>
        /// látogatóhoz kapcsolódó műveletek konstruktora
        /// </summary>
        public VisitorRepository() { }

        /// <summary>
        /// nhibernate web session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
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

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorSelect").SetString("VisitorId", visitorId)
                                                 .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.Visitor).GetConstructors()[0]));

                //CompanyGroup.Domain.PartnerModule.VisitorData visitor = query.UniqueResult<CompanyGroup.Domain.PartnerModule.VisitorData>();
                CompanyGroup.Domain.PartnerModule.Visitor visitor = query.UniqueResult<CompanyGroup.Domain.PartnerModule.Visitor>();

                return visitor;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> GetCustomerPriceGroups(int id)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerPriceGroups").SetInt32("Id", id).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup).GetConstructors()[0]));

                List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroups = query.List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>() as List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>;

                return customerPriceGroups;
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
                string representativeId = visitor.Representatives.Count > 0 ? visitor.Representatives[0].Id : String.Empty;

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorInsert")
                        .SetString("LoginIP", visitor.LoginIP)
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
                        .SetString("PaymTermIdBsc", visitor.PaymTermIdBsc)
                        .SetString("PaymTermIdHrp", visitor.PaymTermIdHrp)
                        .SetString("Currency", visitor.Currency)
                        .SetString("LanguageId", visitor.LanguageId)
                        .SetString("DefaultPriceGroupIdHrp", visitor.DefaultPriceGroupIdHrp)
                        .SetString("DefaultPriceGroupIdBsc", visitor.DefaultPriceGroupIdBsc)
                        .SetString("InventLocationIdHrp", visitor.InventLocationIdHrp)
                        .SetString("InventLocationIdBsc", visitor.InventLocationIdBsc)
                        .SetString("RepresentativeId", representativeId)
                        .SetEnum("LoginType", visitor.LoginType)
                        .SetBoolean("RightHrp", visitor.RightHrp)
                        .SetBoolean("RightBsc", visitor.RightBsc)
                        .SetBoolean("ContractHrp", visitor.ContractHrp)
                        .SetBoolean("ContractBsc", visitor.ContractBsc)
                        .SetInt32("CartId", visitor.CartId)
                        .SetString("RegistrationId", visitor.RegistrationId)
                        .SetBoolean("IsCatalogueOpened", visitor.IsCatalogueOpened)
                        .SetBoolean("IsShoppingCartOpened", visitor.IsShoppingCartOpened)
                        .SetBoolean("AutoLogin", visitor.AutoLogin).SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.VisitorInsertResult).GetConstructors()[0]));

                CompanyGroup.Domain.PartnerModule.VisitorInsertResult result = query.UniqueResult<CompanyGroup.Domain.PartnerModule.VisitorInsertResult>();

                visitor.Id = result.Id;

                visitor.VisitorId = result.VisitorId;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// vevő árcsoport hozzáadás
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="order"></param>
        /// <param name="customerPriceGroup"></param>
        public void AddCustomerPriceGroup(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup customerPriceGroup)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerPriceGroupInsert")
                                                            .SetInt32("VisitorKey", customerPriceGroup.VisitorKey)
                                                            .SetString("PriceGroupId", customerPriceGroup.PriceGroupId)
                                                            .SetString("ManufacturerId", customerPriceGroup.ManufacturerId)
                                                            .SetString("Category1Id", customerPriceGroup.Category1Id)
                                                            .SetString("Category2Id", customerPriceGroup.Category2Id)
                                                            .SetString("Category3Id", customerPriceGroup.Category3Id)
                                                            .SetInt32("Order", customerPriceGroup.Order)
                                                            .SetString("DataAreaId", customerPriceGroup.DataAreaId);
            query.UniqueResult();
        }

        /// <summary>
        /// speciális vevői ár hozzáadása
        /// </summary>
        /// <param name="customerSpecialPrice"></param>
        public void AddCustomerSpecialPrice(CompanyGroup.Domain.PartnerModule.CustomerSpecialPrice customerSpecialPrice)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerSpecialPriceInsert")
                                                            .SetInt32("VisitorKey", customerSpecialPrice.VisitorKey)
                                                            .SetString("ProductId", customerSpecialPrice.ProductId)
                                                            .SetInt32("Price", customerSpecialPrice.Price)
                                                            .SetString("DataAreaId", customerSpecialPrice.DataAreaId);
            query.UniqueResult();
        }

        /// <summary>
        /// bejelentkezett státusz állapot törlése, kijelentkezés 
        /// </summary>
        /// <param name="visitorId"></param>
        public void DisableStatus(string visitorId)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorDisableStatus").SetString("VisitorId", visitorId);

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
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorChangeLanguage").SetString("VisitorId", visitorId).SetString("Language", language);

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
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.VisitorChangeCurrency").SetString("VisitorId", visitorId).SetString("Currency", currency);

                query.UniqueResult();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
