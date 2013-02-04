using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.WebApi;
using CompanyGroup.WebApi.Controllers;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    [TestClass]
    public class PictureControllerTest : ControllerBase
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

        [TestMethod]
        public void GetListByProductTest()
        {
            CompanyGroup.Dto.WebshopModule.PictureFilterRequest request = new CompanyGroup.Dto.WebshopModule.PictureFilterRequest("hrp", "0021165103535");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Picture/GetListByProduct", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.Pictures pictures = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Pictures>().Result;

                Assert.IsNotNull(pictures);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetByIdTest()
        {
            string productId = "0021165103535";
            string recId = "5637192799";
            string maxWidth = "120";
            string maxHeight = "120";

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Picture/GetItem/{0}/{1}/{2}/{3}", productId, recId, maxWidth, maxHeight)).Result;

            if (response.IsSuccessStatusCode)
            {
                System.IO.Stream stream = response.Content.ReadAsStreamAsync().Result;

                Assert.IsNotNull(stream);

                Assert.IsTrue(stream.Length > 0);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetItemByIdTest()
        {          
            string pictureId = "40";
            string maxWidth = "120";
            string maxHeight = "120";

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Picture/GetItemById/{0}/{1}/{2}", pictureId, maxWidth, maxHeight)).Result;

            if (response.IsSuccessStatusCode)
            {
                System.IO.Stream stream = stream = response.Content.ReadAsStreamAsync().Result;

                Assert.IsNotNull(stream);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

    }
}
