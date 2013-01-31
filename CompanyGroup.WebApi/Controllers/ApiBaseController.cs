using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// API kontrollerek őse
    /// </summary>
    public class ApiBaseController : ApiController
    {
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
    }
}
