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
    public class InvoiceControllerTest : ControllerBase
    {

        [TestMethod]
        public void GetListTest()
        {
            CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest("visitorId", "HU", true, true, "", "", "", "", "", 0, 0, 0, 0, new List<int>());

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Invoice/GetList", request).Result;

            List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> invoiceInfo = response.Content.ReadAsAsync<List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>().Result;

            Assert.IsNotNull(invoiceInfo);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            long count = 0;

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Invoice/GetById/{0}", "3001")).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.InvoiceInfo>().Result;

                count = invoiceInfo.ListCount;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Assert.IsTrue(count > 0);
        }
    }
}
