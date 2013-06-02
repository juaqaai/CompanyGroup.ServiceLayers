using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// eseméynregisztráció műveletek
    /// </summary>
    public class EventRegistrationService : ServiceCoreBase, IEventRegistrationService
    {

        private CompanyGroup.Domain.PartnerModule.IEventRegistrationRepository eventRegistrationRepository;

        /// <summary>
        /// konstruktor eseményregisztráció repository-val és
        /// visitor repository-val. 
        /// </summary>
        /// <param name="eventRegistrationRepository"></param>
        /// <param name="visitorRepository"></param>
        public EventRegistrationService(CompanyGroup.Domain.PartnerModule.IEventRegistrationRepository eventRegistrationRepository, 
                                        CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (eventRegistrationRepository == null)
            {
                throw new ArgumentNullException("EventRegistrationRepository");
            }

            this.eventRegistrationRepository = eventRegistrationRepository;
        }

        /// <summary>
        /// eseményregisztráció hozzáadás   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool AddNew(CompanyGroup.Dto.PartnerModule.EventRegistration request)
        { 
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(request.Data.Keys.Count);

                sb.Append("<EventRegistration>");

                foreach( string key in request.Data.Keys)
                {
                    sb.AppendFormat("<{0}>{1}</{2}>", key, request.Data[key], key);
                }

                sb.Append("</EventRegistration>");

                bool response = eventRegistrationRepository.AddNew(request.EventId, request.EventName, sb.ToString());

                //this.SendMail(request);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "email küldés"

        private static readonly string EventRegistrationMailSubject = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailSubject", "Eseményregisztráció értesítő üzenet");

        private static readonly string EventRegistrationMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailHtmlTemplateFile", "EventRegistration.html");

        private static readonly string EventRegistrationMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailTextTemplateFile", "EventRegistration.txt");

        private static readonly string EventRegistrationMailFromAddress = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailFromAddress", "webadmin@hrp.hu");

        private static readonly string EventRegistrationMailFromName = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailFromName", "HRP-BSC web adminisztrátor csoport");

        private static readonly string EventRegistrationMailBCcAddress = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailBCcAddress", "ajuhasz@hrp.hu");

        private static readonly string EventRegistrationMailBCcName = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailBCcName", "Juhász Attila");

        private static readonly string EventRegistrationMailSmtpHost = Helpers.ConfigSettingsParser.GetString("EventRegistrationMailSmtpHost", "195.30.7.14");

        #endregion

        /// <summary>
        /// eseményregisztrációs email küldése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool SendMail(CompanyGroup.Dto.PartnerModule.EventRegistration request)
        {
            try
            {
                string tmpHtml = EventRegistrationService.HtmlText(EventRegistrationService.EventRegistrationMailHtmlTemplateFile);
                string html = tmpHtml.Replace("$EventName$", request.EventName)
                                     .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                     .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));

                foreach(string key in request.Data.Keys)
                {
                    string keyExpression = String.Format("${0}$", key);

                    html = html.Replace(keyExpression, request.Data[key]);
                }

                string tmpPlain = EventRegistrationService.PlainText(EventRegistrationService.EventRegistrationMailTextTemplateFile);
                string plain = tmpPlain.Replace("$EventName$", request.EventName)
                                       .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                       .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));

                foreach (string key in request.Data.Keys)
                {
                    string keyExpression = String.Format("${0}$", key);

                    html = html.Replace(keyExpression, request.Data[key]);
                }

                CompanyGroup.Domain.Core.MailSettings mailSettings = new CompanyGroup.Domain.Core.MailSettings(EventRegistrationService.EventRegistrationMailSmtpHost,
                                                                                                               EventRegistrationService.EventRegistrationMailSubject,
                                                                                                               plain,
                                                                                                               html,
                                                                                                               EventRegistrationService.EventRegistrationMailFromName,
                                                                                                               EventRegistrationService.EventRegistrationMailFromAddress);

                mailSettings.BccAddressList.Add(EventRegistrationService.EventRegistrationMailBCcName, EventRegistrationService.EventRegistrationMailBCcAddress);

                string toName = request.Data.Keys.Contains("PersonName") ? request.Data["PersonName"] : EventRegistrationService.EventRegistrationMailBCcName;

                string toAddress = request.Data.Keys.Contains("Email") ? request.Data["Email"] : EventRegistrationService.EventRegistrationMailBCcAddress;

                mailSettings.ToAddressList.Add(toName, toAddress);

                return this.SendMail(mailSettings);

            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("EventRegistrationService SendMail {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return false;
            }

        }
    }
}
