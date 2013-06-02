using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// hírlevéllel kapcsolatos műveletek
    /// </summary>
    public class NewsletterApiController : ApiBaseController
    {
        [HttpPost]
        [ActionName("NewsletterList")]
        public HttpResponseMessage NewsletterList()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            //hírlevél lista lekérdezése
            CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest(visitorData.Language,visitorData.VisitorId,String.Empty);

            CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetCollection", request);

            CompanyGroup.WebClient.Models.Newsletter viewModel = new CompanyGroup.WebClient.Models.Newsletter(response);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.Newsletter>(HttpStatusCode.Created, viewModel);

            return httpResponseMessage;
        }
    }
}
