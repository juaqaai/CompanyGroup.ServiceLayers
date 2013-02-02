using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.WebApi;
using CompanyGroup.WebApi.Controllers;
using System.Net.Http.Headers;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    [TestClass]
    public class NewsletterControllerTest : ControllerBase
    {

        [TestMethod]
        public void GetNewsletterCollection()
        {
            CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest("hu", "5092cfe46ee0121e98101481", "");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Newsletter/GetCollection", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            CompanyGroup.Dto.WebshopModule.NewsletterCollection collection = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.NewsletterCollection>().Result;

            Assert.IsNotNull(collection);
        }

    }
}
