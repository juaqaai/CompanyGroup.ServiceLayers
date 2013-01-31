using CompanyGroup.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using CompanyGroup.ApplicationServices.PartnerModule;
using CompanyGroup.Dto.PartnerModule;
using System.Net.Http;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    
    
    /// <summary>
    ///This is a test class for VisitorControllerTest and is intended
    ///to contain all VisitorControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VisitorControllerTest : ControllerBase
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
        ///A test for ChangeCurrency
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void ChangeCurrencyTest()
        {
            ChangeCurrencyRequest request = new ChangeCurrencyRequest("", "HUF");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Visitor/ChangeCurrency", request).Result;

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for ChangeLanguage
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void ChangeLanguageTest()
        {
            ChangeLanguageRequest request = new ChangeLanguageRequest("", "hu");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Visitor/ChangeLanguage", request).Result;

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for GetVisitorInfo
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void GetVisitorInfoTest()
        {
            VisitorInfoRequest request = new VisitorInfoRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Visitor/GetVisitorInfo", request).Result;

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for SignIn
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void SignInTest()
        {
            SignInRequest request = new SignInRequest("hrp", "ipon", "gild4MAX19", "127.0.0.1");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Visitor/SignIn", request).Result;

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for SignOut
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod]
        public void SignOutTest()
        {
            SignOutRequest request = new SignOutRequest("hrp", "127.0.0.1");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Visitor/SignOut", request).Result;

            Assert.IsNotNull(response);
        }
    }
}
