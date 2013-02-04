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
    public class CustomerControllerTest : ControllerBase
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
        public void GetAddressZipCodesTest()
        {
            CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request = new Dto.PartnerModule.AddressZipCodeRequest() { DataAreaId = "hrp", Prefix = "11" };

            HttpResponseMessage response = CreateHttpClient().GetAsync("Customer/GetAddressZipCodes/hrp/11").Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.PartnerModule.AddressZipCodes addressZipCodes = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.AddressZipCodes>().Result;

                Assert.IsNotNull(addressZipCodes);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

        [TestMethod]
        public void GetCustomerRegistrationTest()
        {
            CompanyGroup.Dto.PartnerModule.GetCustomerRegistrationRequest request = new Dto.PartnerModule.GetCustomerRegistrationRequest("alma", "hrp");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Customer/GetCustomerRegistration", request).Result;

            string registrationId = String.Empty;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.RegistrationModule.Registration registration = response.Content.ReadAsAsync<CompanyGroup.Dto.RegistrationModule.Registration>().Result;

                registrationId = registration.RegistrationId;
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Assert.IsTrue(!String.IsNullOrEmpty(registrationId));
        }

        [TestMethod]
        public void GetDeliveryAddressesTest()
        {
            CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest request = new Dto.PartnerModule.GetDeliveryAddressesRequest("hrp", "alma");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Customer/GetDeliveryAddresses", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.DeliveryAddresses>().Result;

                Assert.IsNotNull(deliveryAddresses);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

    }
}
