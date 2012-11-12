using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebClient.Controllers
{
    public class PartnerInfoController : BaseController
    {

        /// <summary>
        /// PartnerInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "PartnerInfo view.";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// InvoiceInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        //public ActionResult InvoiceInfo()
        //{
        //    ViewBag.Message = "InvoiceInfo view.";

        //    CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

        //    CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request = new CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo()
        //    {
        //        LanguageId = visitorData.Language,
        //        VisitorId = visitorData.ObjectId,
        //        PaymentType = 1
        //    };

        //    List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo, List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>("Customer", "GetInvoiceInfo", request);

        //    List<CompanyGroup.WebClient.Models.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.WebClient.Models.InvoiceInfo>();

        //    invoiceInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.InvoiceInfo(x)));

        //    CompanyGroup.WebClient.Models.InvoiceInfoList model = new CompanyGroup.WebClient.Models.InvoiceInfoList(invoiceInfoList);

        //    return View(model);
        //}

        /// <summary>
        /// OrderInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        //public ActionResult OrderInfo()
        //{
        //    ViewBag.Message = "OrderInfo view.";

        //    CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

        //    CompanyGroup.Dto.ServiceRequest.GetOrderInfo request = new CompanyGroup.Dto.ServiceRequest.GetOrderInfo()
        //    {
        //        LanguageId = visitorData.Language,
        //        VisitorId = visitorData.ObjectId,
        //    };

        //    List<CompanyGroup.Dto.PartnerModule.OrderInfo> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetOrderInfo, List<CompanyGroup.Dto.PartnerModule.OrderInfo>>("SalesOrder", "GetOrderInfo", request);

        //    List<CompanyGroup.WebClient.Models.OrderInfo> orderInfoList = new List<CompanyGroup.WebClient.Models.OrderInfo>();

        //    orderInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.OrderInfo(x)));

        //    CompanyGroup.WebClient.Models.OrderInfoList model = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfoList);

        //     return View(model);
        //}

        /// <summary>
        /// ChangePassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        //public ActionResult ChangePassword()
        //{
        //    ViewBag.Message = "ChangePassword view.";

        //    CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

        //    CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

        //    return View(visitor);
        //}

        /// <summary>
        /// jelszómódosítást visszavonó művelet - view kezdőérték beállításokkal
        /// </summary>
        /// <param name="undoChangePassword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UndoChangePassword(string undoChangePassword)
        {
            ViewBag.Message = "UndoChangePassword view.";

            CompanyGroup.Dto.PartnerModule.UndoChangePassword response;

            if (String.IsNullOrEmpty(undoChangePassword))
            {
                CompanyGroup.Dto.ServiceRequest.UndoChangePassword request = new CompanyGroup.Dto.ServiceRequest.UndoChangePassword(undoChangePassword);

                response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UndoChangePassword, CompanyGroup.Dto.PartnerModule.UndoChangePassword>("ContactPerson", "UndoChangePassword", request);
            }
            else
            {
                response = new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Succeeded = false, Message = "A jelszómódosítás visszavonásához tartozó azonosító nem lett megadva!" };
            }

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.WebClient.Models.UndoChangePassword model = new CompanyGroup.WebClient.Models.UndoChangePassword(response, visitor);

            return View(model);
        }

        /// <summary>
        /// ForgetPassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        //public ActionResult ForgetPassword()
        //{
        //    ViewBag.Message = "ForgetPassword view.";

        //    CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

        //    CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

        //    return View(visitor);
        //}


    }
}
