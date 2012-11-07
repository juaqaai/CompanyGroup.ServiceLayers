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
        [HttpGet]
        [ActionName("GetInvoiceInfo")]
        public CompanyGroup.WebClient.Models.InvoiceInfoList GetInvoiceInfo(CompanyGroup.WebClient.Models.GetInvoiceInfo request)
        {

            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo req = new CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo()
            {
                LanguageId = visitorData.Language,
                VisitorId = visitorData.ObjectId,
                PaymentType = request.PaymentType
            };

            List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo, List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>("Customer", "GetInvoiceInfo", req);

            List<CompanyGroup.WebClient.Models.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.WebClient.Models.InvoiceInfo>();

            invoiceInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.InvoiceInfo(x)));

            CompanyGroup.WebClient.Models.InvoiceInfoList model = new CompanyGroup.WebClient.Models.InvoiceInfoList(invoiceInfoList);

            return model;
        }

    }
}
