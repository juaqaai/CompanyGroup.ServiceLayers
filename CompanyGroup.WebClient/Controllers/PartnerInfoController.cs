using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
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

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// InvoiceInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoiceInfo()
        {
            ViewBag.Message = "InvoiceInfo view.";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest request = new CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest()
            //{
            //    LanguageId = visitorData.Language,
            //    VisitorId = visitorData.VisitorId,
            //    Debit = true, 
            //    Overdue = true
            //};

            //List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetInvoiceInfoRequest, List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>>("Customer", "GetInvoiceInfo", request);

            //List<CompanyGroup.WebClient.Models.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.WebClient.Models.InvoiceInfo>();

            //invoiceInfoList.AddRange(response.ConvertAll(x => new CompanyGroup.WebClient.Models.InvoiceInfo(x)));

            //CompanyGroup.WebClient.Models.InvoiceInfoList model = new CompanyGroup.WebClient.Models.InvoiceInfoList(invoiceInfoList, visitor);

            return View(visitor);
        }

        /// <summary>
        /// OrderInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderInfo()
        {
            ViewBag.Message = "OrderInfo view.";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest req = new CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest(visitorData.VisitorId, visitorData.Language, request.CanBeTaken, request.SalesStatus);

            //System.Net.Http.HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest, System.Net.Http.HttpResponseMessage>("SalesOrder", "GetOrderInfo", req);

            //CompanyGroup.Dto.PartnerModule.OrderInfoList orderInfoList = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.OrderInfoList>().Result : new CompanyGroup.Dto.PartnerModule.OrderInfoList(0, new List<CompanyGroup.Dto.PartnerModule.OrderInfo>());

            //CompanyGroup.WebClient.Models.OrderInfoList viewModel = new CompanyGroup.WebClient.Models.OrderInfoList(orderInfoList.Items, orderInfoList.OpenOrderAmount, visitor);

            //return Request.CreateResponse<CompanyGroup.WebClient.Models.OrderInfoList>(HttpStatusCode.OK, viewModel);

            return View(visitor);
        }

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
        //[System.Web.Mvc.HttpGet]
        public ActionResult UndoChangePassword(string undoChangePassword)
        {
            ViewBag.Message = "UndoChangePassword view.";

            CompanyGroup.Dto.PartnerModule.UndoChangePassword response;

            if (String.IsNullOrEmpty(undoChangePassword))
            {
                CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest request = new CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest(undoChangePassword);

                response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest, CompanyGroup.Dto.PartnerModule.UndoChangePassword>("ContactPerson", "UndoChangePassword", request);
            }
            else
            {
                response = new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Succeeded = false, Message = "A jelszómódosítás visszavonásához tartozó azonosító nem lett megadva!" };
            }

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

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
