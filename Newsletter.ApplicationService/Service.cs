using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Newsletter.ApplicationService
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true),
                     System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    [Newsletter.ApplicationService.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class Service : IService
    {
        private static readonly bool testMode = CompanyGroup.Helpers.ConfigSettingsParser.GetInt("TestMode", 1).Equals(1);

        private static readonly string testModeMailAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("TestModeMailAddress", "ajuhasz@hrp.hu");

        private static readonly int SendStrategy = CompanyGroup.Helpers.ConfigSettingsParser.GetInt("SendStrategy", 2);

        private Newsletter.Repository.ISendOut sendoutRepository;

        private Newsletter.Repository.IRecipient recipientRepository;

        private SendContext context = null;

        /// <summary>
        /// context létrehozás
        /// </summary>
        private void CreateContext() 
        {
            SendStrategies sendStrategy = (SendStrategies) SendStrategy;

            context = new SendContext(sendStrategy); 
        }

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public Service(Newsletter.Repository.ISendOut sendoutRepository, Newsletter.Repository.IRecipient recipientRepository)
        {
            if (sendoutRepository == null)
            {
                throw new ArgumentNullException("sendoutRepository");
            }

            if (recipientRepository == null)
            {
                throw new ArgumentNullException("recipientRepository");
            }

            this.sendoutRepository = sendoutRepository;

            this.recipientRepository = recipientRepository;
        }

        /// <summary>
        ///  vevő levelezési cím lista kiolvasása vevőazonosító és vállalatkód alapján  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Send(string id)
        {
            int headerId = 0;
            bool testMode = true;
            try
            {
                headerId = CompanyGroup.Helpers.ConvertData.ConvertStringToInt(id, 0);

                if (headerId == 0) { return 0; }

                CreateContext();

                List<Dto.Newsletter> newsletterList = sendoutRepository.GetList();

                Dto.Newsletter newsletter = newsletterList.FirstOrDefault(x => x.HeaderId == headerId);

                    string sharePathPrefix = GetAbsolutePathPrefix(newsletter.DataAreaId);

                    CompanyGroup.Helpers.FileReader fileReader = new CompanyGroup.Helpers.FileReader(String.Format("{0}{1}", sharePathPrefix, newsletter.HtmlPath), newsletter.DecodingId);

                    string plain = newsletter.Subject;

                    string html = fileReader.ReadToEnd();

                    //címzettlista
                    List<Dto.Recipient> recipients = recipientRepository.GetRecipientList(newsletter.HeaderId);

                    recipients.ForEach(delegate(Dto.Recipient recipient)
                    {
                        testMode = recipient.Type.Equals(0);

                        this.SendEmail(newsletter.Subject, plain, html, newsletter.DecodingId, newsletter.SenderEmail, newsletter.SenderName, recipient, newsletter.DataAreaId);

                        int delayBetweenSending = CompanyGroup.Helpers.ConfigSettingsParser.GetInt("DelayBetweenSending", 500);

                        if (delayBetweenSending > 0)
                        {
                            System.Threading.Thread.Sleep(delayBetweenSending);
                        }
                    });

                    //extra címlistára csak akkor kell küldeni, ha élesben megy a kiküldés
                    if (!testMode)
                    {
                        List<Dto.Address> extraAddressList = recipientRepository.GetExtraAddressList(newsletter.DataAreaId);

                        extraAddressList.ForEach(delegate(Dto.Address extraAddress)
                        {
                            Dto.Recipient recipient = new Dto.Recipient(0, extraAddress.Name, extraAddress.Email, 4);

                            this.SendEmail(newsletter.Subject, plain, html, newsletter.DecodingId, newsletter.SenderEmail, newsletter.SenderName, recipient, newsletter.DataAreaId);
                        });
                    }

                    //hírlevél fejléc beallitas kiküldött státuszra
                    sendoutRepository.SetHeader(newsletter.HeaderId, 2);

                return 1;
            }
            catch (Exception ex)
            {
                sendoutRepository.SetDetail(headerId, -1, String.Format("Nincs kiküldve: Message: {0} StackTrace: {1}", ex.Message, ex.StackTrace));

                return 0;
            }
            finally
            {
                this.context.Dispose();
            }
        }

        /// <summary>
        /// visszaadja a hírlevél konyvtár megosztást abszolút url-ként, vállalatfüggően.
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        private string GetAbsolutePathPrefix(string dataAreaId)
        {
            if (dataAreaId.Equals("hrp")) 
            {
                return CompanyGroup.Helpers.ConfigSettingsParser.GetString("AbsolutePathPrefix_Hrp", @"\\hrpinternet\ArticleSources\");
            }
            else if (dataAreaId.Equals("bsc"))
            {
                return CompanyGroup.Helpers.ConfigSettingsParser.GetString("AbsolutePathPrefix_Bsc", @"\\hrpinternet\Bsc-Articles\");
            }
            else if (dataAreaId.Equals("ser"))
            {
                return CompanyGroup.Helpers.ConfigSettingsParser.GetString("AbsolutePathPrefix_Serbia", @"\\hrpinternet\SerbianArticles\");
            }
            return CompanyGroup.Helpers.ConfigSettingsParser.GetString("AbsolutePathPrefix_Hrp", @"\\hrpinternet\ArticleSources\");
        }
         
        /// <summary>
        /// email értesítés küldése egy hírlevélre feliratkozott címzett számára
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="plain"></param>
        /// <param name="html"></param>
        /// <param name="encoding"></param>
        /// <param name="senderEmail"></param>
        /// <param name="senderName"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        private void SendEmail(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId) 
        {
            try
            {
                this.context.Send(subject, plain, html, encoding, senderEmail, senderName, recipient, dataAreaId);

                if (recipient.DetailId > 0)
                {
                    sendoutRepository.SetDetail(recipient.DetailId, 2, "kiküldve");
                }
            }
            catch(Exception ex)
            {
                if (recipient.DetailId > 0)
                {
                    sendoutRepository.SetDetail(recipient.DetailId, -1, String.Format("Nincs kiküldve: {0}{1}", ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
