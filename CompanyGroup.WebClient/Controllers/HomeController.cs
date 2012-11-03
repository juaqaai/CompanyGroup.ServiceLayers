using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// cshtml view-k betöltését végzi
    /// </summary>
    public class HomeController : System.Web.Mvc.Controller
    {
        /// <summary>
        /// Webshop view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Webshop view";

            return View();
        }

        /// <summary>
        /// Webshop details view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Webshop()
        {
            ViewBag.Message = "Details webshop view";

            return View();
        }

        /// <summary>
        /// Newsletter view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletter()
        {
            ViewBag.Message = "Newsletter view.";

            return View();
        }

        /// <summary>
        /// PartnerInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult PartnerInfo()
        {
            ViewBag.Message = "PartnerInfo view.";

            return View();
        }

        /// <summary>
        /// InvoiceInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoiceInfo()
        {
            ViewBag.Message = "InvoiceInfo view.";

            return View();
        }

        /// <summary>
        /// OrderInfo view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderInfo()
        {
            ViewBag.Message = "OrderInfo view.";

            return View();
        }

        /// <summary>
        /// Registration view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Registration()
        {
            ViewBag.Message = "Registration view.";

            return View();
        }

        /// <summary>
        /// ChangePassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            ViewBag.Message = "ChangePassword view.";

            return View();
        }

        /// <summary>
        /// UndoChangePassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult UndoChangePassword()
        {
            ViewBag.Message = "UndoChangePassword view.";

            return View();
        }

        /// <summary>
        /// ForgetPassword view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPassword()
        {
            ViewBag.Message = "ForgetPassword view.";

            return View();
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
