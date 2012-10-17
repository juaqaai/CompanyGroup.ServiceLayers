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
    public class ICustomerTest
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
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost/CompanyGroup.Services")]
        public void AddressZipCodesTest()
        {
            string prefix = "1"; 
            string dataAreaId = "hrp"; 

            string baseUrl = "http://localhost/CompanyGroup.Services/Customer.svc/";

            RestSharp.RestClient client = new RestSharp.RestClient(baseUrl);

            RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.POST);

            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = "AddressZipCodes";

            request.AddBody(new { Prefix = prefix, DataAreaId = dataAreaId } );

            //request.AddParameter("prefix", prefix);

            //request.AddParameter("dataAreaId", dataAreaId);

            //RestSharp.RestResponse response = client.Execute(request);

            RestSharp.RestResponse<AddressZipCodes> response = client.Execute<AddressZipCodes>(request);

            Assert.IsNotNull(response.Content);

        }

        [TestMethod()]
        public void Hello()
        {
            //http://localhost/CompanyGroup.Services/Customer.svc/hello
            string baseUrl = "http://localhost/CompanyGroup.Services/Customer.svc/";

            RestSharp.RestClient client = new RestSharp.RestClient(baseUrl);

            RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.GET);

            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = "hello";

            RestSharp.RestResponse response = client.Execute(request);

            Assert.IsNotNull(response.Content);

        }
    }
}
