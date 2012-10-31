using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// hírlevéllel kapcsolatos műveletek
    /// </summary>
    public class NewsletterController : ApiController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.INewsletterService service;

        /// <summary>
        /// konstruktor hírlevéllel kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public NewsletterController(CompanyGroup.ApplicationServices.WebshopModule.INewsletterService service)
        {
            this.service = service;
        }

        /// <summary>
        /// hírlevél gyűjetemény lekérdezése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetNewsletterCollection")]
        public CompanyGroup.Dto.WebshopModule.NewsletterCollection GetNewsletterCollection(CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection request)
        {
            return service.GetNewsletterCollection(request);
        }


    }
}
