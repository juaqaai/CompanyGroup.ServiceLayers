using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices
{
    /// <summary>
    /// ősosztály a visitor service-ek összefogására
    /// </summary>
    public class ServiceCoreBase
    {
        protected CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository;

        private const string CACHEKEY_VISITOR = "visitor";

        /// <summary>
        /// 30 percig cache-be kerül a visitor objektum
        /// </summary>
        private const double CACHE_EXPIRATION_VISITOR = 30d;

        /// <summary>
        /// konstruktor visitor repository-val
        /// </summary>
        /// <param name="customerRepository"></param>
        public ServiceCoreBase(CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository)
        {
            if (visitorRepository == null)
            {
                throw new ArgumentNullException("VisitorRepository");
            }

            this.visitorRepository = visitorRepository;
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó mentett információk kiolvasása
        /// - ha a kulcs üres, akkor új visitor példánnyal tér vissza
        /// - ha nincs a cache-ben az objektum, akkor repository hívás történik
        /// - ha nem érvényes a bejelentkezés, akkor repository hívás történik
        /// - egyebkent visszaadásra kerül a cache tartalma
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected CompanyGroup.Domain.PartnerModule.Visitor GetVisitor(string objectId)
        {
            if (String.IsNullOrWhiteSpace(objectId))
            {
                return CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor();
            }

            CompanyGroup.Domain.PartnerModule.Visitor visitor = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_VISITOR, objectId));

            if ((visitor == null) || (!visitor.IsValidLogin))
            {
                visitor = this.GetVisitorFromRepository(objectId);
            }

            return visitor;
        }

        /// <summary>
        /// visitor kikeresése repository-ból, majd cache-be mentés
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        private CompanyGroup.Domain.PartnerModule.Visitor GetVisitorFromRepository(string objectId)
        {
            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(objectId);

                visitor.SetLoggedIn();

                //cache-be mentés csak akkor, ha érvényes a login
                if (visitor.IsValidLogin)
                {
                    visitor.Representative.SetDefault();

                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_VISITOR, objectId), visitor, DateTime.Now.AddMinutes(CACHE_EXPIRATION_VISITOR));
                }

                return visitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// levél txt template
        /// </summary>
        /// <param name="mailTextTemplateFile"></param>
        protected static string PlainText(string mailTextTemplateFile)
        {
            System.IO.StreamReader sr = null;
            try
            {
                string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + mailTextTemplateFile);

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

        /// <summary>
        /// finanszírozási ajánlat levél html template
        /// </summary>
        protected static string HtmlText(string mailTextTemplateFile)
        {
            System.IO.StreamReader sr = null;
            try
            {
                string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + mailTextTemplateFile);

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

        /// <summary>
        /// levélküldés
        /// </summary>
        /// <param name="mailSettings"></param>
        /// <returns></returns>
        protected bool SendMail(CompanyGroup.Domain.Core.MailSettings mailSettings)
        {
            try
            {
                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage(mailSettings.Subject, mailSettings.PlainText, mailSettings.HtmlText);

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
                mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                foreach (KeyValuePair<string, string> toAddress in mailSettings.ToAddressList.Addresses)
                {
                    mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + toAddress.Key + ">", toAddress.Value, System.Text.Encoding.Default));
                }

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, String.Format("<{0}>", mailSettings.FromAddress), mailSettings.FromName, System.Text.Encoding.Default));

                foreach (KeyValuePair<string, string> bccAddress in mailSettings.ToAddressList.Addresses)
                {
                    mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, String.Format("<{0}>", bccAddress.Key), bccAddress.Value, System.Text.Encoding.Default));
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
                mailSender.SmtpHost = mailSettings.SmtpHost;
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

        /// <summary>
        /// xml string sorosítás
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string Serialize<T>(T obj)
        {
            System.Xml.XmlWriter writer = null;

            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                writer = System.Xml.XmlWriter.Create(sb);

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);

                string tmp = sb.ToString();

                return tmp;
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }

        /// <summary>
        /// string-ből xml-be visszasorosítás
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected T DeSerialize<T>(string xml)
        {
            System.Xml.XmlReader xmlReader = null;
            try
            {
                System.IO.StringReader stringReader = new System.IO.StringReader(xml);

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                xmlReader = new System.Xml.XmlTextReader(stringReader);

                T obj = (T)serializer.Deserialize(xmlReader);

                return obj;
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }

    }
}
