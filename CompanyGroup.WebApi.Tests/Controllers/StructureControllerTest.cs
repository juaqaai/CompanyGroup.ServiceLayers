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
    public class StructureControllerTest : ControllerBase
    {
        [TestMethod]
        public void GetStructuresTest()
        {
            CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request = new CompanyGroup.Dto.WebshopModule.GetAllStructureRequest();

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Structure/GetStructures", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.Structures structures = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Structures>().Result;

                Assert.IsNotNull(structures);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

    }
}
