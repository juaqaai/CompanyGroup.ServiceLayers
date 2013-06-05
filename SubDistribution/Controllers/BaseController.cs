using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SubDistribution.Controllers
{
    public class BaseController : ApiController
    {
        #region "Http exception"

        /// <summary>
        /// biztonságos kivétel dobása
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        protected void ThrowSafeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            HttpResponseMessage response = Request.CreateResponse<SubDistribution.Models.ApiMessage>(statusCode, new SubDistribution.Models.ApiMessage(message));

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


        protected HttpResponseMessage GetXmlData(string serviceBaseAddress, string actionName, string parameter)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(serviceBaseAddress))
                {
                    throw new ApplicationException("ServiceBaseAddress cannot be null or empty!"); 
                }

                if (String.IsNullOrWhiteSpace(actionName))
                {
                    throw new ApplicationException("ActionName cannot be null or empty!");
                }

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(serviceBaseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));

                string requestUrl = String.Format("{0}/{1}", actionName, parameter);

                HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                return response;
            }
            catch(Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}