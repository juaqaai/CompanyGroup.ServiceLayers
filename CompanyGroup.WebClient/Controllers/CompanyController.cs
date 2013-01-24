using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebClient.Controllers
{
    public class CompanyController : BaseController
    {

        /// <summary>
        /// céginformáció
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// Newsletter view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletter()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, BaseController.CookieName);

            if (visitorData == null)
            {
                visitorData = new CompanyGroup.WebClient.Models.VisitorData();
            }

            CompanyGroup.Dto.ServiceRequest.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.ServiceRequest.GetNewsletterCollectionRequest()
            {
                Language = visitorData.Language,
                VisitorId = visitorData.VisitorId,
                ManufacturerId = String.Empty
            };

            CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetNewsletterCollectionRequest, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetCollection", request);

            CompanyGroup.WebClient.Models.NewsletterCollection model = new CompanyGroup.WebClient.Models.NewsletterCollection(response);

            return View(model);
        }

        /// <summary>
        /// Carreer view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Carreer()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// Guide view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Guide()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return View(visitor);
        }

        public ActionResult Contact()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return View(visitor);
        }

        public ActionResult WasteManagement()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return View(visitor);
        }
    }
}
