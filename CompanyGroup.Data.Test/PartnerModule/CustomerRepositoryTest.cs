using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test.PartnerModule
{
    /// <summary>
    /// Summary description for CustomerRepository
    /// </summary>
    [TestClass]
    public class CustomerRepositoryTest
    {
        public CustomerRepositoryTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetCustomerLetter()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = repository.GetMailAddress("V002020", "hrp");

            Assert.IsNotNull(mailAddress);
        }

        [TestMethod]
        public void GetAddressZipCode()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.PartnerModule.AddressZipCode> addresszipCode = repository.GetAddressZipCode("1", "hrp");

            Assert.IsTrue(addresszipCode.Count > 0);
        }

        //[TestMethod]
        //public void SignIn()
        //{
        //    CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

        //    CompanyGroup.Domain.PartnerModule.LoginInfo loginInfo = repository.SignIn("elektroplaza", "58915891", "hrp");

        //    Assert.IsNotNull(loginInfo);
        //}
        [TestMethod]
        public void SignIn()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Domain.PartnerModule.Visitor visitor = repository.SignIn("elektroplaza", "58915891", "hrp");

            Assert.IsNotNull(visitor);
        }

        [TestMethod]
        public void GetCustomerPriceGroups()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> customerPriceGroups = repository.GetCustomerPriceGroups("V006199");

            Assert.IsNotNull(customerPriceGroups);
        }

        [TestMethod]
        public void GetDeliveryAddress()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddress = repository.GetDeliveryAddress("V006199", "hrp");

            Assert.IsNotNull(deliveryAddress);
        }

        [TestMethod]
        public void GetBankAccounts()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.PartnerModule.BankAccount> bankAccounts = repository.GetBankAccounts("V006199", "hrp");

            Assert.IsNotNull(bankAccounts);
        }

        [TestMethod]
        public void GetContactPersons()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.PartnerModule.ContactPerson> contactPersons = repository.GetContactPersons("V006199", "hrp");

            Assert.IsNotNull(contactPersons);
        }


        [TestMethod]
        public void GetCustomer()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository repository = new CompanyGroup.Data.PartnerModule.CustomerRepository(NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Domain.PartnerModule.Customer customer = repository.GetCustomer("V006199", "hrp");

            Assert.IsNotNull(customer);
        }
    }
}
