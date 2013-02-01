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
    public class BaseController : System.Web.Mvc.Controller
    {
        protected readonly static string CookieName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("CookieName", "companygroup_hrpbsc");

        /// <summary>
        /// látogató objektum lekérdezése süti objectId alapján
        /// </summary>
        /// <param name="visitorData"></param>
        /// <returns></returns>
        protected CompanyGroup.WebClient.Models.Visitor GetVisitor(CompanyGroup.WebClient.Models.VisitorData visitorData)
        {
            Helpers.DesignByContract.Require( (visitorData != null), "VisitorData cannot be null or empty!");

            CompanyGroup.WebClient.Models.Visitor visitor;

            try
            {
                if (String.IsNullOrWhiteSpace(visitorData.VisitorId))
                {
                    visitor = new CompanyGroup.WebClient.Models.Visitor();
                }
                else
                {
                    CompanyGroup.Dto.PartnerModule.VisitorInfoRequest request = new CompanyGroup.Dto.PartnerModule.VisitorInfoRequest() { VisitorId = visitorData.VisitorId, DataAreaId = BaseController.DataAreaId };

                    CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.VisitorInfoRequest, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "GetVisitorInfo", request);

                    visitor = new CompanyGroup.WebClient.Models.Visitor(response);
                }

                return visitor;
            }
            catch { return new CompanyGroup.WebClient.Models.Visitor(); }
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
        protected TResponse PostJSonData<TRequest, TResponse>(string controllerName, string actionName, TRequest request) where TRequest : new() where TResponse : new()
        {

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(controllerName), "Controller name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(actionName), "Action name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Request can not be null!");

            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                client.BaseAddress = new Uri(BaseController.ServiceBaseAddress);

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
