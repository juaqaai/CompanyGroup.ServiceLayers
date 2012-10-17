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
    public class ProductRepository
    {
        public ProductRepository()
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
        public void GetList()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),  
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017), 
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

            long count = 0;

            CompanyGroup.Domain.WebshopModule.Products products = repository.GetList("hrp", new List<string>(), new List<string>(), new List<string>(), new List<string>(), false, false, false, false, false, "", "100000", 1, "", 0, 0, 30, ref count);

            Assert.IsTrue(products.Count > 0);            
        }

        [TestMethod]
        public void GetItem()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

            CompanyGroup.Domain.WebshopModule.Product product = repository.GetItem("01279001", "hrp");

            Assert.IsTrue(product != null);
        }

        [TestMethod]
        public void GetBannerList()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),  
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017), 
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

            CompanyGroup.Domain.WebshopModule.Products products = repository.GetBannerList("hrp", 30);

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void CreateIndexes()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);
            
            repository.CreateIndexes();
        }
    }
}
