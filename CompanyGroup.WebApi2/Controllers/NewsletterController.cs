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
    public class NewsletterController : ApiBaseController
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
        [HttpPost]
        [ActionName("GetCollection")]
        public HttpResponseMessage GetCollection(CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.NewsletterCollection response = service.GetNewsletterCollection(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.NewsletterCollection>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("NewsletterController GetCollection {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }

    }
}
