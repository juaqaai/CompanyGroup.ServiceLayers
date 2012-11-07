using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// base controller
    /// - PostJSonData
    /// - GetJSonData
    /// - GetVisitorInfo
    /// - ReadObjectIdFromCookie
    /// - WriteObjectIdToCookie
    /// </summary>
    public class ApiBaseController : ApiController
    {
        private readonly static string ServiceBaseAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceBaseAddress", "http://1Juhasza/CompanyGroup.WebApi/api/");

        protected readonly static string DataAreaId = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DataAreaId", "hrp");

        protected readonly static string CookieName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("CookieName", "companygroup_hrpbsc");

        protected readonly static string LanguageEnglish = CompanyGroup.Helpers.ConfigSettingsParser.GetString("LanguageEnglish", "en");

        protected readonly static string LanguageHungarian = CompanyGroup.Helpers.ConfigSettingsParser.GetString("LanguageHungarian", "hu");

        /// <summary>
        /// alapértelmezett valutanem, ha nincs beállítva semmi, akkor ebben a valutanemben lesznek értelmezve az árak
        /// </summary>
        protected readonly static string DefaultCurrency = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DefaultCurrency", "HUF");

        protected readonly static string MongoDbEmptyObjectId = CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDbEmptyObjectId", "000000000000000000000000");

        #region "PostJSonData, GetJSonData, DownloadData"

        /// <summary>
        /// post json 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected TResponse PostJSonData<TRequest, TResponse>(string controllerName, string actionName, TRequest request) where TRequest : new() where TResponse : new()
        {

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "Controller name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "Action name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Request can not be null!");

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(ApiBaseController.ServiceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri requestUri = null;

                HttpResponseMessage response = client.PostAsJsonAsync(String.Format("{0}/{1}", controllerName, actionName), request).Result;

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

        /// <summary>
        /// retriewe json data from an application service url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        protected T GetJSonData<T>(string requestUrl) where T : new()
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(requestUrl), "RequestUrl name can not be null or empty!");

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(ApiBaseController.ServiceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                T result = response.Content.ReadAsAsync<T>().Result;

                return result;
            }
            catch { return default(T); }
        }

        /// <summary>
        /// retriewe raw byte array data from an application service url
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected byte[] DownloadData(string requestUrl)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(requestUrl), "RequestUrl can not be null or empty!");

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(ApiBaseController.ServiceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                byte[] result = response.Content.ReadAsByteArrayAsync().Result;

                return result;
            }
            catch
            {
                return new byte[] { };
            }
        }

        #endregion

        #region "SignIn( CompanyGroup.WebClient.SignIn request), SignOut( CompanyGroup.WebClient.SignOut request), VisitorInfo(), GetVisitorInfo()"

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.Visitor SignIn(CompanyGroup.WebClient.Models.SignInRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "SignIn request can not be null!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Password), "A jelszó megadása kötelező!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.UserName), "A belépési név megadása kötelező!");

                //előző belépés azonosítójának mentése
                string permanentObjectId = this.ReadPermanentIdFromCookie();

                CompanyGroup.Dto.ServiceRequest.SignIn signIn = new CompanyGroup.Dto.ServiceRequest.SignIn(ApiBaseController.DataAreaId,
                                                                                                           request.UserName, 
                                                                                                           request.Password, 
                                                                                                           System.Web.HttpContext.Current.Request.UserHostAddress);


                CompanyGroup.Dto.PartnerModule.Visitor signInResponse = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignIn, CompanyGroup.Dto.PartnerModule.Visitor>("Customer", "SignIn", signIn);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor(signInResponse);

                //check status
                if (!visitor.LoggedIn)
                {
                    visitor.ErrorMessage = "A bejelentkezés nem sikerült!";
                }
                else    //SignIn process, set http cookie, etc...
                {
                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.Id), "A bejelentkezés nem sikerült! (üres azonosító)");

                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.CompanyId), "A bejelentkezés nem sikerült! (üres cégazonosító)");

                    //kosár társítása
                    CompanyGroup.Dto.ServiceRequest.AssociateCart associateRequest = new CompanyGroup.Dto.ServiceRequest.AssociateCart(visitor.Id, permanentObjectId) { Language = this.ReadLanguageFromCookie() };

                    CompanyGroup.Dto.WebshopModule.ShoppingCartInfo associateCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AssociateCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AssociateCart", associateRequest);

                    //visitor adatok http sütibe írása     
                    this.WriteCookie(new CompanyGroup.WebClient.Models.VisitorData(visitor.Id, visitor.LanguageId, false, false, visitor.Currency, visitor.Id, associateCart.ActiveCart.Id, ""));

                    visitor.ErrorMessage = String.Empty;

                }
                return visitor;
            }
            catch (CompanyGroup.Helpers.DesignByContractException ex)
            {
                return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = ex.Message };
            }
            catch
            {
                return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = "A bejelentkezés nem sikerült! (hiba)" };
            }
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.Visitor SignOut(CompanyGroup.WebClient.Models.SignOutRequest request)
        {
            try
            {
                CompanyGroup.Dto.ServiceResponse.Empty empty = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignOut, CompanyGroup.Dto.ServiceResponse.Empty>("Customer", "SignOut", new CompanyGroup.Dto.ServiceRequest.SignOut() { DataAreaId = ApiBaseController.DataAreaId, ObjectId = this.ReadObjectIdFromCookie() });

                this.RemoveObjectIdFromCookie();

                //this.RemoveCurrencyFromCookie();

                return new CompanyGroup.WebClient.Models.Visitor();
            }
            catch (Exception ex)
            {
                return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// látogató adatainak kiolvasása
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("VisitorInfo")]
        public CompanyGroup.WebClient.Models.Visitor VisitorInfo()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return visitor;
        }

        /// <summary>
        /// látogató adatainak kiolvasása szerviz hívással  
        /// </summary>
        /// <returns></returns>
        protected CompanyGroup.WebClient.Models.Visitor GetVisitor(CompanyGroup.WebClient.Models.VisitorData visitorData)
        {
            try
            {

                if (String.IsNullOrWhiteSpace(visitorData.ObjectId))
                {
                    return new CompanyGroup.WebClient.Models.Visitor();
                }

                CompanyGroup.Dto.ServiceRequest.VisitorInfo request = new CompanyGroup.Dto.ServiceRequest.VisitorInfo() { ObjectId = visitorData.ObjectId, DataAreaId = ApiBaseController.DataAreaId };

                CompanyGroup.Dto.PartnerModule.Visitor response  = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.VisitorInfo, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "GetVisitorInfo", request);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor(response);

                return visitor;
            }
            catch (Exception ex) { return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = ex.Message }; }
        }

        /// <summary>
        /// beállítja a http süti Currency mező értékét.
        /// /// </summary>
        /// <returns></returns>
        [ActionName("ChangeCurrency")]
        [HttpPost]
        public void ChangeCurrency(string currency)
        {
            try
            {
                currency = String.IsNullOrEmpty(currency) ? ApiBaseController.DefaultCurrency : currency;

                this.WriteCurrencyToCookie(currency);
            }
            catch 
            { 
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)); 
            }
        }

        /// <summary>
        /// beállítja a http süti Language mező értékét.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [ActionName("ChangeLanguage")]
        [HttpPost]
        public void ChangeLanguage(string language)
        {
            try
            {
                language = String.IsNullOrEmpty(language) ? ApiBaseController.LanguageHungarian : language;

                this.WriteLanguageToCookie(language);
            }
            catch 
            { 
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)); 
            }
        }

        #endregion

        #region "private cookie funkciók - ReadCookie, WriteCookie"

        /// <summary>
        /// visitor adatok kiolvasása http cookie-ból (string -> Json conversion)
        /// </summary>
        /// <returns></returns>
        protected CompanyGroup.WebClient.Models.VisitorData ReadCookie()
        {
            try
            {
                //System.Web.HttpContext.Current.Request.Cookies

                CookieHeaderValue cookieHeaderValue = this.Request.Headers.GetCookies(ApiBaseController.CookieName).FirstOrDefault();

                if (cookieHeaderValue == null) { return new CompanyGroup.WebClient.Models.VisitorData(); }

                return CompanyGroup.Helpers.JsonConverter.FromJSON<CompanyGroup.WebClient.Models.VisitorData>(cookieHeaderValue[ApiBaseController.CookieName].Value);
            }
            catch { return new CompanyGroup.WebClient.Models.VisitorData(); }
        }

        /// <summary>
        /// visitor adatok mentése http cookie-ba
        ///     var resp = new HttpResponseMessage();

    //var cookie = new CookieHeaderValue("session-id", "12345");
    //cookie.Expires = DateTimeOffset.Now.AddDays(1);
    //cookie.Domain = Request.RequestUri.Host;
    //cookie.Path = "/";

    //resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });

        /// </summary>
        /// <param name="visitorData"></param>
        protected void WriteCookie(CompanyGroup.WebClient.Models.VisitorData visitorData)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((visitorData != null), "visitorData object can not be null or empty!");

                //konverzió json string-be
                string json = CompanyGroup.Helpers.JsonConverter.ToJSON<CompanyGroup.WebClient.Models.VisitorData>(visitorData);

                CookieHeaderValue cookieHeaderValue = this.Request.Headers.GetCookies(ApiBaseController.CookieName).FirstOrDefault();

                if (cookieHeaderValue != null)
                {
                    //süti törlése
                    System.Web.HttpContext.Current.Response.Cookies.Remove(ApiBaseController.CookieName);
                }

                CookieHeaderValue newCookieHeaderValue = new CookieHeaderValue(ApiBaseController.CookieName, json) { Expires = DateTime.Now.AddDays(30d), HttpOnly = false };

                HttpResponseMessage response = new HttpResponseMessage();                

                response.Headers.AddCookies(new CookieHeaderValue[] { newCookieHeaderValue });

            }
            catch { }
        }

        #endregion

        #region "ObjectId cookie functions (ReadObjectIdFromCookie(), WriteObjectIdToCookie(string objectId), RemoveObjectIdFromCookie())"

        /// <summary>
        /// read objectId string from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadObjectIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.ObjectId;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the objectId string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteObjectIdToCookie(string objectId)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(objectId), "ObjectId can not be null or empty!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.ObjectId = objectId;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// delete objectId value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        protected void RemoveObjectIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.ObjectId = String.Empty;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "PermanentId cookie functions (ReadPermanentIdFromCookie(), WritePermanentIdToCookie(string permanentId))"

        /// <summary>
        /// read PermanentId value from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadPermanentIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.PermanentId;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the PermanentId value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WritePermanentIdToCookie(string permanentId)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.PermanentId = permanentId;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "CartId cookie functions (ReadCartIdFromCookie(), WriteCartIdToCookie(string cartId))"

        /// <summary>
        /// read PermanentId value from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadCartIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.CartId;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the PermanentId value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteCartIdToCookie(string cartId)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.CartId = cartId;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "Language cookie functions (ReadLanguageFromCookie(), WriteLanguageToCookie(string language), RemoveLanguageFromCookie())"

        /// <summary>
        /// read language string from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadLanguageFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.Language;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the language string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteLanguageToCookie(string language)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(language), "Language can not be null or empty!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Language = language;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// delete language value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        protected void RemoveLanguageFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Language = String.Empty;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "RegistrationId cookie functions (ReadRegistrationIdFromCookie(), WriteRegistrationIdToCookie(string registrationId))"

        /// <summary>
        /// read RegistrationId string from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadRegistrationIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.RegistrationId;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the RegistrationId string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteRegistrationIdToCookie(string registrationId)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(registrationId), "Language can not be null or empty!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.RegistrationId = registrationId;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// regisztrációs azonosító eltávolítása a sütiből
        /// </summary>
        protected void RemoveRegistrationIdFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.RegistrationId = String.Empty;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "Currency cookie functions (ReadCurrencyFromCookie(), WriteCurrencyToCookie(string language), RemoveCurrencyFromCookie())"

        /// <summary>
        /// read Currency string from http cookie
        /// </summary>
        /// <returns></returns>
        protected string ReadCurrencyFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.Currency;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// write the Currency string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteCurrencyToCookie(string currency)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(currency), "Currency can not be null or empty!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Currency = currency;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// delete currency value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        //protected void RemoveCurrencyFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.Currency = String.Empty;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "IsShoppingCartOpened cookie functions (ReadShoppingCartOpenedFromCookie(), WriteShoppingCartOpenedToCookie(bool isShoppingCartOpened), RemoveShoppingCartOpenedFromCookie())"

        /// <summary>
        /// read IsCartOpened value from http cookie
        /// </summary>
        /// <returns></returns>
        protected bool ReadShoppingCartOpenedFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.IsShoppingCartOpened;
            }
            catch { return false; }
        }

        /// <summary>
        /// write the IsShoppingCartOpened value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteShoppingCartOpenedToCookie(bool isShoppingCartOpened)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.IsShoppingCartOpened = isShoppingCartOpened;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// delete isShoppingCartOpened value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        protected void RemoveShoppingCartOpenedFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.IsShoppingCartOpened = false;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "CatalogueOpened cookie functions (ReadCatalogueOpenedFromCookie(), void WriteCatalogueOpenedToCookie(bool isCatalogueOpened), RemoveCatalogueOpenedFromCookie())"

        /// <summary>
        /// read IsCatalogueOpened value from http cookie
        /// </summary>
        /// <returns></returns>
        protected bool ReadCatalogueOpenedFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                return visitorData.IsCatalogueOpened;
            }
            catch { return false; }
        }

        /// <summary>
        /// write the IsCatalogueOpened value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        protected void WriteCatalogueOpenedToCookie(bool isCatalogueOpened)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.IsCatalogueOpened = isCatalogueOpened;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        /// <summary>
        /// delete IsCatalogueOpened value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        protected void RemoveCatalogueOpenedFromCookie()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.IsCatalogueOpened = false;

                this.WriteCookie(visitorData);
            }
            catch { }
        }

        #endregion

        #region "ShoppingCart"

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.StoredShoppingCartCollection GetStoredShoppingCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request)
        /// </summary>
        /// <returns></returns>
        //public CompanyGroup.WebClient.Response.StoredOpenedShoppingCartCollection GetStoredOpenedShoppingCartCollectionByVisitor()
        //{
        //    try
        //    {
        //        CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request = new CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor(this.ReadLanguageFromCookie(), this.ReadObjectIdFromCookie());

        //        CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection>("ShoppingCartService", "GetStoredOpenedShoppingCartCollectionByVisitor", request);

        //        return new CompanyGroup.WebClient.Response.StoredOpenedShoppingCartCollection(response.StoredItems, response.OpenedItems);
        //    }
        //    catch (Exception ex) { return new CompanyGroup.WebClient.Response.StoredOpenedShoppingCartCollection() { ErrorMessage = ex.Message }; }            
        //}

        /// <summary>
        /// bevásárlókosár kiolvasása       
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public CompanyGroup.WebClient.Models.ShoppingCartInfo GetCartInfo()
        {
            CompanyGroup.Dto.ServiceRequest.GetActiveCart request = new CompanyGroup.Dto.ServiceRequest.GetActiveCart(this.ReadLanguageFromCookie(), this.ReadObjectIdFromCookie());

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetActiveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetActiveCart", request);

            if (response == null)
            {
                return new CompanyGroup.WebClient.Models.ShoppingCartInfo();
            }

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo() 
                       { 
                           ActiveCart = response.ActiveCart, 
                           OpenedItems = response.OpenedItems, 
                           StoredItems = response.StoredItems, 
                           ErrorMessage = "", 
                           FinanceOffer = new Dto.WebshopModule.FinanceOffer(), 
                           LeasingOptions = new Dto.WebshopModule.LeasingOptions() 
                       };
        }

        #endregion
    }
}
