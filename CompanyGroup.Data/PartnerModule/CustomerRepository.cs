using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// vevő repository
    /// </summary>
    public class CustomerRepository : RepositoryBase, CompanyGroup.Domain.PartnerModule.ICustomerRepository
    {
        private static readonly string ClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("RegistrationServiceClassName", "CustomerService");

        /// <summary>
        /// vevőhöz kapcsolódó műveletek konstruktor
        /// </summary>
        /// <param name="session"></param>
        public CustomerRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

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

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerSelect")
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

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.MailAddressSelect")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.MailAddress).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = query.UniqueResult<CompanyGroup.Domain.PartnerModule.MailAddress>();

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

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.AddressZipCodeSelect")
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

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.DeliveryAddressSelect")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.DeliveryAddress).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.DeliveryAddress>() as List<CompanyGroup.Domain.PartnerModule.DeliveryAddress>;
        }

        /// <summary>
        /// vevőhöz tartozó bankszámlaszámok
        /// </summary>
        /// <example>InternetUser.BankAccountSelect( @CustomerId NVARCHAR(20), @DataAreaId NVARCHAR(3) = 'hrp' )</example>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.BankAccount> GetBankAccounts(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.BankAccountSelect")
                                                            .SetString("CustomerId", customerId)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.BankAccount).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.BankAccount>() as List<CompanyGroup.Domain.PartnerModule.BankAccount>;
        }

        /// <summary>
        /// vevő kapcsolattartói
        /// InternetUser.ContactPersonSelect( @CustomerId nvarchar(20), @ContactPersonId nvarchar(20), @DataAreaId NVARCHAR(3) = 'hrp')
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.ContactPerson> GetContactPersons(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ContactPersonSelect")
                                                            .SetString("CustomerId", customerId)
                                                            .SetString("ContactPersonId", String.Empty)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ContactPerson).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.ContactPerson>() as List<CompanyGroup.Domain.PartnerModule.ContactPerson>;
        }

        /// <summary>
        /// vevő árbesorolás lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> GetCustomerPriceGroups(string customerId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "CustomerId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CustomerPriceGroupSelect")
                                            .SetString("CustomerId", customerId);   //.SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.CustomerPriceGroup).GetConstructors()[0]));

            List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroups = query.List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>() as List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup>;

            return customerPriceGroups;
        }

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
        /// regisztráció - új vevő létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.CustomerCreateResult Create(CompanyGroup.Domain.RegistrationModule.CustomerCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.RegistrationModule.CustomerCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         CustomerRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createCustomer", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.RegistrationModule.CustomerCreateResult response = this.DeSerialize<CompanyGroup.Domain.RegistrationModule.CustomerCreateResult>(xml);

            return response;            
        }

        /// <summary>
        /// regisztráció - szállítási cím létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long CreateDeliveryAddress(CompanyGroup.Domain.RegistrationModule.DeliveryAddressCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.RegistrationModule.DeliveryAddressCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         CustomerRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createDeliveryAddress", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            long deliveryAddrRecId = 0;

            long.TryParse(xml, out deliveryAddrRecId);

            return deliveryAddrRecId;             
        }

        /// <summary>
        /// regisztráció - kapcsolattartó létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long CreateContactPerson(CompanyGroup.Domain.RegistrationModule.ContactPersonCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.RegistrationModule.ContactPersonCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         CustomerRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createContactPerson", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            long contactPersonRecId = 0;

            long.TryParse(xml, out contactPersonRecId);

            return contactPersonRecId;         
        }

        /// <summary>
        /// regisztráció - bankszámlaszám létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public long CreateBankAccount(CompanyGroup.Domain.RegistrationModule.BankAccountCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.RegistrationModule.BankAccountCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         CustomerRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createBankAccount", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            long bankAccountRecId = 0;

            long.TryParse(xml, out bankAccountRecId);

            return bankAccountRecId;            
        }
    }
}
