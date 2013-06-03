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
            CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest("94A7414E5C5C47719641E6CC7E20D77C", "HU", true, false, "", "", "", "", "", 0, 0, 1, 100);

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Invoice/GetInvoiceInfo", request).Result;

            CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.InvoiceInfo>().Result;

            Assert.IsNotNull(invoiceInfo);
        }

        //[TestMethod]
        //public void GetByIdTest()
        //{
        //    long count = 0;

        //    HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Invoice/GetById/{0}", "3001")).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        CompanyGroup.Dto.PartnerModule.InvoiceInfo invoiceInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.InvoiceInfo>().Result;

        //        count = invoiceInfo.ListCount;
        //    }
        //    else
        //    {
        //        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    Assert.IsTrue(count > 0);
        //}
    }
}
