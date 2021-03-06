﻿using System;
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
    public class RegistrationControllerTest : ControllerBase
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
        public void GetByIdTest()
        {
            Uri requestUri = null;

            CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request = new CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey("513beaa96ee0120ae84a1b67", "");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Registration/GetByKey", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            CompanyGroup.Dto.RegistrationModule.Registration result = response.Content.ReadAsAsync<CompanyGroup.Dto.RegistrationModule.Registration>().Result;

            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void UndoChangePasswordTest()
        //{
        //    CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest request = new CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest();

        //    Uri requestUri = null;

        //    HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ContactPerson/UndoChangePassword", request).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        requestUri = response.Headers.Location;
        //    }
        //    else
        //    {
        //        TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    CompanyGroup.Dto.PartnerModule.UndoChangePassword changePassword = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.UndoChangePassword>().Result;

        //    Assert.IsNotNull(changePassword);
        //}

        //[TestMethod]
        //public void GetByIdTest()
        //{
        //    CompanyGroup.Dto.PartnerModule.GetContactPersonByIdRequest request = new CompanyGroup.Dto.PartnerModule.GetContactPersonByIdRequest("alma", "hu");

        //    Uri requestUri = null;

        //    HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ContactPerson/GetById", request).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        requestUri = response.Headers.Location;
        //    }
        //    else
        //    {
        //        TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    CompanyGroup.Dto.PartnerModule.ContactPerson contactPerson = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.ContactPerson>().Result;

        //    Assert.IsNotNull(contactPerson);
        //}

        //[TestMethod]
        //public void ForgetPasswordTest()
        //{
        //    CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest request = new CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest();

        //    Uri requestUri = null;

        //    HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ContactPerson/ForgetPassword", request).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        requestUri = response.Headers.Location;
        //    }
        //    else
        //    {
        //        TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //    }

        //    CompanyGroup.Dto.PartnerModule.ForgetPassword changePassword = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.ForgetPassword>().Result;

        //    Assert.IsNotNull(changePassword);
        //}

    }
}
