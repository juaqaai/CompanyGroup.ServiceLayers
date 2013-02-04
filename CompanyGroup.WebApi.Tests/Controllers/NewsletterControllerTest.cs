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
            CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest("hu", "alma", "");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Newsletter/GetCollection", request).Result;

            if (response.IsSuccessStatusCode)
            {
                Uri requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.NewsletterCollection collection = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.NewsletterCollection>().Result;

                Assert.IsNotNull(collection);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }


        }

    }
}
