using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// cshtml view-k betöltését végzi
    /// </summary>
    public class HomeController : System.Web.Mvc.Controller
    {
        protected readonly static string CookieName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("CookieName", "companygroup_hrpbsc");

        private CompanyGroup.WebClient.Models.Visitor GetVisitor(CompanyGroup.WebClient.Models.VisitorData visitorData)
        {
            CompanyGroup.WebClient.Models.Visitor visitor;

            if (String.IsNullOrWhiteSpace(visitorData.ObjectId))
            {
                visitor = new CompanyGroup.WebClient.Models.Visitor();
            }
            else
            {
                CompanyGroup.Dto.ServiceRequest.VisitorInfo request = new CompanyGroup.Dto.ServiceRequest.VisitorInfo() { ObjectId = visitorData.ObjectId, DataAreaId = HomeController.DataAreaId };

                CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.VisitorInfo, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "GetVisitorInfo", request);

                visitor = new CompanyGroup.WebClient.Models.Visitor(response);
            }

            return visitor;
        }

        /// <summary>
        /// Webshop view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Webshop view";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

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
                CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses getDeliveryAddresses = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses() { DataAreaId = HomeController.DataAreaId, VisitorId = visitor.Id };

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

                string json = CompanyGroup.Helpers.JsonConverter.ToJSON<CompanyGroup.WebClient.Models.VisitorData>(visitorData);

                CompanyGroup.Helpers.CookieHelper.WriteCookie(this.Response, CookieName, json);
            }

            return View(catalogue);
        }

        /// <summary>
        /// Webshop details view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Webshop(string productId)
        {
            ViewBag.Message = "Details webshop view";

            return View("WebshopDetails");
        }

        /// <summary>
        /// Newsletter view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletter()
        {
            ViewBag.Message = "Newsletter view.";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, HomeController.CookieName);

            if (visitorData == null)
            {
                visitorData = new CompanyGroup.WebClient.Models.VisitorData();
            }

            CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection request = new CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection()
            {
                Language = visitorData.Language,
                VisitorId = visitorData.ObjectId,
                ManufacturerId = String.Empty
            };

            CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetCollection", request);

            CompanyGroup.WebClient.Models.NewsletterCollection model = new CompanyGroup.WebClient.Models.NewsletterCollection(response);

            return View(model);
        }

        /// <summary>
        /// PartnerInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult PartnerInfo()
        {
            ViewBag.Message = "PartnerInfo view.";

            return View();
        }

        /// <summary>
        /// InvoiceInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoiceInfo()
        {
            ViewBag.Message = "InvoiceInfo view.";

            return View();
        }

        /// <summary>
        /// OrderInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderInfo()
        {
            ViewBag.Message = "OrderInfo view.";

            return View();
        }

        /// <summary>
        /// Registration view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Registration()
        {
            ViewBag.Message = "Registration view.";

            return View();
        }

        /// <summary>
        /// ChangePassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "ChangePassword view.";

            return View();
        }

        /// <summary>
        /// UndoChangePassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult UndoChangePassword()
        {
            ViewBag.Message = "UndoChangePassword view.";

            return View();
        }

        /// <summary>
        /// ForgetPassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPassword()
        {
            ViewBag.Message = "ForgetPassword view.";

            return View();
        }

        /// <summary>
        /// Carreer view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Carreer()
        {
            ViewBag.Message = "Carreer view.";

            return View();
        }

        #region "PostJSonData"

        private readonly static string ServiceBaseAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceBaseAddress", "http://1Juhasza/CompanyGroup.WebApi/api/");

        protected readonly static string DataAreaId = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DataAreaId", "hrp");

        /// <summary>
        /// post json 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private TResponse PostJSonData<TRequest, TResponse>(string controllerName, string actionName, TRequest request) where TRequest : new() where TResponse : new()
        {

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "Controller name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "Action name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Request can not be null!");

            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(HomeController.ServiceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                Uri requestUri = null;

                System.Net.Http.HttpResponseMessage response = client.PostAsJsonAsync(String.Format("{0}/{1}", controllerName, actionName), request).Result;

                if (response.IsSuccessStatusCode)
                {
                    requestUri = response.Headers.Location;
                }
                else
                {
                    String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                TResponse content = response.Content.ReadAsAsync<TResponse>().Result;

                return content;
            }
            catch { return default(TResponse); }
        }

        #endregion
    }
}
