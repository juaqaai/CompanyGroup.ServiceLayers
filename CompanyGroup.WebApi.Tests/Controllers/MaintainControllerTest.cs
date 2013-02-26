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
    public class MaintainControllerTest : ControllerBase
    {

        [TestMethod]
        public void StockUpdateTest()
        {
            CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request = new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest("hrp", "KULSO", "WN820BAG");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Maintain/StockUpdate", request).Result;

            if (response.IsSuccessStatusCode)
            {
                Uri requestUri = response.Headers.Location;

                Assert.IsTrue(response.IsSuccessStatusCode);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }


        }

    }
}
