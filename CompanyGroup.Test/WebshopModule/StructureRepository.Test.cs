using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DataUnitTest
    {
        public DataUnitTest()
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
            CompanyGroup.DataInterfaces.IDaoFactory daoFactory = new NHibernateDaoFactory();

            CompanyGroup.Core.DataInterfaces.ICustomerDao customerDao = daoFactory.GetCustomerDao();

            List<CompanyGroup.Core.Domain.CustomerLetter> customerLetter = customerDao.GetCustomerLetter("V002020", "hrp");

            Assert.IsTrue(customerLetter.Count > 0);
        }

        [TestMethod]
        public void GetAddressZipCode()
        {
            CompanyGroup.Core.DataInterfaces.IDaoFactory daoFactory = new NHibernateDaoFactory();

            CompanyGroup.Core.DataInterfaces.ICustomerDao customerDao = daoFactory.GetCustomerDao();

            List<CompanyGroup.Core.Domain.AddressZipCode> addresszipCode = customerDao.GetAddressZipCode("1", "hrp");

            Assert.IsTrue(addresszipCode.Count > 0);
        }

        [TestMethod]
        public void GetStructure()
        {
            CompanyGroup.Core.DataInterfaces.IDaoFactory daoFactory = new NHibernateDaoFactory();

            CompanyGroup.Core.DataInterfaces.IStructureDao structureDao = daoFactory.GetStructureDao(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "195.30.7.59"),  
                                                                                                     CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017), 
                                                                                                     CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "web"), "Catalogue");

            CompanyGroup.Core.Domain.Structures structures = structureDao.GetList("hrp", false, false, false, false, "");



            Assert.IsTrue(structures.Count > 0);            
        }
    }
}
