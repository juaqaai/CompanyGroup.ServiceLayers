using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.WebApi;
using CompanyGroup.WebApi.Controllers;
using System.Net.Http.Headers;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    [TestClass]
    public class ControllerBase
    {
        private static string BaseAddress = "http://localhost/CompanyGroup.WebApi/api/";

        protected HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(BaseAddress);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
