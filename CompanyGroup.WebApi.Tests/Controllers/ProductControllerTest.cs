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
    public class ProductControllerTest : ControllerBase
    {

        [TestMethod]
        public void GetProductsTest()
        {
            CompanyGroup.Dto.WebshopModule.GetAllProductRequest request = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetProducts", request).Result;

            CompanyGroup.Dto.WebshopModule.Products products = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Products>().Result;

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void GetBannerListTest()
        {
            CompanyGroup.Dto.WebshopModule.GetBannerListRequest request = new CompanyGroup.Dto.WebshopModule.GetBannerListRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetBannerList", request).Result;

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.BannerList>().Result;

            Assert.IsNotNull(bannerList);
        }

        [TestMethod]
        public void GetPriceListTest()
        {
            CompanyGroup.Dto.WebshopModule.GetPriceListRequest request = new CompanyGroup.Dto.WebshopModule.GetPriceListRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetPriceList", request).Result;

            CompanyGroup.Dto.WebshopModule.PriceList priceList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.PriceList>().Result;

            Assert.IsNotNull(priceList);
        }

        [TestMethod]
        public void GetItemByProductIdTest()
        {
            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest("", "", "", "HUF");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetItemByProductId", request).Result;

            CompanyGroup.Dto.WebshopModule.Product product = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Product>().Result;

            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void GetCompletionListTest()
        {
            CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request = new CompanyGroup.Dto.WebshopModule.ProductListComplationRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetCompletionList", request).Result;

            CompanyGroup.Dto.WebshopModule.CompletionList completionList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompletionList>().Result;

            Assert.IsNotNull(completionList);
        }

        [TestMethod]
        public void GetCompatibleProductsTest()
        {
            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Products/GetCompatibleProducts", request).Result;

            CompanyGroup.Dto.WebshopModule.CompatibleProducts compatibleProducts = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompatibleProducts>().Result;

            Assert.IsNotNull(compatibleProducts);
        }
    }
}
