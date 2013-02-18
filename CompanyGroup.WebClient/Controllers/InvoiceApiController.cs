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
        [ActionName("GetList")]
        public HttpResponseMessage GetList(CompanyGroup.WebClient.Models.GetInvoiceInfoRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            try
            {
                CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest req = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest()
                {
                    LanguageId = visitorData.Language,
                    VisitorId = visitorData.VisitorId,
                    Debit = request.Debit, 
                    Overdue = request.Overdue
                };

                List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest, List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>("Invoice", "GetList", req);

                List<CompanyGroup.WebClient.Models.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.WebClient.Models.InvoiceInfo>();

                if (response != null)
                {
                    invoiceInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.InvoiceInfo(x)));
                }

                CompanyGroup.WebClient.Models.InvoiceInfoList model = new CompanyGroup.WebClient.Models.InvoiceInfoList(invoiceInfoList, visitor);

                return Request.CreateResponse<CompanyGroup.WebClient.Models.InvoiceInfoList>(HttpStatusCode.OK, model);
            }
            catch(Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
