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

        [HttpPost]
        [ActionName("NewsletterListByFilter")]
        public HttpResponseMessage NewsletterListByFilter(CompanyGroup.WebClient.Models.NewsletterListByFilterRequest request)
        {
            bool manufacturerDisplayConditions = ListContainsAnotherListItem<string>(request.ManufacturerIdList, request.DisplayConditionManufacturerIdList, request.DisplayIfConditionsAreEmpty);

            bool category1DisplayConditions = ListContainsAnotherListItem<string>(request.Category1IdList, request.DisplayConditionCategory1IdList, request.DisplayIfConditionsAreEmpty);

            bool category2DisplayConditions = ListContainsAnotherListItem<string>(request.Category2IdList, request.DisplayConditionCategory2IdList, request.DisplayIfConditionsAreEmpty);

            if (!manufacturerDisplayConditions && !category1DisplayConditions && !category2DisplayConditions)
            {
                return Request.CreateResponse<CompanyGroup.WebClient.Models.Newsletter>(HttpStatusCode.Created, new CompanyGroup.WebClient.Models.Newsletter( new CompanyGroup.Dto.WebshopModule.NewsletterCollection() ));
            }

            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            //hírlevél lista lekérdezése
            CompanyGroup.Dto.WebshopModule.GetNewsletterListByFilterRequest req = new CompanyGroup.Dto.WebshopModule.GetNewsletterListByFilterRequest(visitorData.Language, visitorData.VisitorId, request.NewsletterIdList);

            CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetNewsletterListByFilterRequest, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetNewsletterListByFilter", req);

            CompanyGroup.WebClient.Models.Newsletter viewModel = new CompanyGroup.WebClient.Models.Newsletter(response);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.Newsletter>(HttpStatusCode.Created, viewModel);

            return httpResponseMessage;
        }
    }
}
