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
        [HttpGet]
        [ActionName("GetList")]
        public CompanyGroup.WebClient.Models.InvoiceInfoList GetList(int id)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            //if (!visitor.IsValidLogin)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            //}

            try
            {
                CompanyGroup.Dto.ServiceRequest.GetInvoiceInfoRequest req = new CompanyGroup.Dto.ServiceRequest.GetInvoiceInfoRequest()
                {
                    LanguageId = visitorData.Language,
                    VisitorId = visitorData.VisitorId,
                    PaymentType = id
                };

                List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetInvoiceInfoRequest, List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>("Invoice", "GetList", req);

                List<CompanyGroup.WebClient.Models.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.WebClient.Models.InvoiceInfo>();

                if (response != null)
                {
                    invoiceInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.InvoiceInfo(x)));
                }

                CompanyGroup.WebClient.Models.InvoiceInfoList model = new CompanyGroup.WebClient.Models.InvoiceInfoList(invoiceInfoList, visitor);

                return model;
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

    }
}
