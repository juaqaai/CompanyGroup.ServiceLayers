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
    public class ProductRepository : RepositoryBase
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
            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository();

            long count = 100;

            string categories = Helpers.ConvertData.ConvertStringListToDelimitedString(new List<string>() { "B004" });

            CompanyGroup.Domain.WebshopModule.Products products = repository.GetList(0, "hrp", "", categories, "", "", false, false, false, false, false, 0, "", "", 1, 1, 30, count, String.Empty);

            Assert.IsTrue(products.Count > 0);

            Assert.IsNotNull(count);
        }

        [TestMethod]
        public void GetItem()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository();

            CompanyGroup.Domain.WebshopModule.Product product = repository.GetItem(0, "WX950BTHDTRU", "hrp");

            Assert.IsTrue(product != null);
        }

        [TestMethod]
        public void GetBannerList()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository repository = new CompanyGroup.Data.WebshopModule.ProductRepository();

            CompanyGroup.Domain.WebshopModule.BannerProducts products = repository.GetBannerList("hrp", "", "", "", "");

            Assert.IsNotNull(products);
        }

    }
}
