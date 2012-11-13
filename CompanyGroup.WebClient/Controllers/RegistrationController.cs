using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebClient.Controllers
{
    public class RegistrationController : BaseController
    {
        /// <summary>
        /// Registration view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(this.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// aláírási címpéldány file feltöltés   
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFile()
        {
            CompanyGroup.WebClient.Models.UploadedFile response = new CompanyGroup.WebClient.Models.UploadedFile();

            foreach (string file in Request.Files)
            {
                System.Web.HttpPostedFileBase postedFile = Request.Files[file] as System.Web.HttpPostedFileBase;
                
                if (postedFile.ContentLength == 0) continue;

                string fileName = String.Format("{0}_{1}{2}{3}", postedFile.FileName, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                string savedFileName = System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads"), System.IO.Path.GetFileName(fileName));

                postedFile.SaveAs(savedFileName);

                response.Name = fileName;
                response.Length = postedFile.ContentLength;
                response.Type = postedFile.ContentType;
            }

            return Json(response, "application/json; charset=utf-8", System.Text.Encoding.UTF8, JsonRequestBehavior.DenyGet);
        } 
    }
}
