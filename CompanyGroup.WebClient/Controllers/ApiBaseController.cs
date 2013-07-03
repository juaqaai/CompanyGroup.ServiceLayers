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
    /// api base controller
    /// - PostJSonData
    /// - GetJSonData
    /// - GetVisitorInfo
    /// - ReadCookie
    /// - WriteCookie
    /// </summary>
    public class ApiBaseController : ApiController
    {
        private readonly static string ServiceBaseAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceBaseAddress", "http://1Juhasza/CompanyGroup.WebApi/api/");

        private readonly static string ServiceReadOnlyBaseAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceReadOnlyBaseAddress", "http://srv3/CompanyGroup.WebApi/api/");

        protected readonly static string DataAreaId = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DataAreaId", "hrp");

        protected readonly static string CookieName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("CookieName", "companygroup_hrpbsc");

        protected readonly static string LanguageEnglish = CompanyGroup.Helpers.ConfigSettingsParser.GetString("LanguageEnglish", "EN");

        protected readonly static string LanguageHungarian = CompanyGroup.Helpers.ConfigSettingsParser.GetString("LanguageHungarian", "HU");

        /// <summary>
        /// alapértelmezett valutanem, ha nincs beállítva semmi, akkor ebben a valutanemben lesznek értelmezve az árak
        /// </summary>
        protected readonly static string DefaultCurrency = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DefaultCurrency", "HUF");

        #region "Http exception"

        /// <summary>
        /// biztonságos kivétel dobása
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        protected void ThrowSafeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            HttpResponseMessage response = Request.CreateResponse<CompanyGroup.Dto.SharedModule.ApiMessage>(statusCode, new CompanyGroup.Dto.SharedModule.ApiMessage(message));

            throw new HttpResponseException(response);
        }

        /// <summary>
        /// http kivétel visszaadása
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage ThrowHttpError(Exception ex)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
        }

        /// <summary>
        /// http kivétel visszaadása
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage ThrowHttpError(string message, string source)
        {
            ApplicationException ex = new ApplicationException(message);

            ex.Source = source;

            return ThrowHttpError(ex);
        }

        #endregion

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

        protected HttpResponseMessage PostJSonData<TRequest>(string controllerName, string actionName, TRequest request) where TRequest : new()
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "Controller name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "Action name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Request can not be null!");

            try
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(ApiBaseController.ServiceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(String.Format("{0}/{1}", controllerName, actionName), request).Result;

                return response;
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// GET json data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected T GetJSonData<T>(string controllerName, string actionName, string parameters) where T : new()
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "ControllerName can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "ActionName can not be null or empty!");

            if (!String.IsNullOrWhiteSpace(parameters)) { parameters = String.Format("/{0}", parameters); }

            return GetJSonData<T>(String.Format("{0}/{1}{2}", controllerName, actionName, parameters));
        }

        /// <summary>
        /// GET json data 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        protected T GetJSonData<T>(string requestUrl) where T : new()
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(requestUrl), "RequestUrl can not be null or empty!");

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

        protected HttpResponseMessage GetJSonData(string controllerName, string actionName, string parameter)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "ControllerName can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "ActionName can not be null or empty!");

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(ApiBaseController.ServiceBaseAddress);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string requestUrl = String.Format("{0}/{1}", controllerName, actionName);

            if (!String.IsNullOrWhiteSpace(parameter)) { requestUrl = String.Format("{0}/{1}", requestUrl, parameter); }

            HttpResponseMessage response = client.GetAsync(requestUrl).Result;

            return response;
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

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));
    
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpg"));
           
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));

                //HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                byte[] result = client.GetByteArrayAsync(requestUrl).Result;    //response.Content.ReadAsByteArrayAsync().Result;

                return result;
            }
            catch
            {
                return new byte[] { };
            }
        }

        #endregion

        /// <summary>
        /// látogató adatainak kiolvasása szerviz hívással  
        /// </summary>
        /// <returns></returns>
        protected CompanyGroup.WebClient.Models.Visitor GetVisitor(CompanyGroup.WebClient.Models.VisitorData visitorData)
        {
            try
            {
                //üres a látogató azonosító
                if (String.IsNullOrWhiteSpace(visitorData.VisitorId))
                {
                    return new CompanyGroup.WebClient.Models.Visitor();
                }

                CompanyGroup.Dto.PartnerModule.VisitorInfoRequest request = new CompanyGroup.Dto.PartnerModule.VisitorInfoRequest(visitorData.VisitorId, ApiBaseController.DataAreaId);

                HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.VisitorInfoRequest>("Visitor", "GetVisitorInfo", request);

                if (response.IsSuccessStatusCode)
                {
                    CompanyGroup.Dto.PartnerModule.Visitor visitor = response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.Visitor>().Result;

                    CompanyGroup.WebClient.Models.Visitor visitorViewModel = new CompanyGroup.WebClient.Models.Visitor(visitor);

                    return visitorViewModel;
                }
                else
                {
                    return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = String.Format("{0}/{1}", response.StatusCode, response.ReasonPhrase) };
                }
            }
            catch (Exception ex) { return new CompanyGroup.WebClient.Models.Visitor() { ErrorMessage = ex.Message }; }
        }

        public static bool ListContainsAnotherListItem<T>(IList<T> list1, IList<T> list2, bool displayIfEmpty)
        {
            if (list1.Count.Equals(0) && displayIfEmpty)
            {
                return true;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i].Equals(list2[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #region "cookie funkciók - ReadCookie, WriteCookie"

        /// <summary>
        /// visitor adatok kiolvasása http cookie-ból (string -> Json conversion)
        /// </summary>
        /// <returns></returns>
        protected CompanyGroup.WebClient.Models.VisitorData ReadCookie()
        {
            try
            {
                //CookieHeaderValue cookieHeaderValue = this.Request.Headers.GetCookies(ApiBaseController.CookieName).FirstOrDefault();

                //if (cookieHeaderValue == null) { return new CompanyGroup.WebClient.Models.VisitorData(); }

                //CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.JsonConverter.FromJSON<CompanyGroup.WebClient.Models.VisitorData>(cookieHeaderValue[ApiBaseController.CookieName].Value);

                CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, ApiBaseController.CookieName);

                if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

                return visitorData;
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
            CompanyGroup.Helpers.DesignByContract.Require((visitorData != null), "visitorData object can not be null or empty!");

            try
            {
                #region "CookieHeaderValue"
                ////konverzió json string-be
                //string json = CompanyGroup.Helpers.JsonConverter.ToJSON<CompanyGroup.WebClient.Models.VisitorData>(visitorData);

                //CookieHeaderValue cookieHeaderValue = this.Request.Headers.GetCookies(ApiBaseController.CookieName).FirstOrDefault();

                //if (cookieHeaderValue != null)
                //{
                //    //süti törlése
                //    System.Web.HttpContext.Current.Response.Cookies.Remove(ApiBaseController.CookieName);
                //}

                //CookieHeaderValue newCookieHeaderValue = new CookieHeaderValue(ApiBaseController.CookieName, json);
                
                //newCookieHeaderValue.Expires = DateTime.Now.AddDays(30d);
                
                //newCookieHeaderValue.HttpOnly = false;

                //newCookieHeaderValue.Domain = Request.RequestUri.Host;

                //newCookieHeaderValue.Path = "/";

                //HttpResponseMessage response = new HttpResponseMessage();                

                //response.Headers.AddCookies(new CookieHeaderValue[] { newCookieHeaderValue });
                #endregion

                CompanyGroup.Helpers.CookieHelper.WriteCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Response, ApiBaseController.CookieName, visitorData);

            }
            catch { }
        }

        #endregion

        #region "ObjectId cookie functions (ReadObjectIdFromCookie(), WriteObjectIdToCookie(string objectId), RemoveObjectIdFromCookie())"

        /// <summary>
        /// read objectId string from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadObjectIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.ObjectId;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the objectId string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteObjectIdToCookie(string objectId)
        //{
        //    try
        //    {
        //        CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(objectId), "ObjectId can not be null or empty!");

        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.ObjectId = objectId;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// delete objectId value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        //protected void RemoveObjectIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.ObjectId = String.Empty;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "PermanentId cookie functions (ReadPermanentIdFromCookie(), WritePermanentIdToCookie(string permanentId))"

        /// <summary>
        /// read PermanentId value from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadPermanentIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.PermanentId;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the PermanentId value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WritePermanentIdToCookie(string permanentId)
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.PermanentId = permanentId;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "CartId cookie functions (ReadCartIdFromCookie(), WriteCartIdToCookie(string cartId))"

        /// <summary>
        /// read PermanentId value from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadCartIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.CartId;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the PermanentId value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteCartIdToCookie(string cartId)
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.CartId = cartId;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "Language cookie functions (ReadLanguageFromCookie(), WriteLanguageToCookie(string language), RemoveLanguageFromCookie())"

        /// <summary>
        /// read language string from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadLanguageFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.Language;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the language string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteLanguageToCookie(string language)
        //{
        //    try
        //    {
        //        CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(language), "Language can not be null or empty!");

        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.Language = language;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// delete language value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        //protected void RemoveLanguageFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.Language = String.Empty;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "RegistrationId cookie functions (ReadRegistrationIdFromCookie(), WriteRegistrationIdToCookie(string registrationId))"

        /// <summary>
        /// read RegistrationId string from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadRegistrationIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.RegistrationId;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the RegistrationId string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteRegistrationIdToCookie(string registrationId)
        //{
        //    try
        //    {
        //        CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(registrationId), "Language can not be null or empty!");

        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.RegistrationId = registrationId;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// regisztrációs azonosító eltávolítása a sütiből
        /// </summary>
        //protected void RemoveRegistrationIdFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.RegistrationId = String.Empty;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "Currency cookie functions (ReadCurrencyFromCookie(), WriteCurrencyToCookie(string language), RemoveCurrencyFromCookie())"

        /// <summary>
        /// read Currency string from http cookie
        /// </summary>
        /// <returns></returns>
        //protected string ReadCurrencyFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.Currency;
        //    }
        //    catch { return String.Empty; }
        //}

        /// <summary>
        /// write the Currency string to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteCurrencyToCookie(string currency)
        //{
        //    try
        //    {
        //        CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(currency), "Currency can not be null or empty!");

        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.Currency = currency;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

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
        //protected bool ReadShoppingCartOpenedFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.IsShoppingCartOpened;
        //    }
        //    catch { return false; }
        //}

        /// <summary>
        /// write the IsShoppingCartOpened value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteShoppingCartOpenedToCookie(bool isShoppingCartOpened)
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.IsShoppingCartOpened = isShoppingCartOpened;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// delete isShoppingCartOpened value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        //protected void RemoveShoppingCartOpenedFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.IsShoppingCartOpened = false;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

        #region "CatalogueOpened cookie functions (ReadCatalogueOpenedFromCookie(), void WriteCatalogueOpenedToCookie(bool isCatalogueOpened), RemoveCatalogueOpenedFromCookie())"

        /// <summary>
        /// read IsCatalogueOpened value from http cookie
        /// </summary>
        /// <returns></returns>
        //protected bool ReadCatalogueOpenedFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        return visitorData.IsCatalogueOpened;
        //    }
        //    catch { return false; }
        //}

        /// <summary>
        /// write the IsCatalogueOpened value to http cookie (if the named cookie exists than existing cookie will be used, otherwise new cookie will be created)
        /// The cookie expiring date is: current date + 30 day 
        /// </summary>
        /// <param name="objectId"></param>
        //protected void WriteCatalogueOpenedToCookie(bool isCatalogueOpened)
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.IsCatalogueOpened = isCatalogueOpened;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        /// <summary>
        /// delete IsCatalogueOpened value from the http cookie
        /// </summary>
        /// <param name="objectId"></param>
        //protected void RemoveCatalogueOpenedFromCookie()
        //{
        //    try
        //    {
        //        CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

        //        visitorData.IsCatalogueOpened = false;

        //        this.WriteCookie(visitorData);
        //    }
        //    catch { }
        //}

        #endregion

    }
}
