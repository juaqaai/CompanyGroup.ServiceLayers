﻿using System;
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
            return View();
        }

        /// <summary>
        /// Newsletter view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletter()
        {
            ViewBag.Message = "Newsletter view.";

            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, BaseController.CookieName);

            if (visitorData == null)
            {
                visitorData = new CompanyGroup.WebClient.Models.VisitorData();
            }

            CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection request = new CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection()
            {
                Language = visitorData.Language,
                VisitorId = visitorData.ObjectId,
                ManufacturerId = String.Empty
            };

            CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetNewsletterCollection, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetCollection", request);

            CompanyGroup.WebClient.Models.NewsletterCollection model = new CompanyGroup.WebClient.Models.NewsletterCollection(response);

            return View(model);
        }

        /// <summary>
        /// Carreer view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Carreer()
        {
            ViewBag.Message = "Carreer view.";

            return View();
        }

    }
}
