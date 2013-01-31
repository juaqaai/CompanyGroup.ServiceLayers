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
    public class PictureControllerTest : ControllerBase
    {

        [TestMethod]
        public void GetListByProductTest()
        {
            CompanyGroup.Dto.PartnerModel.PictureFilterRequest request = new CompanyGroup.Dto.PartnerModel.PictureFilterRequest("hrp", "");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Picture/GetListByProduct", request).Result;

            CompanyGroup.Dto.WebshopModule.Pictures pictures = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Pictures>().Result;

            Assert.IsNotNull(pictures);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            string productId = ""; 
            string recId = "";
            string maxWidth = "";
            string maxHeight = "";

            System.IO.Stream stream = null;

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Picture/GetItem/{0}/{1}/{2}/{3}", productId, recId, maxWidth, maxHeight)).Result;

            if (response.IsSuccessStatusCode)
            {
                stream = response.Content.ReadAsStreamAsync().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            Assert.IsNotNull(stream);

            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void GetItemByIdTest()
        {
            System.IO.Stream stream = null;
            
            string pictureId = "";
            string maxWidth = "";
            string maxHeight = "";

            HttpResponseMessage response = CreateHttpClient().GetAsync(String.Format("Picture/GetItemById/{0}/{1}/{2}", pictureId, maxWidth, maxHeight)).Result;

            stream = response.Content.ReadAsStreamAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                stream = response.Content.ReadAsStreamAsync().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            Assert.IsNotNull(stream);
        }

    }
}
