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
            //CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, BaseController.CookieName);

            //if (visitorData == null)
            //{
            //    visitorData = new CompanyGroup.WebClient.Models.VisitorData();
            //}

            //CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest()
            //{
            //    Language = visitorData.Language,
            //    VisitorId = visitorData.VisitorId,
            //    ManufacturerId = String.Empty
            //};

            //CompanyGroup.Dto.WebshopModule.NewsletterCollection response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest, CompanyGroup.Dto.WebshopModule.NewsletterCollection>("Newsletter", "GetCollection", request);

            //CompanyGroup.WebClient.Models.Newsletter viewModel = new CompanyGroup.WebClient.Models.Newsletter(response);

            return View();
        }

        /// <summary>
        /// Carreer view kezdőérték beállításokkal 
        /// </summary>
        /// <returns></returns>
        public ActionResult Carreer()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor viewModel = this.GetVisitor(visitorData);

            return View(viewModel);
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

        public JsonResult ApplyForJob(CompanyGroup.WebClient.Models.ApplyForJobRequest request)
        {
            bool result = this.CarreerApplyForJobMail(request);

            return Json(result);
        }

        #region "karrier email küldés"

        /// <summary>
        /// finanszírozási ajánlat levél txt template
        /// </summary>
        private static string CarreerPlainText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + CarreerApplyForJobMailTextTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat levél html template
        /// </summary>
        private static string CarreerHtmlText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + CarreerApplyForJobMailHtmlTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// karrier levélküldés 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool CarreerApplyForJobMail(CompanyGroup.WebClient.Models.ApplyForJobRequest request)
        {
            try
            {
                string tmpHtml = CarreerHtmlText;
                string html = tmpHtml.Replace("$DayfBirth$", request.DayfBirth)
                                     .Replace("$Email$", request.Email)
                                     .Replace("$FirstName$", request.FirstName)
                                     .Replace("$LastName$", request.LastName)
                                     .Replace("$Message$", request.Message)
                                     .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                     .Replace("$PermanentAddress$", request.PermanentAddress)
                                     .Replace("$Phone$", request.Phone)
                                     .Replace("$PlaceOfBirth$", request.PlaceOfBirth)
                                     .Replace("$Position$", request.Position)
                                     .Replace("$TemporaryAddress$", request.TemporaryAddress)
                                     .Replace("$UploadFileName$", request.UploadFileName);

                string tmpPlain = CarreerPlainText;
                string plain = tmpPlain.Replace("$DayfBirth$", request.DayfBirth)
                                     .Replace("$Email$", request.Email)
                                     .Replace("$FirstName$", request.FirstName)
                                     .Replace("$LastName$", request.LastName)
                                     .Replace("$Message$", request.Message)
                                     .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                     .Replace("$PermanentAddress$", request.PermanentAddress)
                                     .Replace("$Phone$", request.Phone)
                                     .Replace("$PlaceOfBirth$", request.PlaceOfBirth)
                                     .Replace("$Position$", request.Position)
                                     .Replace("$TemporaryAddress$", request.TemporaryAddress)
                                     .Replace("$UploadFileName$", request.UploadFileName);

                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage(CarreerApplyForJobMailSubject, plain, html);

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
                mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + CarreerApplyForJobMailToAddress + ">", CarreerApplyForJobMailToName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, "<webadmin@hrp.hu>", "web adminisztrátor csoport", System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, "<" + CarreerApplyForJobMailBccAddress + ">", CarreerApplyForJobMailBccName, System.Text.Encoding.Default));

                if (!String.IsNullOrEmpty(request.UploadFileName))
                {
                    mmm.AddExternalInlineAttachment(new MailMergeLib.FileAttachment(System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads"), request.UploadFileName), request.UploadFileName));
                }

                //mail sender
                MailMergeLib.MailMergeSender mailSender = new MailMergeLib.MailMergeSender();

                //esemenykezelok beallitasa, ha van
                mailSender.OnSendFailure += new EventHandler<MailMergeLib.MailSenderSendFailureEventArgs>(delegate(object obj, MailMergeLib.MailSenderSendFailureEventArgs args)
                //( ( obj, args ) =>
                {
                    string errorMsg = args.Error.Message;
                    MailMergeLib.MailMergeMessage.MailMergeMessageException ex = args.Error as MailMergeLib.MailMergeMessage.MailMergeMessageException;
                    if (ex != null && ex.Exceptions.Count > 0)
                    {
                        errorMsg = string.Format("{0}", ex.Exceptions[0].Message);
                    }
                    string text = string.Format("Error: {0}", errorMsg);

                });

                mailSender.LocalHostName = Environment.MachineName; //"mail." + 
                mailSender.MaxFailures = 1;
                mailSender.DelayBetweenMessages = 1000;
                string messageOutputDir = System.IO.Path.GetTempPath() + @"\mail";
                if (!System.IO.Directory.Exists(messageOutputDir))
                {
                    System.IO.Directory.CreateDirectory(messageOutputDir);
                }
                mailSender.MailOutputDirectory = messageOutputDir;
                mailSender.MessageOutput = MailMergeLib.MessageOutput.SmtpServer;  // change to MessageOutput.Directory if you like

                // smtp details
                mailSender.SmtpHost = BaseController.MailSmtpHost;
                mailSender.SmtpPort = 25;
                //mailSender.SetSmtpAuthentification( "username", "password" );

                mailSender.Send(mmm);

                return true;
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

        #endregion

        /// <summary>
        /// Guide view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Guide()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor viewModel = this.GetVisitor(visitorData);

            return View(viewModel);
        }

        /// <summary>
        /// kapcsolattartás view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor viewModel = this.GetVisitor(visitorData);

            return View(viewModel);
        }

        /// <summary>
        /// hulladékkezelés view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult WasteManagement()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CompanyController.CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor viewModel = this.GetVisitor(visitorData);

            return View(viewModel);
        }
    }
}
