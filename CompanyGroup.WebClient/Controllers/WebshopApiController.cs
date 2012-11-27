using System;
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
        [ActionName("GetCatalogue")]
        public CompanyGroup.WebClient.Models.Catalogue GetCatalogue()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //struktúrák lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetAllStructure allStructure = new CompanyGroup.Dto.ServiceRequest.GetAllStructure()
            {
                ActionFilter = false,
                BargainFilter = false,
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
                PriceFilterRelation = "0",
                NameOrPartNumberFilter = ""
            };

            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllStructure, CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetAll", allStructure);

            //katalógus lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetAllProduct allProduct = new CompanyGroup.Dto.ServiceRequest.GetAllProduct()
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
                VisitorId = visitor.Id,
                NameOrPartNumberFilter = ""
            };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", allProduct);

            //banner lista lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetBannerList bannerListRequest = new CompanyGroup.Dto.ServiceRequest.GetBannerList()
            {
                BscFilter = true,
                HrpFilter = true,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetBannerList, CompanyGroup.Dto.WebshopModule.BannerList>("Product", "GetBannerList", bannerListRequest);

            //kosár lekérdezések     
            bool shoppingCartOpenStatus = visitorData.IsShoppingCartOpened;

            bool catalogueOpenStatus = visitorData.IsCatalogueOpened;

            CompanyGroup.WebClient.Models.ShoppingCartInfo cartInfo = new CompanyGroup.WebClient.Models.ShoppingCartInfo();  //(visitor.IsValidLogin) ? this.GetCartInfo() : 

            if (visitor.IsValidLogin && !String.IsNullOrEmpty(visitorData.CartId))
            {
                CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo shoppingCartInfoRequest = new CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo(visitorData.CartId, visitor.Id);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetShoppingCartInfo", shoppingCartInfoRequest);

                cartInfo.ActiveCart = (shoppingCartInfo != null) ? shoppingCartInfo.ActiveCart : cartInfo.ActiveCart;
                cartInfo.OpenedItems = (shoppingCartInfo != null) ? shoppingCartInfo.OpenedItems : cartInfo.OpenedItems;
                cartInfo.StoredItems = (shoppingCartInfo != null) ? shoppingCartInfo.StoredItems : cartInfo.StoredItems;
                cartInfo.LeasingOptions = (shoppingCartInfo != null) ? shoppingCartInfo.LeasingOptions : cartInfo.LeasingOptions;
            }

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses getDeliveryAddresses = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses() { DataAreaId = ApiBaseController.DataAreaId, VisitorId = visitor.Id };

                deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", getDeliveryAddresses);
            }
            else
            {
                deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses() { Items = new List<CompanyGroup.Dto.PartnerModule.DeliveryAddress>() };
            }

            CompanyGroup.WebClient.Models.Catalogue catalogue = new CompanyGroup.WebClient.Models.Catalogue(structures, products, visitor, cartInfo.ActiveCart, cartInfo.OpenedItems, cartInfo.StoredItems, shoppingCartOpenStatus, catalogueOpenStatus, deliveryAddresses, bannerList, cartInfo.LeasingOptions);

            //aktív kosár azonosítójának mentése http cookie-ba
            if (!String.IsNullOrWhiteSpace(cartInfo.ActiveCart.Id))
            {
                visitorData.CartId = cartInfo.ActiveCart.Id;

                this.WriteCookie(visitorData);
            }

            return catalogue;
        }

        /// <summary>
        /// terméklista lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns>terméklista objektum és a látogató objektum JSON formátumban</returns>
        [HttpPost]
        [ActionName("GetProducts")]
        public CompanyGroup.WebClient.Models.ProductCatalogue GetProducts(CompanyGroup.Dto.ServiceRequest.GetAllProduct request) //CompanyGroup.Dto.ServiceRequest.CatalogueFilter
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            request.VisitorId = visitor.Id;   //this.ReadObjectIdFromCookie();

            request.Currency = visitorData.Currency;    // this.ReadCurrencyFromCookie();

            request.NameOrPartNumberFilter = request.NameOrPartNumberFilter ?? String.Empty;

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", request);

            return new CompanyGroup.WebClient.Models.ProductCatalogue(products, visitor);
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

            CompanyGroup.Dto.ServiceRequest.GetItemByProductId request = new CompanyGroup.Dto.ServiceRequest.GetItemByProductId()
            {
                ProductId = productId,
                DataAreaId = ApiBaseController.DataAreaId,
                VisitorId = visitorData.ObjectId,
                Currency = visitorData.Currency
            };

            CompanyGroup.Dto.WebshopModule.Product product = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetItemByProductId, CompanyGroup.Dto.WebshopModule.Product>("Product", "GetItemByProductId", request);

            CompanyGroup.Dto.WebshopModule.CompatibleProducts compatibleProducts = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetItemByProductId, CompanyGroup.Dto.WebshopModule.CompatibleProducts>("Product", "GetCompatibleProducts", request);

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

            CompanyGroup.Dto.WebshopModule.CompletionList response = this.GetJSonData<CompanyGroup.Dto.WebshopModule.CompletionList>(String.Format("Product/GetCompletionList/{0}/{1}/{2}", WebshopApiController.DataAreaId, prefix, 2));

            return new CompanyGroup.WebClient.Models.CompletionList(response);
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

            CompanyGroup.Dto.WebshopModule.CompletionList response = this.GetJSonData<CompanyGroup.Dto.WebshopModule.CompletionList>(String.Format("Product/GetCompletionList/{0}/{1}/{2}", WebshopApiController.DataAreaId, prefix, 1));

            return new CompanyGroup.WebClient.Models.CompletionList(response);
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

                CompanyGroup.Dto.ServiceRequest.SignIn singnInRequest = new CompanyGroup.Dto.ServiceRequest.SignIn(ApiBaseController.DataAreaId,
                                                                                                        request.UserName,
                                                                                                        request.Password,
                                                                                                        System.Web.HttpContext.Current.Request.UserHostAddress);

                CompanyGroup.Dto.PartnerModule.Visitor signInResponse = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignIn, CompanyGroup.Dto.PartnerModule.Visitor>("Customer", "SignIn", singnInRequest);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor(signInResponse);

                //válaszüzenet összeállítása
                CompanyGroup.WebClient.Models.ShoppingCartInfo cartInfo;

                CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

                bool shoppingCartOpenStatus = visitorData.IsShoppingCartOpened;

                bool catalogueOpenStatus = visitorData.IsCatalogueOpened;

                HttpStatusCode httpStatusCode = HttpStatusCode.OK;

                CompanyGroup.WebClient.Models.CatalogueResponse catalogueResponse;

                CompanyGroup.Dto.WebshopModule.Products products;

                //nem sikerült a belépés 
                if (!visitor.LoggedIn)
                {
                    visitor.ErrorMessage = "A bejelentkezés nem sikerült!";

                    cartInfo = new CompanyGroup.WebClient.Models.ShoppingCartInfo()
                    {
                        ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart(),
                        OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(),
                        StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(),
                        ErrorMessage = "",
                        LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions()
                    };

                    deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();

                    products = new CompanyGroup.Dto.WebshopModule.Products();
                }
                else    //sikerült a belépés, http cookie beállítás, ...
                {
                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.Id), "A bejelentkezés nem sikerült! (üres azonosító)");

                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.CompanyId), "A bejelentkezés nem sikerült! (üres cégazonosító)");

                    //kosár társítása
                    CompanyGroup.Dto.ServiceRequest.AssociateCart associateRequest = new CompanyGroup.Dto.ServiceRequest.AssociateCart(visitor.Id, permanentObjectId) { Language = visitorData.Language };

                    CompanyGroup.Dto.WebshopModule.ShoppingCartInfo associateCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AssociateCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AssociateCart", associateRequest);

                    //aktív kosár beállítás
                    cartInfo = new CompanyGroup.WebClient.Models.ShoppingCartInfo()
                    {
                        ActiveCart = associateCart.ActiveCart,
                        OpenedItems = associateCart.OpenedItems,
                        StoredItems = associateCart.StoredItems,
                        LeasingOptions = associateCart.LeasingOptions,
                        ErrorMessage = ""
                    };

                    //szállítási címek lekérdezése
                    CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses deliveryAddressRequest = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses(WebshopApiController.DataAreaId, visitor.Id);

                    deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", deliveryAddressRequest);

                    //visitor adatok http sütibe írása     
                    this.WriteCookie(new CompanyGroup.WebClient.Models.VisitorData(visitor.Id, visitor.LanguageId, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, visitor.Currency, visitor.Id, associateCart.ActiveCart.Id, visitorData.RegistrationId));

                    visitor.ErrorMessage = String.Empty;

                    //katalógus lekérdezése
                    CompanyGroup.Dto.ServiceRequest.GetAllProduct allProduct = new CompanyGroup.Dto.ServiceRequest.GetAllProduct()
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
                        VisitorId = visitor.Id,
                        NameOrPartNumberFilter = ""
                    };

                    products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", allProduct);

                    httpStatusCode = HttpStatusCode.Created;
                }

                catalogueResponse = new CompanyGroup.WebClient.Models.CatalogueResponse(products,
                                                                                        visitor,
                                                                                        cartInfo.ActiveCart,
                                                                                        cartInfo.OpenedItems,
                                                                                        cartInfo.StoredItems,
                                                                                        shoppingCartOpenStatus,
                                                                                        catalogueOpenStatus,
                                                                                        deliveryAddresses,
                                                                                        cartInfo.LeasingOptions);

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.CatalogueResponse>(httpStatusCode, catalogueResponse);

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

        [HttpPost]
        [ActionName("SignOut")]
        public HttpResponseMessage SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                CompanyGroup.Dto.ServiceRequest.SignOut req = new CompanyGroup.Dto.ServiceRequest.SignOut() { DataAreaId = ApiBaseController.DataAreaId, ObjectId = visitorData.ObjectId };

                CompanyGroup.Dto.ServiceResponse.Empty empty = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignOut, CompanyGroup.Dto.ServiceResponse.Empty>("Customer", "SignOut", req);

                visitorData.ObjectId = String.Empty;

                this.WriteCookie(visitorData);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor();

                //katalógus lekérdezése
                CompanyGroup.Dto.ServiceRequest.GetAllProduct allProduct = new CompanyGroup.Dto.ServiceRequest.GetAllProduct()
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
                    VisitorId = visitor.Id,
                    NameOrPartNumberFilter = ""
                };

                CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", allProduct);

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
