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
            CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest("visitorId", "HU", true, true, "", "", "", "", "", 0, 0, 0);

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Invoice/GetList", request).Result;

            List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> invoiceInfo = response.Content.ReadAsAsync<List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>().Result;

            Assert.IsNotNull(invoiceInfo);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            string salesId = "";

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Invoice/GetById/{0}", salesId)).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.InvoiceInfo>().Result;

                salesId = invoiceInfo.SalesId;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Assert.IsTrue(!String.IsNullOrEmpty(salesId));
        }

        [TestMethod]
        public void GetAllTest()
        {
            string salesId = "";

            HttpResponseMessage response = CreateHttpClient().GetAsync("Invoice/GetAll").Result;

            CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.InvoiceInfo>().Result;

            if (response.IsSuccessStatusCode)
            {
                salesId = invoiceInfo.SalesId;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            Assert.IsNotNull(!String.IsNullOrEmpty(salesId));
        }

    }
}
