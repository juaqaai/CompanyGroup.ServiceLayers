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

        [TestMethod]
        public void GetAddressZipCodesTest()
        {
            CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request = new Dto.PartnerModule.AddressZipCodeRequest() { DataAreaId = "hrp", Prefix = "11" };

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Customer/GetAddressZipCodes", request).Result;

            CompanyGroup.Dto.PartnerModule.AddressZipCodes addressZipCodes = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.AddressZipCodes>().Result;

            Assert.IsNotNull(addressZipCodes);
        }

        [TestMethod]
        public void GetCustomerRegistrationTest()
        {
            string visitorId = "";

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Customer/GetCustomerRegistration/{0}/hrp", visitorId)).Result;

            string registrationId = String.Empty;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.RegistrationModule.Registration registration = response.Content.ReadAsAsync<CompanyGroup.Dto.RegistrationModule.Registration>().Result;

                registrationId = registration.RegistrationId;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Assert.IsTrue(!String.IsNullOrEmpty(registrationId));
        }

        [TestMethod]
        public void GetDeliveryAddressesTest()
        {
            CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest request = new Dto.PartnerModule.GetDeliveryAddressesRequest("hrp", "visitorId");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Customer/GetDeliveryAddresses", request).Result;

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.DeliveryAddresses>().Result;

            Assert.IsNotNull(deliveryAddresses);
        }

    }
}
