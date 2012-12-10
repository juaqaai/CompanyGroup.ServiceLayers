using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test.PartnerModule
{
    /// <summary>
    /// Summary description for InvoiceRepository
    /// </summary>
    [TestClass]
    public class InvoiceRepositoryTest
    {
        public InvoiceRepositoryTest()
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
        public void GetOrderDetailedLineInfoTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Visitor"));

            CompanyGroup.Domain.MaintainModule.IInvoiceRepository repository = new CompanyGroup.Data.MaintainModule.InvoiceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> invoices = repository.GetInvoiceDetailedLineInfo("hrp");

            Assert.IsTrue(invoices.Count > 0);
        }
    }
}
