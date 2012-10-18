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
    [ServiceBehavior(UseSynchronizationContext = false,
                     InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true),
                     System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    [CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
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
        /// konstruktor repository paraméterekkel
        /// </summary>
        /// <param name="contactPersonRepository"></param>
        /// <param name="changePasswordRepository"></param>
        /// <param name="visitorRepository"></param>
        public ContactPersonService(CompanyGroup.Domain.PartnerModule.IContactPersonRepository contactPersonRepository, CompanyGroup.Domain.PartnerModule.IChangePasswordRepository changePasswordRepository, CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (contactPersonRepository == null)
            {
                throw new ArgumentNullException("ContactPersonRepository");
            }

            if (changePasswordRepository == null)
            {
                throw new ArgumentNullException("ChangePasswordRepository");
            }

            this.contactPersonRepository = contactPersonRepository;

            this.changePasswordRepository = changePasswordRepository;
        }

        /// <summary>
        /// kapcsolattartó lekérdezése azonosító alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.ContactPerson GetContactPersonById(CompanyGroup.Dto.ServiceRequest.GetContactPersonById request)
        {
            //ha üres a látogató azonosító
            CompanyGroup.Helpers.DesignByContract.Require( !String.IsNullOrEmpty(request.VisitorId), CompanyGroup.Domain.Resources.Messages.validationVisitorIdCannotBeNull);

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
        public CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.ServiceRequest.UndoChangePassword request)
        {
            //ha üres a látogató azonosító
            if (String.IsNullOrEmpty(request.VisitorId))
            {
                return new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validationVisitorIdCannotBeNull, Succeeded = false };
            }

            //látogató kikeresése
            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            if (String.IsNullOrEmpty(request.Id))
            {
                return new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Message = CompanyGroup.Domain.Resources.Messages.validation_ChangePasswordLogIdCannotBeNull, Succeeded = false };
            }

            CompanyGroup.Domain.PartnerModule.ChangePassword chagePassword = changePasswordRepository.GetItemByKey(request.Id);

            //jelszómódosítsítás művelet előkészítése
            CompanyGroup.Domain.PartnerModule.ChangePasswordCreate changePasswordCreate = new CompanyGroup.Domain.PartnerModule.ChangePasswordCreate()
            {
                ContactPersonId = visitor.PersonId,
                DataAreaId = chagePassword.DataAreaId,
                NewPassword = chagePassword.OldPassword,
                OldPassword = chagePassword.NewPassword,
                WebLoginName = chagePassword.UserName
            };
            //jelszómódosítás AX
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
        public CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.ServiceRequest.ChangePassword request)
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
                                                                                    Status = Domain.PartnerModule.ChangePasswordStatus.Active,
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

            string message = ( changePasswordCreateResult.Succeeded && !sendMailSucceeded ) ? CompanyGroup.Domain.Resources.Messages.verification_ChangePasswordMailSendFailed : changePasswordCreateResult.Message;

            return new CompanyGroup.Dto.PartnerModule.ChangePassword() { Message = message, OperationSucceeded = changePasswordCreateResult.Succeeded, SendMailSucceeded = sendMailSucceeded };
        }

        #region "jelszómódosítás mail küldés"

        private static readonly string ChangePasswordMailSubject = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailSubject", "Jelszó módosítás értesítő üzenet");

        private static readonly string ChangePasswordMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailHtmlTemplateFile", "changepassword.html");

        private static readonly string ChangePasswordMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailTextTemplateFile", "changepassword.txt");

        private static readonly string ChangePasswordMailFromAddress = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailFromAddress", "webadmin@hrp.hu"); 

        private static readonly string ChangePasswordMailFromName = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailFromName", "HRP-BSC web adminisztrátor csoport");

        private static readonly string ChangePasswordMailBCcAddress = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailBCcAddress", "ajuhasz@hrp.hu"); 

        private static readonly string ChangePasswordMailBCcName = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailBCcName", "Juhász Attila");

        private static readonly string ChangePasswordMailSmtpHost = Helpers.ConfigSettingsParser.GetString("ChangePasswordMailSmtpHost", "195.30.7.14");

        private static readonly string UndoChangePasswordUrl = Helpers.ConfigSettingsParser.GetString("UndoChangePasswordUrl", "http://1juhasza/cms/PartnerInfo/UndoChangePassword/{0}/{1}/");

        private static readonly bool TestMode = Helpers.ConfigSettingsParser.GetBoolean("TestMode", true);

        /// <summary>
        /// jelszómódosítás email küldés
        /// </summary>
        /// <param name="changePasswordCreate"></param>
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
                                        .Replace("$UndoChangePasswordUrl$", String.Format(ContactPersonService.UndoChangePasswordUrl, visitor.Id.ToString(), changePassword.Id.ToString()));

                string tmpPlain = ContactPersonService.PlainText(ContactPersonService.ChangePasswordMailTextTemplateFile);
                string plain = tmpPlain.Replace("$PersonName$", visitor.PersonName)
                                        .Replace("$NewPassword$", changePassword.NewPassword)
                                        .Replace("$OldPassword$", changePassword.OldPassword)
                                        .Replace("$WebLoginName$", changePassword.UserName)
                                        .Replace("$Date$", String.Format("{0}.{1}.{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                        .Replace("$Time$", String.Format("{0}.{1}.{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second))
                                        .Replace("$UndoChangePasswordUrl$", String.Format(ContactPersonService.UndoChangePasswordUrl, visitor.Id.ToString(), changePassword.Id.ToString()));

                string toAddress = ContactPersonService.TestMode ? ContactPersonService.ChangePasswordMailBCcAddress : visitor.Email;

                string toName = visitor.PersonName;

                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage(ContactPersonService.ChangePasswordMailSubject, plain, html);

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
                mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + toAddress + ">", toName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, String.Format("<{0}>", ContactPersonService.ChangePasswordMailFromAddress), ContactPersonService.ChangePasswordMailFromName, System.Text.Encoding.Default));

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, String.Format("<{0}>", ContactPersonService.ChangePasswordMailBCcAddress), ContactPersonService.ChangePasswordMailBCcName, System.Text.Encoding.Default));

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
                mailSender.SmtpHost = ChangePasswordMailSmtpHost;
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
    }
}
