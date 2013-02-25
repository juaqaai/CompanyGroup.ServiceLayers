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
        public void GetProductsTest()
        {
            CompanyGroup.Dto.WebshopModule.GetAllProductRequest request = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest() { CurrentPageIndex = 1, ItemsOnPage = 30 };

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetProducts", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.Products products = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Products>().Result;

                Assert.IsNotNull(products);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetBannerListTest()
        {
            CompanyGroup.Dto.WebshopModule.GetBannerListRequest request = new CompanyGroup.Dto.WebshopModule.GetBannerListRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetBannerList", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.BannerList bannerList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.BannerList>().Result;

                Assert.IsNotNull(bannerList);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetPriceListTest()
        {
            CompanyGroup.Dto.WebshopModule.GetPriceListRequest request = new CompanyGroup.Dto.WebshopModule.GetPriceListRequest();

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetPriceList", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.PriceList priceList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.PriceList>().Result;

                Assert.IsNotNull(priceList);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetItemByProductIdTest()
        {
            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest("NWA3160", "hrp", "alma", "HUF");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetItemByProductId", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.Product product = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Product>().Result;

                Assert.IsNotNull(product);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetCompletionListTest()
        {
            CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request = new CompanyGroup.Dto.WebshopModule.ProductListComplationRequest() { Prefix = "del" };

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetCompletionList", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.CompletionList completionList = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompletionList>().Result;

                Assert.IsNotNull(completionList);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetCompatibleProductsTest()
        {
            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest("NWA3160", "hrp", "alma", "HUF");

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/GetCompatibleProducts", request).Result;

            if (response.IsSuccessStatusCode)
            {
                CompanyGroup.Dto.WebshopModule.CompatibleProducts compatibleProducts = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompatibleProducts>().Result;

                Assert.IsNotNull(compatibleProducts);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void StockUpdate()
        {
            CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request = new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest("hrp", "", 3);

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("Product/StockUpdate", request).Result;

            if (response.IsSuccessStatusCode)
            {
                Assert.IsNotNull(response);
            }
            else
            {
                TestContext.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
