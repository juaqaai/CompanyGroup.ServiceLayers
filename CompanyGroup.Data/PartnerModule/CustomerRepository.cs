using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// vevő repository
    /// </summary>
    public class CustomerRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.PartnerModule.ICustomerRepository
    {
        /// <summary>
        /// vevőhöz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public CustomerRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// InternetUser.cms_CustomerData( @CustomerId nvarchar(20), @DataAreaId nvarchar(3) = 'hrp' )	
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Customer GetCustomer(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_CustomerData")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)    //.UniqueResult<CompanyGroup.Domain.PartnerModule.MailAddress>();
                                            .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.Customer).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.Customer customer = query.UniqueResult<CompanyGroup.Domain.PartnerModule.Customer>();

            return customer; 
        }

        /// <summary>
        /// vevő levelezési cím lista kiolvasása vevőazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.MailAddress GetMailAddress(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = Session.GetNamedQuery("InternetUser.cms_MailAddress")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId).UniqueResult<CompanyGroup.Domain.PartnerModule.MailAddress>();
                                            //.SetResultTransformer(
                                            //new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.CustomerLetter).GetConstructors()[0]));

            return mailAddress;  //query.List<CompanyGroup.Domain.PartnerModule.CustomerLetter>() as List<CompanyGroup.Domain.PartnerModule.CustomerLetter>;
        }

        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.AddressZipCode> GetAddressZipCode(string prefix, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_AddressZipCode")
                                            .SetString("Prefix", prefix)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.AddressZipCode).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.AddressZipCode>() as List<CompanyGroup.Domain.PartnerModule.AddressZipCode>;
        }

        /// <summary>
        /// vevőhöz tartozó szállítási címek
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> GetDeliveryAddress(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_DeliveryAddress")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.DeliveryAddress).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.DeliveryAddress>() as List<CompanyGroup.Domain.PartnerModule.DeliveryAddress>;
        }

        /// <summary>
        /// vevőhöz tartozó bankszámlaszámok
        /// </summary>
        /// <example>InternetUser.cms_BankAccountList( @CustomerId NVARCHAR(20), @DataAreaId NVARCHAR(3) = 'hrp' )</example>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.BankAccount> GetBankAccounts(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_BankAccountList")
                                                            .SetString("CustomerId", customerId)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.BankAccount).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.BankAccount>() as List<CompanyGroup.Domain.PartnerModule.BankAccount>;
        }

        /// <summary>
        /// vevő kapcsolattartói
        /// InternetUser.cms_ContactPersons( @CustomerId nvarchar(20), @DataAreaId NVARCHAR(3) = 'hrp')
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.ContactPerson> GetContactPersons(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ContactPersons")
                                                            .SetString("CustomerId", customerId)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ContactPerson).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.ContactPerson>() as List<CompanyGroup.Domain.PartnerModule.ContactPerson>;
        }

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <remarks>
        /// [InternetUser].[SignIn] @UserName nvarchar(32) = '', @Password nvarchar(32) = '', @DataAreaId nvarchar(3) = ''
        /// </remarks>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        //public CompanyGroup.Domain.PartnerModule.LoginInfo SignIn(string userName, string password, string dataAreaId)
        //{
        //    CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(userName), "User name may not be null or empty");

        //    CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(password), "Password may not be null or empty");

        //    CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

        //    NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_SignIn")
        //                                    .SetString("UserName", userName)
        //                                    .SetString("Password", password)
        //                                    .SetString("DataAreaId", dataAreaId).SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.LoginInfo).GetConstructors()[0]));

        //    CompanyGroup.Domain.PartnerModule.LoginInfo loginInfo = query.UniqueResult<CompanyGroup.Domain.PartnerModule.LoginInfo>();

        //    return loginInfo;
        //}

        /// <summary>
        /// InternetUser.CustomerPrices( @DataAreaId VARCHAR(3) = '', @CustomerId NVARCHAR(10) = '' )
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> GetCustomerPriceGroups(string dataAreaId, string customerId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_CustomerPriceGroups")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId).SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup).GetConstructors()[0]));

            List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroups = query.List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>() as List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>;

            return customerPriceGroups;
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
        public void AddCustomerPriceGroup(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup customerPriceGroup)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerPriceGroupInsert")
                                                            .SetInt32("VisitorId", customerPriceGroup.VisitorId)
                                                            .SetString("ManufacturerId", customerPriceGroup.ManufacturerId)
                                                            .SetString("Category1Id", customerPriceGroup.Category1Id)
                                                            .SetString("Category2Id", customerPriceGroup.Category2Id)
                                                            .SetString("Category3Id", customerPriceGroup.Category3Id)
                                                            .SetInt32("Order", customerPriceGroup.Order);
            query.UniqueResult();
        }
    }
}
