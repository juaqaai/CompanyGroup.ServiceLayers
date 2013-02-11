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
    public class ShoppingCartControllerTest : ControllerBase
    {
        [TestMethod]
        public void AssociateCartTest()
        {
            CompanyGroup.Dto.WebshopModule.AssociateCartRequest request = new CompanyGroup.Dto.WebshopModule.AssociateCartRequest("alma", "", "hu");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/AssociateCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void AddCartTest()
        {
            CompanyGroup.Dto.WebshopModule.AddCartRequest request = new CompanyGroup.Dto.WebshopModule.AddCartRequest("hu", "alma", "teszt1");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/AddCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void SaveCartTest()
        {
            CompanyGroup.Dto.WebshopModule.SaveCartRequest request = new CompanyGroup.Dto.WebshopModule.SaveCartRequest("hu", "alma", 1, "teszt2");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/SaveCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void RemoveCartTest()
        {
            CompanyGroup.Dto.WebshopModule.RemoveCartRequest request = new CompanyGroup.Dto.WebshopModule.RemoveCartRequest(1, "hu", "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/RemoveCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void ActivateCartTest()
        {
            CompanyGroup.Dto.WebshopModule.ActivateCartRequest request = new CompanyGroup.Dto.WebshopModule.ActivateCartRequest(1, "hu", "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/ActivateCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void AddLineTest()
        {
            CompanyGroup.Dto.WebshopModule.AddLineRequest request = new CompanyGroup.Dto.WebshopModule.AddLineRequest(1, "ZYWALL5", "hu", "hrp", 2, "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/AddLine", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCartAndLeasingOptions = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>().Result;

                Assert.IsNotNull(shoppingCartAndLeasingOptions);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void RemoveLineTest()
        {
            CompanyGroup.Dto.WebshopModule.RemoveLineRequest request = new CompanyGroup.Dto.WebshopModule.RemoveLineRequest(1, 1, "hu", "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/RemoveLine", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCartAndLeasingOptions = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>().Result;

                Assert.IsNotNull(shoppingCartAndLeasingOptions);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void UpdateLineQuantityTest()
        {
            CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest request = new CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest(1, 1, "hu", "hrp", 2, "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/UpdateLineQuantity", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCartAndLeasingOptions = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>().Result;

                Assert.IsNotNull(shoppingCartAndLeasingOptions);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetShoppingCartInfoTest()
        {
            CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest request = new CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest(1, "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/GetShoppingCartInfo", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetCartByKeyTest()
        {
            CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest request = new CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest("hu", 1, "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/GetCartByKey", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCart collection = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCart>().Result;

                Assert.IsNotNull(collection);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void GetActiveCartTest()
        {
            CompanyGroup.Dto.WebshopModule.GetActiveCartRequest request = new CompanyGroup.Dto.WebshopModule.GetActiveCartRequest("hu", "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/GetActiveCart", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>().Result;

                Assert.IsNotNull(shoppingCartInfo);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [TestMethod]
        public void CreateOrderTest()
        {
            CompanyGroup.Dto.WebshopModule.SalesOrderCreateRequest request = new CompanyGroup.Dto.WebshopModule.SalesOrderCreateRequest(1, "hu", "alma");

            Uri requestUri = null;

            HttpResponseMessage response = CreateHttpClient().PostAsJsonAsync("ShoppingCart/CreateOrder", request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;

                CompanyGroup.Dto.WebshopModule.OrderFulFillment orderFulFillment = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.OrderFulFillment>().Result;

                Assert.IsNotNull(orderFulFillment);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
