using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test.PartnerModule
{
    /// <summary>
    /// Summary description for VisitorRepository
    /// </summary>
    [TestClass]
    public class VisitorRepositoryTest
    {
        public VisitorRepositoryTest()
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
        public void GetItemById()
        {
            CompanyGroup.Domain.PartnerModule.IVisitorRepository repository = new CompanyGroup.Data.PartnerModule.VisitorRepository();

            CompanyGroup.Domain.PartnerModule.Visitor visitor = repository.GetItemById("865D102B5C834B99858C390B269C68A6");    //CBB75B0D-D8C1-4863-8456-7A7DA685E164

            Assert.IsNotNull(visitor);
        }

        [TestMethod]
        public void InsertTest()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository = new CompanyGroup.Data.PartnerModule.CustomerRepository();

            List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDatas = customerRepository.SignIn("ipon", "gild4MAX19");

            CompanyGroup.Domain.PartnerModule.Visitor visitor = new CompanyGroup.Domain.PartnerModule.Visitor(visitorDatas); 

            CompanyGroup.Domain.PartnerModule.IVisitorRepository repository = new CompanyGroup.Data.PartnerModule.VisitorRepository();

            repository.Add(visitor);    

            Assert.IsNotNull(visitor);
        }
    }
}
