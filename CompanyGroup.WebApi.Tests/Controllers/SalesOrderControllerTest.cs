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
    public class SalesOrderControllerTest : ControllerBase
    {
        [TestMethod]
        public void GetOrderInfoTest()
        {
            CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest request = new CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest("alma", "hu");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("SalesOrder/GetOrderInfo", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                List<CompanyGroup.Dto.PartnerModule.OrderInfo> orderInfo = response.Content.ReadAsAsync<List<CompanyGroup.Dto.PartnerModule.OrderInfo>>().Result;

                Assert.IsNotNull(orderInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

    }
}
