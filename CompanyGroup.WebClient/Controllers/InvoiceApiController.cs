using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class InvoiceApiController : ApiBaseController
    {
        /// <summary>
        /// számla információk lekérdezése műveletek
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetInvoiceInfo")]
        public HttpResponseMessage GetInvoiceInfo(CompanyGroup.WebClient.Models.GetInvoiceInfoRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            try
            {
                CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest req = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest( visitorData.VisitorId, 
                                                                                                                                     visitorData.Language, 
                                                                                                                                     request.Debit, 
                                                                                                                                     request.Overdue, 
                                                                                                                                     request.ItemId, 
                                                                                                                                     request.ItemName, 
                                                                                                                                     request.SalesId, 
                                                                                                                                     request.SerialNumber,
                                                                                                                                     request.InvoiceId,  
                                                                                                                                     request.DateIntervall,  
                                                                                                                                     request.Sequence, 
                                                                                                                                     request.CurrentPageIndex, 
                                                                                                                                     request.ItemsOnPage, 
                                                                                                                                     request.Items);

                CompanyGroup.Dto.PartnerModule.InvoiceInfo response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest, CompanyGroup.Dto.PartnerModule.InvoiceInfo>("Invoice", "GetInvoiceInfo", req);

                CompanyGroup.WebClient.Models.InvoiceInfo model = new CompanyGroup.WebClient.Models.InvoiceInfo(response.Items, response.Pager, response.ListCount, response.TotalNettoCredit, response.AllOverdueDebts, visitor);

                return Request.CreateResponse<CompanyGroup.WebClient.Models.InvoiceInfo>(HttpStatusCode.OK, model);
            }
            catch(Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// összes számla kiolvasása (mindkét vállalatból)
        /// </summary>
        /// <returns></returns>
        [ActionName("GetDetails")]
        [HttpGet]
        public HttpResponseMessage GetDetails(int id)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

                CompanyGroup.Dto.PartnerModule.GetDetailedInvoiceInfoRequest request = new Dto.PartnerModule.GetDetailedInvoiceInfoRequest(visitorData.VisitorId, visitorData.Language, id);

                CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetDetailedInvoiceInfoRequest, CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed>("Invoice", "GetDetails", request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.InvoiceInfoDetailed>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
