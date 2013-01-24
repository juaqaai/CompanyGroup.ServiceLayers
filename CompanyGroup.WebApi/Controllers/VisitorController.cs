using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// látogatóval kapcsolatos műveletek
    /// </summary>
    public class VisitorController : ApiController
    {
        private CompanyGroup.ApplicationServices.PartnerModule.IVisitorService service;

        /// <summary>
        /// konstruktor látogatóval kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public VisitorController(CompanyGroup.ApplicationServices.PartnerModule.IVisitorService service)
        { 
            this.service = service;
        }

        /// <summary>
        /// bejelentkezés           
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("SignIn")]
        [HttpPost]
        public HttpResponseMessage SignIn(CompanyGroup.Dto.ServiceRequest.SignInRequest request)
        {
            CompanyGroup.Dto.PartnerModule.Visitor response = service.SignIn(request);

            return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.Visitor>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        [ActionName("SignOut")]
        [HttpPost]
        public HttpResponseMessage SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request)
        {
            service.SignOut(request);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó információk kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetVisitorInfo")]
        [HttpPost]
        public HttpResponseMessage GetVisitorInfo(CompanyGroup.Dto.ServiceRequest.VisitorInfoRequest request)
        {
            CompanyGroup.Dto.PartnerModule.Visitor response = service.GetVisitorInfo(request);

            return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.Visitor>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// valutanem csere 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("ChangeCurrency")]
        [HttpPost]
        public HttpResponseMessage ChangeCurrency(CompanyGroup.Dto.ServiceRequest.ChangeCurrencyRequest request)
        {
            service.ChangeCurrency(request);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("ChangeLanguage")]
        [HttpPost]
        public HttpResponseMessage ChangeLanguage(CompanyGroup.Dto.ServiceRequest.ChangeLanguageRequest request)
        {
            service.ChangeLanguage(request);

            return Request.CreateResponse(HttpStatusCode.OK); 
        }
    }
}
