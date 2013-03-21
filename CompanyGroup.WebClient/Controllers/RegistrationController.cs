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
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            return View(visitor);
        }

        /// <summary>
        /// aláírási címpéldány file feltöltés   
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase signature_entity_file)
        {
            CompanyGroup.WebClient.Models.UploadedFile response = new CompanyGroup.WebClient.Models.UploadedFile();

            try
            {
                string fileName = String.Empty;

                string extension = String.Empty;

                string fileNameGenerated = String.Empty;

                if (signature_entity_file.ContentLength > 0)
                {
                    DateTime now = DateTime.Now;

                    fileName = System.IO.Path.GetFileNameWithoutExtension(signature_entity_file.FileName);

                    extension = System.IO.Path.GetExtension(signature_entity_file.FileName);

                    fileNameGenerated = String.Format("{0}_{1}_{2}_{3}_{4}_{5}{6}", fileName, now.Year, now.Month, now.Day, now.Hour, now.Minute, extension);

                    string path = System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileNameGenerated);

                    signature_entity_file.SaveAs(path);

                    response.Name = fileNameGenerated;

                    response.Length = signature_entity_file.ContentLength;

                    response.Type = signature_entity_file.ContentType;
                }

                return Json(response, "application/json; charset=utf-8", System.Text.Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                response.Name = ex.Message;

                return Json(response, "application/json");
            }
        }

        [HttpPost]
        public JsonResult UploadApplyDoc(HttpPostedFileBase file)
        {
            try
            {
                bool result = false;
                string fileName = String.Empty;
                string extension = String.Empty;
                string fileNameGenerated = String.Empty;

                if (file.ContentLength > 0)
                {
                    DateTime now = DateTime.Now;

                    fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);

                    extension = System.IO.Path.GetExtension(file.FileName);

                    fileNameGenerated = String.Format("{0}_{1}_{2}_{3}_{4}_{5}{6}", fileName, now.Year, now.Month, now.Day, now.Hour, now.Minute, extension);

                    string path = System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileNameGenerated);

                    file.SaveAs(path);

                    result = true;
                }
                else
                {
                    result = false;
                }
                return Json(new { Result = result, FileName = fileNameGenerated }, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, FileName = ex.Message }, "application/json");
            }
        }

        /// <summary>
        /// szerződés nyomtatása
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Print()
        {
            //return Redirect("~/Reports/Print.aspx");
            try
            {
                CompanyGroup.WebClient.Models.RegistrationData model = this.GetRegistration();

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CompanyGroup.WebClient.Models.RegistrationData GetRegistration()
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = null;

                //regisztrációs azonosító kiolvasása sütiből
                CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

                //CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

                //ha nem volt regisztrációs azonosítója, akkor adatok olvasása az ERP-ből     
                if (String.IsNullOrEmpty(visitorData.RegistrationId) && !String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>(String.Format("{0}/{1}/{2}", "Customer", "GetCustomerRegistration", RegistrationController.DataAreaId)); ;
                }
                //volt már regisztrációs azonosítója, ezért az ahhoz tartozó adatokat kell visszaolvasni a cacheDb-ből
                else if (!String.IsNullOrEmpty(visitorData.RegistrationId))
                {
                    CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request = new CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey(visitorData.RegistrationId, visitorData.VisitorId);

                    response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey, CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "GetByKey", request);
                }
                //ha nincs belépve, és nincs megkezdett regisztrációja sem, akkor üreset kell visszaadni
                else
                {
                    response = new CompanyGroup.Dto.RegistrationModule.Registration();
                }

                CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts = new CompanyGroup.Dto.RegistrationModule.BankAccounts(response.BankAccounts);

                CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons = new CompanyGroup.Dto.RegistrationModule.ContactPersons(response.ContactPersons);

                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses(response.DeliveryAddresses);

                CompanyGroup.WebClient.Models.RegistrationData model = new CompanyGroup.WebClient.Models.RegistrationData(bankAccounts,
                                                                                                                          response.CompanyData,
                                                                                                                          contactPersons,
                                                                                                                          response.DataRecording,
                                                                                                                          deliveryAddresses,
                                                                                                                          response.InvoiceAddress,
                                                                                                                          response.MailAddress,
                                                                                                                          response.RegistrationId,
                                                                                                                          response.Visitor,
                                                                                                                          response.WebAdministrator,
                                                                                                                          new CompanyGroup.WebClient.Models.Countries());

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
