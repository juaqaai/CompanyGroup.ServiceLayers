using System;
using System.Collections.Generic;

namespace Newsletter.ApplicationService
{
    /// <summary>
    /// Higlabo szerinti kiküldés implementáció
    /// </summary>
    public class HigLaboStrategy : BaseStrategy, ISendStrategy, IDisposable
    {
        public HigLaboStrategy()
        {
            Init();
        }

        public void Init()
        {
            client = new HigLabo.Net.Smtp.SmtpClient();

            client.ServerName = smtpMailServerName;

            client.Port = SmtpPort;

            client.Ssl = false;

            client.AuthenticateMode = HigLabo.Net.Smtp.SmtpAuthenticateMode.None;

            //client.UserName = @"hrp.hu\juhasza";

            //client.Password = "juha";            
        }

        HigLabo.Net.Smtp.SmtpClient client;

        public void Send(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId)
        {
            try
            {

                HigLabo.Net.Smtp.SmtpMessage message = new HigLabo.Net.Smtp.SmtpMessage();

                message.Priority = HigLabo.Net.Mail.MailPriority.Normal;

                message.ContentEncoding = System.Text.Encoding.GetEncoding(encoding);

                message.ContentTransferEncoding = HigLabo.Net.Mail.TransferEncoding.QuotedPrintable;

                message.HeaderEncoding = System.Text.Encoding.GetEncoding(encoding);

                message.HeaderTransferEncoding = HigLabo.Net.Mail.TransferEncoding.QuotedPrintable;

                message.Date = DateTime.Now.ToUniversalTime();

                message["Mime-Version"] = "1.0";

                message.Subject = subject;

                message.BodyText = html;

                message.IsHtml = true;

                message.From = senderEmail;

                message.ReplyTo = senderEmail;

                message.To.Add(new HigLabo.Net.Mail.MailAddress(recipient.Email, recipient.Name));

                //HigLabo.Net.Smtp.SmtpContent content = new HigLabo.Net.Smtp.SmtpContent().;

                //message.Contents.Add(content);

                HigLabo.Net.Smtp.SendMailResult result = client.SendMail(message);

                if (!result.SendSuccessful)
                {
                    HigLabo.Net.Smtp.SendMailResultState resultState = result.State;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }
    }
}
