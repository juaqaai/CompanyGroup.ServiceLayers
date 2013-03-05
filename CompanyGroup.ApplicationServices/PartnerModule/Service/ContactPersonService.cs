using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// kapcsolattartó műveleteket tartalmazó szolgáltatások
    /// </summary>
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class ContactPersonService : ServiceCoreBase, IContactPersonService 
    {
        /// <summary>
        /// kapcsolattartó repository
        /// </summary>
        CompanyGroup.Domain.PartnerModule.IContactPersonRepository contactPersonRepository;

        /// <summary>
        /// jelszómódosítás repository
        /// </summary>
        CompanyGroup.Domain.PartnerModule.IChangePasswordRepository changePasswordRepository;

        /// <summary>
        /// elfelejtett jelszó repository
        /// </summary>
        CompanyGroup.Domain.PartnerModule.IForgetPasswordRepository forgetPasswordRepository;

        /// <summary>
        /// konstruktor repository paraméterekkel
        /// </summary>
        /// <param name="contactPersonRepository"></param>
        /// <param name="changePasswordRepository"></param>
        /// <param name="visitorRepository"></param>
        public ContactPersonService(CompanyGroup.Domain.PartnerModule.IContactPersonRepository contactPersonRepository, 
                                    CompanyGroup.Domain.PartnerModule.IChangePasswordRepository changePasswordRepository, 
                                    CompanyGroup.Domain.PartnerModule.IForgetPasswordRepository forgetPasswordRepository, 
                                    CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (contactPersonRepository == null)
            {
                throw new ArgumentNullException("ContactPersonRepository");
            }

            if (changePasswordRepository == null)
            {
                throw new ArgumentNullException("ChangePasswordRepository");
            }

            if (forgetPasswordRepository == null)
            {
                throw new ArgumentNullException("ForgetPasswordRepository");
            }

            this.contactPersonRepository = contactPersonRepository;

            this.changePasswordRepository = changePasswordRepository;

            this.forgetPasswordRepository = forgetPasswordRepository;
        }

        /// <summary>
        /// kapcsolattartó lekérdezése azonosító alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.ContactPerson GetContactPersonById(CompanyGroup.Dto.PartnerModule.GetContactPersonByIdRequest request)
        {
            //ha üres a látogató azonosító
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.VisitorId), CompanyGroup.Domain.Resources.Messages.validationVisitorIdCannotBeNull);

            try
            {
                //látogató kikeresése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.PartnerModule.ContactPerson contactPerson = contactPersonRepository.GetContactPerson(visitor.PersonId, visitor.DataAreaId);

                CompanyGroup.Dto.PartnerModule.ContactPerson result = new ContactPersonToContactPerson().MapToPartnerModuleContactPerson(contactPerson);

                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// jelszómódosítás visszavonása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest request)
        {
            if (String.IsNullOrEmpty(request.Id))
            {
                return new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validation_ChangePasswordLogIdCannotBeNull, Succeeded = false };
            }

            //visszavonandó jelszómódosítás
            CompanyGroup.Domain.PartnerModule.ChangePassword chagePassword = changePasswordRepository.GetItemByKey(request.Id);

            //visszavonandó jelszómódosítás státuszának passzívra állítása
            changePasswordRepository.SetStatus(request.Id, Domain.PartnerModule.ChangePasswordStatus.Passive);

            //látogató kikeresése
            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(chagePassword.VisitorId);

            //ha üres a látogató azonosító
            if (visitor.Id.Equals(MongoDB.Bson.ObjectId.Empty))
            {
                return new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validationVisitorIdCannotBeNull, Succeeded = false };
            }

            //jelszómódosítás művelet előkészítése
            CompanyGroup.Domain.PartnerModule.ChangePasswordCreate changePasswordCreate = new CompanyGroup.Domain.PartnerModule.ChangePasswordCreate()
            {
                ContactPersonId = visitor.PersonId,
                DataAreaId = chagePassword.DataAreaId,
                NewPassword = chagePassword.OldPassword,
                OldPassword = chagePassword.NewPassword,
                WebLoginName = chagePassword.UserName
            };
            //jelszómódosítás AX (visszaállítás)
            CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult changePasswordCreateResult = changePasswordRepository.Change(changePasswordCreate);

            //jelszómódosítás log hozzáadás
            CompanyGroup.Domain.PartnerModule.ChangePassword changePasswordLog = new CompanyGroup.Domain.PartnerModule.ChangePassword()
            {
                CreatedDate = DateTime.Now,
                DataAreaId = chagePassword.DataAreaId,
                NewPassword = changePasswordCreate.NewPassword,
                OldPassword = changePasswordCreate.OldPassword,
                UserName = changePasswordCreate.WebLoginName,
                Status = Domain.PartnerModule.ChangePasswordStatus.Active,
                VisitorId = visitor.Id.ToString(),
                Id = MongoDB.Bson.ObjectId.GenerateNewId()
            };

            changePasswordRepository.Add(changePasswordLog);

            return new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validation_ChangePasswordLogIdCannotBeNull, Succeeded = true };
        }

        /// <summary>
        /// jelszó módosítás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.PartnerModule.ChangePasswordRequest request)
        {
            //ha üres a látogató azonosító
            if (String.IsNullOrEmpty(request.VisitorId))
            {
                return new CompanyGroup.Dto.PartnerModule.ChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validationVisitorIdCannotBeNull, OperationSucceeded = false, SendMailSucceeded = false };
            }

            //látogató kikeresése
            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            //látogató belépésének ellenörzése, csak a személyi belépések esetén lehetséges a jelszómódosítás
            if (!visitor.LoginType.Equals(LoginType.Person))
            {
                return new CompanyGroup.Dto.PartnerModule.ChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.verification_ChangePasswordNotAllowed, OperationSucceeded = false, SendMailSucceeded = false };
            }

            //lehetséges-e a megadott adatokkal a jelszómódosítsítás művelet?
            CompanyGroup.Domain.PartnerModule.ChangePasswordVerify verify = contactPersonRepository.VerifyChangePassword(visitor.PersonId, request.UserName, request.OldPassword, request.NewPassword, visitor.DataAreaId);

            //ha nem, akkor kilépés hibaüzenettel
            if (!verify.Success)
            {
                return new CompanyGroup.Dto.PartnerModule.ChangePassword() { Message = verify.Message, OperationSucceeded = false, SendMailSucceeded = false };                
            }

            //jelszómódosítsítás művelet előkészítése
            CompanyGroup.Domain.PartnerModule.ChangePasswordCreate changePasswordCreate = new CompanyGroup.Domain.PartnerModule.ChangePasswordCreate()
                                                                                              { 
                                                                                                  ContactPersonId = visitor.PersonId, 
                                                                                                  DataAreaId = visitor.DataAreaId, 
                                                                                                  NewPassword = request.NewPassword, 
                                                                                                  OldPassword = request.OldPassword, 
                                                                                                  WebLoginName = request.UserName 
                                                                                              };
            //jelszómódosítás AX
            CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult changePasswordCreateResult = changePasswordRepository.Change(changePasswordCreate);

            //jelszómódosítás log hozzáadás
            CompanyGroup.Domain.PartnerModule.ChangePassword changePassword = new CompanyGroup.Domain.PartnerModule.ChangePassword()
                                                                                {
                                                                                    CreatedDate = DateTime.Now,
                                                                                    DataAreaId = visitor.DataAreaId,
                                                                                    NewPassword = request.NewPassword,
                                                                                    OldPassword = request.OldPassword, 
                                                                                    UserName = request.UserName,
                                                                                    Status = changePasswordCreateResult.Succeeded ? Domain.PartnerModule.ChangePasswordStatus.Active : Domain.PartnerModule.ChangePasswordStatus.Failed,
                                                                                    VisitorId = visitor.Id.ToString(),
                                                                                    Id = MongoDB.Bson.ObjectId.GenerateNewId()
                                                                                };

            changePasswordRepository.Add(changePassword);

            bool sendMailSucceeded = false;

            //levél elküldése sikeres esetben
            if (changePasswordCreateResult.Succeeded)
            {
                sendMailSucceeded = this.SendChangePasswordMail(changePassword, visitor);
            }
            else
            {
                sendMailSucceeded = this.SendChangePasswordFailedMail(changePassword, visitor, changePasswordCreateResult);
            }

            string message = ( changePasswordCreateResult.Succeeded && !sendMailSucceeded ) ? CompanyGroup.Domain.Resources.Messages.verification_ChangePasswordMailSendFailed : changePasswordCreateResult.Message;

            return new CompanyGroup.Dto.PartnerModule.ChangePassword() { Message = message, OperationSucceeded = changePasswordCreateResult.Succeeded, SendMailSucceeded = sendMailSucceeded };
        }

        #region "jelszómódosítás mail küldés"

        private static readonly string ChangePasswordMailSubject = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailSubject", "Jelszó módosítás értesítő üzenet");

        private static readonly string ChangePasswordFailedMailSubject = Helpers.ConfigSettingsParser.GetString("ChangePasswordFailedMailSubject", "Hibás jelszó módosítás értesítő üzenet");

        private static readonly string ChangePasswordMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailHtmlTemplateFile", "changepassword.html");

        private static readonly string ChangePasswordFailedMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordFailedMailHtmlTemplateFile", "changepasswordfailed.html");

        private static readonly string ChangePasswordMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailTextTemplateFile", "changepassword.txt");

        private static readonly string ChangePasswordFailedMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordFailedMailTextTemplateFile", "changepasswordfailed.txt");

        private static readonly string ChangePasswordMailFromAddress = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailFromAddress", "webadmin@hrp.hu");

        private static readonly string ChangePasswordMailFromName = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailFromName", "HRP-BSC web adminisztrátor csoport");

        private static readonly string ChangePasswordFailedMailToAddress = Helpers.ConfigSettingsParser.GetString("ChangePasswordFailedMailToAddress", "webadmin@hrp.hu");

        private static readonly string ChangePasswordFailedMailToName = Helpers.ConfigSettingsParser.GetString("ChangePasswordFailedMailToName", "HRP-BSC web adminisztrátor csoport"); 

        private static readonly string ChangePasswordMailBCcAddress = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailBCcAddress", "ajuhasz@hrp.hu"); 

        private static readonly string ChangePasswordMailBCcName = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailBCcName", "Juhász Attila");

        private static readonly string ChangePasswordMailSmtpHost = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailSmtpHost", "195.30.7.14");

        private static readonly string UndoChangePasswordUrl = Helpers.ConfigSettingsParser.GetString("UndoChangePasswordUrl", "http://1juhasza/cms/PartnerInfo/UndoChangePassword/{0}/");

        private static readonly bool TestMode = Helpers.ConfigSettingsParser.GetBoolean("TestMode", true);

        /// <summary>
        /// jelszómódosítás email küldés
        /// </summary>
        /// <param name="changePassword"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        private bool SendChangePasswordMail(CompanyGroup.Domain.PartnerModule.ChangePassword changePassword, 
                                            CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                string tmpHtml = ContactPersonService.HtmlText(ContactPersonService.ChangePasswordMailHtmlTemplateFile);
                string html = tmpHtml.Replace("$Name$", visitor.PersonName)
                                        .Replace("$NewPassword$", changePassword.NewPassword)
                                        .Replace("$OldPassword$", changePassword.OldPassword)
                                        .Replace("$WebLoginName$", changePassword.UserName)
                                        .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                        .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                        .Replace("$UndoChangePasswordUrl$", String.Format(ContactPersonService.UndoChangePasswordUrl, changePassword.Id.ToString()));

                string tmpPlain = ContactPersonService.PlainText(ContactPersonService.ChangePasswordMailTextTemplateFile);
                string plain = tmpPlain.Replace("$PersonName$", visitor.PersonName)
                                        .Replace("$NewPassword$", changePassword.NewPassword)
                                        .Replace("$OldPassword$", changePassword.OldPassword)
                                        .Replace("$WebLoginName$", changePassword.UserName)
                                        .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                        .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                        .Replace("$UndoChangePasswordUrl$", String.Format(ContactPersonService.UndoChangePasswordUrl, changePassword.Id.ToString()));

                string toAddress = ContactPersonService.TestMode ? ContactPersonService.ChangePasswordMailBCcAddress : visitor.Email;

                string toName = visitor.PersonName;

                CompanyGroup.Domain.Core.MailSettings mailSettings = new CompanyGroup.Domain.Core.MailSettings(ContactPersonService.ChangePasswordMailSmtpHost,
                                                                                                               ContactPersonService.ChangePasswordMailSubject,
                                                                                                               plain,
                                                                                                               html, 
                                                                                                               ContactPersonService.ChangePasswordMailFromName, 
                                                                                                               ContactPersonService.ChangePasswordMailFromAddress);

                mailSettings.BccAddressList.Add(ContactPersonService.ChangePasswordMailBCcName, ContactPersonService.ChangePasswordMailBCcAddress);

                mailSettings.ToAddressList.Add(toAddress, toName);

                return this.SendMail(mailSettings);

            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

        /// <summary>
        /// jelszómódosítás email küldés
        /// </summary>
        /// <param name="changePassword"></param>
        /// <param name="visitor"></param>
        /// <param name="changePasswordCreateResult"></param>
        /// <returns></returns>
        private bool SendChangePasswordFailedMail(CompanyGroup.Domain.PartnerModule.ChangePassword changePassword,
                                                  CompanyGroup.Domain.PartnerModule.Visitor visitor, 
                                                  CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult changePasswordCreateResult)
        {
            try
            {
                string tmpHtml = ContactPersonService.HtmlText(ContactPersonService.ChangePasswordFailedMailHtmlTemplateFile);
                string html = tmpHtml.Replace("$Name$", visitor.PersonName)
                                        .Replace("$NewPassword$", changePassword.NewPassword)
                                        .Replace("$OldPassword$", changePassword.OldPassword)
                                        .Replace("$WebLoginName$", changePassword.UserName)
                                        .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                        .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                        .Replace("$ErrorCode$", String.Format("{0}", changePasswordCreateResult.ResultCode))
                                        .Replace("$ErrorMessage$", changePasswordCreateResult.Message);

                string tmpPlain = ContactPersonService.PlainText(ContactPersonService.ChangePasswordFailedMailTextTemplateFile);
                string plain = tmpPlain.Replace("$PersonName$", visitor.PersonName)
                                        .Replace("$NewPassword$", changePassword.NewPassword)
                                        .Replace("$OldPassword$", changePassword.OldPassword)
                                        .Replace("$WebLoginName$", changePassword.UserName)
                                        .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                        .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                        .Replace("$ErrorCode$", String.Format("{0}", changePasswordCreateResult.ResultCode))
                                        .Replace("$ErrorMessage$", changePasswordCreateResult.Message);

                string toAddress = ContactPersonService.TestMode ? ContactPersonService.ChangePasswordMailBCcAddress : ContactPersonService.ChangePasswordFailedMailToAddress;

                string toName = ContactPersonService.TestMode ? ContactPersonService.ChangePasswordMailBCcName : ContactPersonService.ChangePasswordFailedMailToName;

                CompanyGroup.Domain.Core.MailSettings mailSettings = new CompanyGroup.Domain.Core.MailSettings(ContactPersonService.ChangePasswordMailSmtpHost,
                                                                                                               ContactPersonService.ChangePasswordFailedMailSubject,
                                                                                                               plain,
                                                                                                               html,
                                                                                                               ContactPersonService.ChangePasswordMailFromName,
                                                                                                               ContactPersonService.ChangePasswordMailFromAddress);

                mailSettings.BccAddressList.Add(ContactPersonService.ChangePasswordMailBCcName, ContactPersonService.ChangePasswordMailBCcAddress);

                mailSettings.ToAddressList.Add(toAddress, toName);

                return this.SendMail(mailSettings);

            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

        #endregion

        /// <summary>
        /// elfelejtett jelszó
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.ForgetPassword ForgetPassword(CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest request)
        {
            //ha üres a felhasználónév 
            if (String.IsNullOrEmpty(request.UserName))
            {
                return new CompanyGroup.Dto.PartnerModule.ForgetPassword(false, CompanyGroup.Domain.Resources.Messages.verification_UserNameCannotBeNull);
            }

            //lehetséges-e a megadott adatokkal az elfelejtett jelszó művelet? Van-e ilyen felhasználó?
            CompanyGroup.Domain.PartnerModule.ForgetPassword forgetPassword = contactPersonRepository.GetForgetPassword(request.UserName);

            string message = forgetPassword.GetMessage(request.Language);

            //ha nem, akkor kilépés hibaüzenettel
            if (!forgetPassword.Succeedeed)
            {
                return new CompanyGroup.Dto.PartnerModule.ForgetPassword(false, message);
            }

            bool sent = false; 

            //levél elküldése dbManagers-nek sikertelen esetben
            if (!forgetPassword.Succeedeed)
            {
                this.SendForgetPasswordFailedMail(forgetPassword, message);
            }
            else
            {
                sent = this.SendForgetPasswordMail(forgetPassword);

                if (!sent) { message = (request.Language.Equals(CompanyGroup.Domain.Core.Constants.LanguageEnglish)) ? "Sending email failed!" : "Az email küldés nem sikerült!"; }
            }

            return new CompanyGroup.Dto.PartnerModule.ForgetPassword((forgetPassword.Succeedeed && sent), message);
        }

        #region "Elfelejtett jelszó email küldés"

        private static readonly string ForgetPasswordFailedMailSubject = Helpers.ConfigSettingsParser.GetString("ForgetPasswordFailedMailSubject", "Hibás elfelejtett jelszó művelet értesítő üzenet");

        private static readonly string ForgetPasswordMailSubject = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailSubject", "HRP - BSC elfelejtett jelszó művelet értesítő üzenet");

        private static readonly string ForgetPasswordFailedMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("ForgetPasswordFailedMailHtmlTemplateFile", "forgetpasswordfailed.html");

        private static readonly string ForgetPasswordMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailHtmlTemplateFile", "forgetpassword.html");

        private static readonly string ForgetPasswordFailedMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("ForgetPasswordFailedMailTextTemplateFile", "forgetpasswordfailed.txt");

        private static readonly string ForgetPasswordMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailTextTemplateFile", "forgetpassword.txt");

        private static readonly string ForgetPasswordMailFromAddress = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailFromAddress", "webadmin@hrp.hu");

        private static readonly string ForgetPasswordMailFromName = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailFromName", "HRP-BSC web adminisztrátor csoport");

        private static readonly string ForgetPasswordFailedMailToAddress = Helpers.ConfigSettingsParser.GetString("ForgetPasswordFailedMailToAddress", "webadmin@hrp.hu");

        private static readonly string ForgetPasswordFailedMailToName = Helpers.ConfigSettingsParser.GetString("ForgetPasswordFailedMailToName", "HRP-BSC web adminisztrátor csoport"); 

        private static readonly string ForgetPasswordMailBCcAddress = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailBCcAddress", "ajuhasz@hrp.hu"); 

        private static readonly string ForgetPasswordMailBCcName = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailBCcName", "Juhász Attila");

        private static readonly string ForgetPasswordMailSmtpHost = Helpers.ConfigSettingsParser.GetString("ForgetPasswordMailSmtpHost", "195.30.7.14");

        private bool SendForgetPasswordMail(CompanyGroup.Domain.PartnerModule.ForgetPassword forgetPassword)
        {
            try
            {
                string tmpHtml = ContactPersonService.HtmlText(ContactPersonService.ForgetPasswordMailHtmlTemplateFile);
                string html = tmpHtml.Replace("$RecipientName$", String.Format("{0} {1}", forgetPassword.CompanyName, forgetPassword.PersonName))
                                     .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                     .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                     .Replace("$UserName$", forgetPassword.UserName)
                                     .Replace("$Password$", forgetPassword.Password)
                                     .Replace("$Email$", forgetPassword.Email);

                string tmpPlain = ContactPersonService.PlainText(ContactPersonService.ForgetPasswordMailTextTemplateFile);
                string plain = tmpPlain.Replace("$RecipientName$", String.Format("{0} {1}", forgetPassword.CompanyName, forgetPassword.PersonName))
                                     .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                     .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                     .Replace("$UserName$", forgetPassword.UserName)
                                     .Replace("$Password$", forgetPassword.Password)
                                     .Replace("$Email$", forgetPassword.Email);

                string toAddress = ContactPersonService.TestMode ? ContactPersonService.ForgetPasswordMailBCcAddress : forgetPassword.Email;

                string toName = ContactPersonService.TestMode ? ContactPersonService.ForgetPasswordMailBCcName : String.Format("{0} {1}", forgetPassword.CompanyName, forgetPassword.PersonName);

                CompanyGroup.Domain.Core.MailSettings mailSettings = new CompanyGroup.Domain.Core.MailSettings(ContactPersonService.ForgetPasswordMailSmtpHost,
                                                                                                               ContactPersonService.ForgetPasswordMailSubject,
                                                                                                               plain,
                                                                                                               html,
                                                                                                               ContactPersonService.ForgetPasswordMailFromName,
                                                                                                               ContactPersonService.ForgetPasswordMailFromAddress);

                mailSettings.BccAddressList.Add(ContactPersonService.ForgetPasswordMailBCcName, ContactPersonService.ForgetPasswordMailBCcAddress);

                mailSettings.ToAddressList.Add(toAddress, toName);

                return this.SendMail(mailSettings);

            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

        /// <summary>
        /// elfelejtett jelszó email küldés, sikertelen művelet esetére
        /// </summary>
        /// <param name="forgetPassword"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool SendForgetPasswordFailedMail(CompanyGroup.Domain.PartnerModule.ForgetPassword forgetPassword, string message)
        {
            try
            {
                string tmpHtml = ContactPersonService.HtmlText(ContactPersonService.ForgetPasswordFailedMailHtmlTemplateFile);
                string html = tmpHtml.Replace("$WebLoginName$", forgetPassword.UserName)
                                     .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                     .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                     .Replace("$ErrorMessage$", message);

                string tmpPlain = ContactPersonService.PlainText(ContactPersonService.ForgetPasswordFailedMailTextTemplateFile);
                string plain = tmpPlain.Replace("$WebLoginName$", forgetPassword.UserName)
                                     .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                     .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                     .Replace("$ErrorMessage$", message);

                string toAddress = ContactPersonService.TestMode ? ContactPersonService.ForgetPasswordMailBCcAddress : ContactPersonService.ForgetPasswordFailedMailToAddress;

                string toName = ContactPersonService.TestMode ? ContactPersonService.ForgetPasswordMailBCcName : ContactPersonService.ForgetPasswordFailedMailToName;

                CompanyGroup.Domain.Core.MailSettings mailSettings = new CompanyGroup.Domain.Core.MailSettings(ContactPersonService.ForgetPasswordMailSmtpHost,
                                                                                                               ContactPersonService.ForgetPasswordFailedMailSubject,
                                                                                                               plain,
                                                                                                               html,
                                                                                                               ContactPersonService.ForgetPasswordMailFromName,
                                                                                                               ContactPersonService.ForgetPasswordMailFromAddress);

                mailSettings.BccAddressList.Add(ContactPersonService.ForgetPasswordMailBCcName, ContactPersonService.ForgetPasswordMailBCcAddress);

                mailSettings.ToAddressList.Add(toAddress, toName);

                return this.SendMail(mailSettings);

            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

        #endregion
    }
}
