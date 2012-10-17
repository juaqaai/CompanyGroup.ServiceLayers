using System;
using System.Collections.Generic;

namespace Newsletter.ApplicationService
{
    /// <summary>
    /// MailMerge lib szerinti implementáció
    /// </summary>
    public class MailMergeStrategy : BaseStrategy, ISendStrategy, IDisposable
    {
        public MailMergeStrategy()
        {
            Init();
        }

        MailMergeLib.MailMergeSender mailSender;

        private void Init()
        {
            mailSender = new MailMergeLib.MailMergeSender();

            //esemenykezelok beallitasa, ha van
            //mailSender.OnSendFailure += new EventHandler<MailMergeLib.MailSenderSendFailureEventArgs>(delegate(object obj, MailMergeLib.MailSenderSendFailureEventArgs args)
            ////( ( obj, args ) =>
            //{
            //    string errorMsg = args.Error.Message;
            //    MailMergeLib.MailMergeMessage.MailMergeMessageException ex = args.Error as MailMergeLib.MailMergeMessage.MailMergeMessageException;
            //    if (ex != null && ex.Exceptions.Count > 0)
            //    {
            //        errorMsg = string.Format("{0}", ex.Exceptions[0].Message);
            //    }
            //    string text = string.Format("Error: {0}", errorMsg);

            //});

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
            mailSender.SmtpHost = smtpMailServerName;

            mailSender.SmtpPort = SmtpPort;

            //mailSender.SetSmtpAuthentification(@"hrp.hu\juhasza", "juha");        
        }

        public void Send(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId)
        {
            try
            {
                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage();

                mmm.Subject = subject;
                mmm.PlainText = plain;
                mmm.HtmlText = html;
                //mmm.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.None;

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding(encoding); //System.Text.Encoding.GetEncoding("iso-8859-2");

                //System.Globalization.CultureTypes.AllCultures; //new System.Globalization.CultureInfo("sr-SP-Latn")
                mmm.CultureInfo = (dataAreaId.ToLower().Equals("ser")) ? System.Globalization.CultureInfo.InvariantCulture : new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = ""; //System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                string toEmail = testMode ? testModeMailAddress : recipient.Email;

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + toEmail + ">", recipient.Name, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, "<" + senderEmail + ">", senderName, System.Text.Encoding.Default));
                //mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, "<" + Shared.Web.Helpers.ConfigSettingsParser.GetString( "TestMailName", "ajuhasz@hrp.hu") + ">", "Juhász Attila", System.Text.Encoding.Default));

                mailSender.Send(mmm);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
         
        }

        public void Dispose()
        {
            //if (mailSender != null)
            //{
            //    mailSender.Dispose();
            //}
        }
    }
}
