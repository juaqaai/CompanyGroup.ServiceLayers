using System;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;


namespace CompanyGroup.GlobalServices
{
    public class ServiceBase
    {
        private readonly static string ServiceBaseUrl = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceBaseUrl", "http://1Juhasza/CompanyGroup.GlobalServicesHost/{0}.svc/");

        private static string BaseUrl(string serviceName)
        {
            return String.Format(ServiceBaseUrl, serviceName);
        }

        #region "RestSharp (T PostJSonData<T>(string serviceName, string resource, object requestBody) where T : new()), T GetJSonData<T>(string serviceName, string resource) where T : new(), byte[] DownloadData(string serviceName, string resource))"

        /// <summary>
        /// post json data to an application service url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">Catalogue</param>
        /// <param name="resource">GetStructures</param>
        /// <param name="requestBody">new { DataAreaId = dataAreaId, ActionFilter = false, BargainFilter = false, NewFilter = false, StockFilter = false, TextFilter = textFilter }</param>
        /// <returns></returns>
        protected T PostJSonData<T>(string serviceName, string resource, object requestBody) where T : new()
        {

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(serviceName), "Service name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(resource), "Resource name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require((requestBody != null), "RequestBody can not be null!");

            RestSharp.RestClient client = null;

            try
            {
                client = new RestSharp.RestClient(BaseUrl(serviceName));

                RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.POST);

                request.RequestFormat = RestSharp.DataFormat.Json;

                request.Resource = resource;

                request.AddBody(requestBody);

                RestSharp.RestResponse<T> response = client.Execute<T>(request);

                return response.Data;
            }
            catch { return default(T); }
        }

        /// <summary>
        /// retriewe json data from an application service url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <example>http://localhost/CompanyGroup.ServicesHost/PictureService.svc/GetItem/5637193425/PFI702GY/hrp</example>
        protected T GetJSonData<T>(string serviceName, string resource) where T : new()
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(serviceName), "Service name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(resource), "Resource name can not be null or empty!");

            RestSharp.RestClient client = null;

            try
            {
                client = new RestSharp.RestClient(BaseUrl(serviceName));

                RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.GET);

                request.RequestFormat = RestSharp.DataFormat.Json;

                request.Resource = resource;
                //request.BaseUrl = "http://carma.org";
                //request.Action = "api/1.1/searchPlants";
                //request.AddParameter("location", 4338);
                //request.AddParameter("limit", 10);
                //request.AddParameter("color", "red");
                //request.AddParameter("format", "xml");
                //request.ResponseFormat = ResponseFormat.Xml;
                //var plants = client.Execute<PowerPlantsDTO>(request);
                RestSharp.RestResponse<T> response = client.Execute<T>(request);

                return response.Data;
            }
            catch { return default(T); }
        }

        /// <summary>
        /// retriewe raw byte array data from an application service url
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected byte[] DownloadData(string serviceName, string resource)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(serviceName), "Service name can not be null or empty!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(resource), "Resource name can not be null or empty!");

            RestSharp.RestClient client = null;

            try
            {
                client = new RestSharp.RestClient(BaseUrl(serviceName));

                RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.GET);

                request.Resource = resource;

                return client.DownloadData(request);
            }
            catch
            {
                return new byte[] { };
            }
        }

        #endregion
    }
}
