using CompanyGroup.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using CompanyGroup.Dto;

namespace CompanyGroup.Data.Test
{
    
    /// <summary>
    ///This is a test class for ICustomerTest and is intended
    ///to contain all ICustomerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ICatalogueTest
    {

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for AddressZipCodes
        ///</summary>
        [TestMethod()]
        public void GetStructureTest()
        {
            string dataAreaId = "hrp";
            string textFilter = "";

            string baseUrl = "http://localhost/CompanyGroup.Services/Catalogue.svc/";

            RestSharp.RestClient client = new RestSharp.RestClient(baseUrl);

            RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.POST);

            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = "GetStructures";

            request.AddBody(new { DataAreaId = dataAreaId, ActionFilter = false, BargainFilter = false, NewFilter = false, StockFilter = false, TextFilter = textFilter });

            //RestSharp.RestResponse response = client.Execute(request);

            RestSharp.RestResponse<Structures> response = client.Execute<Structures>(request);

            Assert.IsNotNull(response.Content);

        }
    }
}
