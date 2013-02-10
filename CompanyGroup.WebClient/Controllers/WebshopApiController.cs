﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class WebshopApiController : ApiBaseController
    {
        // GET api/webshop/Catalogue
        /// <summary>
        /// katalógus lekérdezése, kiinduló állapot
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetInitialCatalogue")]
        public CompanyGroup.WebClient.Models.Catalogue GetInitialCatalogue()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //struktúrák lekérdezése
            CompanyGroup.Dto.WebshopModule.GetAllStructureRequest allStructure = new CompanyGroup.Dto.WebshopModule.GetAllStructureRequest()
            {
                DiscountFilter = false,
                SecondhandFilter = false,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                HrpFilter = true,
                BscFilter = true,
                IsInNewsletterFilter = false,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                StockFilter = false,
                TextFilter = String.Empty,
                PriceFilter = "0",
                PriceFilterRelation = "0"
            };

            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllStructureRequest, CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetAll", allStructure);

            //katalógus lekérdezése
            CompanyGroup.Dto.WebshopModule.GetAllProductRequest allProduct = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest()
            {
                ActionFilter = false,
                BargainFilter = false,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                CurrentPageIndex = 1,
                HrpFilter = true,
                BscFilter = true,
                IsInNewsletterFilter = false,
                ItemsOnPage = 30,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                Sequence = 0,
                StockFilter = false,
                TextFilter = String.Empty,
                PriceFilter = "0",
                PriceFilterRelation = "0",
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllProductRequest, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetProducts", allProduct);

            //banner lista lekérdezése
            CompanyGroup.Dto.WebshopModule.GetBannerListRequest bannerListRequest = new CompanyGroup.Dto.WebshopModule.GetBannerListRequest()
            {
                BscFilter = true,
                HrpFilter = true,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetBannerListRequest, CompanyGroup.Dto.WebshopModule.BannerList>("Product", "GetBannerList", bannerListRequest);

            //kosár lekérdezések     
            List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedShoppingCart;

            List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedShoppingCart;

            CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart;

            CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions;

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

            if (visitor.IsValidLogin && visitorData.CartId > 0)
            {
                CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest shoppingCartInfoRequest = new CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest(visitorData.CartId, visitor.Id);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetShoppingCartInfo", shoppingCartInfoRequest);


                activeCart = shoppingCartInfo.ActiveCart;

                openedShoppingCart = shoppingCartInfo.OpenedItems;

                storedShoppingCart = shoppingCartInfo.StoredItems;

                leasingOptions = shoppingCartInfo.LeasingOptions;
            }
            else
            {
                activeCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

                openedShoppingCart = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

                storedShoppingCart = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

                leasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions();
            }

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest getDeliveryAddresses = new CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest() { DataAreaId = ApiBaseController.DataAreaId, VisitorId = visitor.Id };

                deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", getDeliveryAddresses);
            }
            else
            {
                deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();
            }

            CompanyGroup.WebClient.Models.Catalogue model = new CompanyGroup.WebClient.Models.Catalogue(structures, products, visitor, activeCart, openedShoppingCart, storedShoppingCart, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, deliveryAddresses, bannerList, leasingOptions);

            //aktív kosár azonosítójának mentése http cookie-ba
            if (activeCart.Id > 0)
            {
                visitorData.CartId = activeCart.Id;

                this.WriteCookie(visitorData);
            }

            return model;
        }

        /// <summary>
        /// terméklista lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns>terméklista objektum és a látogató objektum JSON formátumban</returns>
        [HttpPost]
        [ActionName("GetProducts")]
        public CompanyGroup.WebClient.Models.ProductCatalogue GetProducts(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request) 
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            //CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            request.VisitorId = visitorData.VisitorId; //visitor.Id;   

            request.Currency = visitorData.Currency;

            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllProductRequest>("Product", "GetProducts", request);

            CompanyGroup.Dto.WebshopModule.Products products = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Products>().Result : new CompanyGroup.Dto.WebshopModule.Products();

            return new CompanyGroup.WebClient.Models.ProductCatalogue(products, this.GetVisitor(visitorData));
        }

        /// <summary>
        /// részletes termék adatlap
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Details")]
        public CompanyGroup.WebClient.Models.ProductCatalogueItem Details()
        {
            return GetDetails(CompanyGroup.Helpers.QueryStringParser.GetString("ProductId"));
        }

        /// <summary>
        /// részletes termék adatlap
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetDetails")]
        public CompanyGroup.WebClient.Models.ProductCatalogueItem GetDetails(string productId)
        {
            if (String.IsNullOrWhiteSpace(productId)) { productId = "PGI7BK"; }

            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest()
            {
                ProductId = productId,
                DataAreaId = ApiBaseController.DataAreaId,
                VisitorId = visitorData.VisitorId,
                Currency = visitorData.Currency
            };

            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest>("Product", "GetItemByProductId", request);

            CompanyGroup.Dto.WebshopModule.Product product = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Product>().Result : new CompanyGroup.Dto.WebshopModule.Product();

            CompanyGroup.Dto.WebshopModule.CompatibleProducts compatibleProducts = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest, CompanyGroup.Dto.WebshopModule.CompatibleProducts>("Product", "GetCompatibleProducts", request);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ProductCatalogueItem(product, compatibleProducts.Items, compatibleProducts.ReverseItems, visitor);
        }

        /// <summary>
        /// terméknév alapján kiegészítő lista lekérdezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCompletionListAllProduct")]
        public CompanyGroup.WebClient.Models.CompletionList GetCompletionListAllProduct()
        {
            string prefix = CompanyGroup.Helpers.QueryStringParser.GetString("Prefix");

            CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request = new CompanyGroup.Dto.WebshopModule.ProductListComplationRequest() { Prefix = prefix };

            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.ProductListComplationRequest>("Product", "GetCompletionList", request);

            CompanyGroup.Dto.WebshopModule.CompletionList completionList = (response.IsSuccessStatusCode) ?  response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompletionList>().Result : new CompanyGroup.Dto.WebshopModule.CompletionList();

            return new CompanyGroup.WebClient.Models.CompletionList(completionList);
        }

        /// <summary>
        /// termékazonosító, cikkszám, terméknév alapján kiegészítő lista lekérdezés
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCompletionListBaseProduct")]
        public CompanyGroup.WebClient.Models.CompletionList GetCompletionListBaseProduct()
        {
            string prefix = CompanyGroup.Helpers.QueryStringParser.GetString("Prefix");
            
            CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request = new CompanyGroup.Dto.WebshopModule.ProductListComplationRequest() { Prefix = prefix };

            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.ProductListComplationRequest>("Product", "GetCompletionList", request);

            CompanyGroup.Dto.WebshopModule.CompletionList completionList = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.CompletionList>().Result : new CompanyGroup.Dto.WebshopModule.CompletionList();

            return new CompanyGroup.WebClient.Models.CompletionList(completionList);
        }

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SignIn")]
        public HttpResponseMessage SignIn(CompanyGroup.WebClient.Models.SignInRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "SignIn request can not be null!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Password), "A jelszó megadása kötelező!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.UserName), "A belépési név megadása kötelező!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                //előző belépés azonosítójának mentése
                string permanentObjectId = visitorData.PermanentId;

                CompanyGroup.Dto.PartnerModule.SignInRequest singnInRequest = new CompanyGroup.Dto.PartnerModule.SignInRequest(ApiBaseController.DataAreaId,
                                                                                                        request.UserName,
                                                                                                        request.Password,
                                                                                                        System.Web.HttpContext.Current.Request.UserHostAddress);

                CompanyGroup.Dto.PartnerModule.Visitor signInResponse = this.PostJSonData<CompanyGroup.Dto.PartnerModule.SignInRequest, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "SignIn", singnInRequest);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor(signInResponse);

                //válaszüzenet összeállítása
                List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedShoppingCart;

                List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedShoppingCart;

                CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart;

                CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions;

                CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

                CompanyGroup.WebClient.Models.CatalogueResponse catalogueResponse;

                CompanyGroup.Dto.WebshopModule.Products products;
                //CompanyGroup.Dto.WebshopModule.Catalogue catalogue;

                //nem sikerült a belépés 
                if (!visitor.LoggedIn)
                {
                    visitor.ErrorMessage = "A bejelentkezés nem sikerült!";

                    storedShoppingCart = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

                    openedShoppingCart  = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

                    activeCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

                    leasingOptions =  new CompanyGroup.Dto.WebshopModule.LeasingOptions();

                    deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();

                    products = new CompanyGroup.Dto.WebshopModule.Products();
                }
                else    //sikerült a belépés, http cookie beállítás, ...
                {
                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.Id), "A bejelentkezés nem sikerült! (üres azonosító)");

                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.CompanyId), "A bejelentkezés nem sikerült! (üres cégazonosító)");

                    //kosár társítása
                    CompanyGroup.Dto.WebshopModule.AssociateCartRequest associateRequest = new CompanyGroup.Dto.WebshopModule.AssociateCartRequest(visitor.Id, permanentObjectId, visitorData.Language);

                    CompanyGroup.Dto.WebshopModule.ShoppingCartInfo associateCart = this.PostJSonData<CompanyGroup.Dto.WebshopModule.AssociateCartRequest, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AssociateCart", associateRequest);

                    //aktív kosár beállítás
                    storedShoppingCart = associateCart.StoredItems;
                    
                    openedShoppingCart = associateCart.OpenedItems;
                    
                    activeCart = associateCart.ActiveCart;
                    
                    leasingOptions = associateCart.LeasingOptions;
 
                    //szállítási címek lekérdezése
                    CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest deliveryAddressRequest = new CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest(WebshopApiController.DataAreaId, visitor.Id);

                    deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", deliveryAddressRequest);

                    //visitor adatok http sütibe írása     
                    this.WriteCookie(new CompanyGroup.WebClient.Models.VisitorData(visitor.Id, visitor.LanguageId, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, visitor.Currency, visitor.Id, associateCart.ActiveCart.Id, visitorData.RegistrationId));

                    visitor.ErrorMessage = String.Empty;

                    //katalógus lekérdezése
                    CompanyGroup.Dto.WebshopModule.GetAllProductRequest allProduct = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest()
                    {
                        ActionFilter = false,
                        BargainFilter = false,
                        Category1IdList = new List<string>(),
                        Category2IdList = new List<string>(),
                        Category3IdList = new List<string>(),
                        Currency = visitorData.Currency,
                        CurrentPageIndex = 1,
                        HrpFilter = true,
                        BscFilter = true,
                        IsInNewsletterFilter = false,
                        ItemsOnPage = 30,
                        ManufacturerIdList = new List<string>(),
                        NewFilter = false,
                        Sequence = 0,
                        StockFilter = false,
                        TextFilter = String.Empty,
                        PriceFilter = "0",
                        PriceFilterRelation = "0",
                        VisitorId = visitor.Id
                    };

                    products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllProductRequest, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetProducts", allProduct);
                }

                catalogueResponse = new CompanyGroup.WebClient.Models.CatalogueResponse(products,
                                                                                        visitor,
                                                                                        activeCart,
                                                                                        openedShoppingCart,
                                                                                        storedShoppingCart,
                                                                                        visitorData.IsShoppingCartOpened,
                                                                                        visitorData.IsCatalogueOpened,
                                                                                        deliveryAddresses,
                                                                                        leasingOptions);

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.CatalogueResponse>(HttpStatusCode.OK, catalogueResponse);

                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                CompanyGroup.WebClient.Models.CatalogueResponse catalogueResponse = new CompanyGroup.WebClient.Models.CatalogueResponse(new CompanyGroup.Dto.WebshopModule.Products(),
                    new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = String.Format("A bejelentkezés nem sikerült! ({0} - {1})", ex.Message, ex.StackTrace) },
                    new CompanyGroup.Dto.WebshopModule.ShoppingCart(),
                    new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(),
                    new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(),
                    false,
                    false,
                    new CompanyGroup.Dto.PartnerModule.DeliveryAddresses(),
                    new CompanyGroup.Dto.WebshopModule.LeasingOptions());

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.ApiMessage>(HttpStatusCode.InternalServerError, new CompanyGroup.WebClient.Models.ApiMessage(String.Format("A bejelentkezés nem sikerült! ({0} - {1})", ex.Message, ex.StackTrace)));

                throw new HttpResponseException(httpResponseMessage);
            }
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SignOut")]
        public HttpResponseMessage SignOut(CompanyGroup.Dto.PartnerModule.SignOutRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                CompanyGroup.Dto.PartnerModule.SignOutRequest req = new CompanyGroup.Dto.PartnerModule.SignOutRequest() { DataAreaId = ApiBaseController.DataAreaId, VisitorId = visitorData.VisitorId };

                CompanyGroup.Dto.ServiceResponse.Empty empty = this.PostJSonData<CompanyGroup.Dto.PartnerModule.SignOutRequest, CompanyGroup.Dto.ServiceResponse.Empty>("Visitor", "SignOut", req);

                visitorData.VisitorId = String.Empty;

                this.WriteCookie(visitorData);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor();

                //katalógus lekérdezése
                CompanyGroup.Dto.WebshopModule.GetAllProductRequest allProduct = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest()
                {
                    ActionFilter = false,
                    BargainFilter = false,
                    Category1IdList = new List<string>(),
                    Category2IdList = new List<string>(),
                    Category3IdList = new List<string>(),
                    Currency = visitorData.Currency,
                    CurrentPageIndex = 1,
                    HrpFilter = true,
                    BscFilter = true,
                    IsInNewsletterFilter = false,
                    ItemsOnPage = 30,
                    ManufacturerIdList = new List<string>(),
                    NewFilter = false,
                    Sequence = 0,
                    StockFilter = false,
                    TextFilter = String.Empty,
                    PriceFilter = "0",
                    PriceFilterRelation = "0",
                    VisitorId = visitor.Id
                };

                CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllProductRequest, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetProducts", allProduct);
                //CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", allProduct);

                CompanyGroup.WebClient.Models.CatalogueResponse catalogueResponse = new CompanyGroup.WebClient.Models.CatalogueResponse(products,
                                                                                                                                        new CompanyGroup.WebClient.Models.Visitor(),
                                                                                                                                        new CompanyGroup.Dto.WebshopModule.ShoppingCart(),
                                                                                                                                        new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(),
                                                                                                                                        new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(),
                                                                                                                                        false,
                                                                                                                                        false,
                                                                                                                                        new CompanyGroup.Dto.PartnerModule.DeliveryAddresses(),
                                                                                                                                        new CompanyGroup.Dto.WebshopModule.LeasingOptions());

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.CatalogueResponse>(HttpStatusCode.OK, catalogueResponse);

                return httpResponseMessage;

            }
            catch (Exception ex)
            {
                CompanyGroup.WebClient.Models.CatalogueResponse catalogueResponse = new CompanyGroup.WebClient.Models.CatalogueResponse(new CompanyGroup.Dto.WebshopModule.Products(),
                                                                                                                                        new CompanyGroup.WebClient.Models.Visitor(){ ErrorMessage = String.Format("A kijelentkezés nem sikerült! ({0})", ex.Message) },
                                                                                                                                        new CompanyGroup.Dto.WebshopModule.ShoppingCart(),
                                                                                                                                        new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(),
                                                                                                                                        new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(),
                                                                                                                                        false,
                                                                                                                                        false,
                                                                                                                                        new CompanyGroup.Dto.PartnerModule.DeliveryAddresses(),
                                                                                                                                        new CompanyGroup.Dto.WebshopModule.LeasingOptions());

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.ApiMessage>(HttpStatusCode.InternalServerError, new CompanyGroup.WebClient.Models.ApiMessage(String.Format("A kijelentkezés nem sikerült! ({0} - {1})", ex.Message, ex.StackTrace)));

                throw new HttpResponseException(httpResponseMessage);
            }
        }

        //// GET api/webshop/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/webshop
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/webshop/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/webshop/5
        //public void Delete(int id)
        //{
        //}
    }
}
