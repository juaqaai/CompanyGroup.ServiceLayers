using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// html kezdőlap kontroller
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SendMail()
        {
            MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage("test", "plain text", "<strong>HTML text</strong>");

            mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
            mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
            mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
            mmm.Priority = System.Net.Mail.MailPriority.Normal;
            mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;


            mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<ajuhasz@hrp.hu>", "Juhász Attila", System.Text.Encoding.Default));

            mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, String.Format("<{0}>", "webadmin@hrp.hu"), "HRP webadmin csoport", System.Text.Encoding.Default));

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
            mailSender.SmtpHost = "195.30.7.14";
            mailSender.SmtpPort = 25;
            //mailSender.SetSmtpAuthentification( "username", "password" );

            mailSender.Send(mmm);


            return View("SendMail", "Mailküldés próba");
        }
    }
}
