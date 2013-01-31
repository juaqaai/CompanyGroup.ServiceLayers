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
    public class VisitorController : ApiBaseController
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
        public HttpResponseMessage SignIn(CompanyGroup.Dto.PartnerModule.SignInRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.Visitor response = service.SignIn(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.Visitor>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        [ActionName("SignOut")]
        [HttpPost]
        public HttpResponseMessage SignOut(CompanyGroup.Dto.PartnerModule.SignOutRequest request)
        {
            try
            {
                service.SignOut(request);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó információk kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetVisitorInfo")]
        [HttpPost]
        public HttpResponseMessage GetVisitorInfo(CompanyGroup.Dto.PartnerModule.VisitorInfoRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.Visitor response = service.GetVisitorInfo(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.Visitor>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// valutanem csere 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("ChangeCurrency")]
        [HttpPost]
        public HttpResponseMessage ChangeCurrency(CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest request)
        {
            try
            {
                service.ChangeCurrency(request);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("ChangeLanguage")]
        [HttpPost]
        public HttpResponseMessage ChangeLanguage(CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest request)
        {
            try
            {
                service.ChangeLanguage(request);

                return Request.CreateResponse(HttpStatusCode.OK); 
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
