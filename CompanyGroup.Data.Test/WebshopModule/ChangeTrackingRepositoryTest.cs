using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test
{
    /// <summary>
    /// UnitTest1
    /// </summary>
    [TestClass]
    public class ChangeTrackingRepository : RepositoryBase
    {
        public ChangeTrackingRepository()
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
        public void InventSumCTTest()
        {
            CompanyGroup.Domain.WebshopModule.IChangeTrackingRepository repository = new CompanyGroup.Data.WebshopModule.ChangeTrackingRepository();

            CompanyGroup.Domain.WebshopModule.InventSumList result = repository.InventSumCT(0);

            Assert.IsTrue(result.Count > 0);            
        }

        [TestMethod]
        public void PriceDiscTableCTTest()
        {
            CompanyGroup.Domain.WebshopModule.IChangeTrackingRepository repository = new CompanyGroup.Data.WebshopModule.ChangeTrackingRepository();

            CompanyGroup.Domain.WebshopModule.PriceDiscTableList result = repository.PriceDiscTableCT(0);

            Assert.IsTrue(result.Count > 0);
        }
    }
}
