using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newsletter.ApplicationService
{
    /// <summary>
    /// kiküldés kontextusa, konstruktorban kapott paraméter dönti el, hogy melyik kiküldési stratégiát kell használni
    /// </summary>
    public class SendContext
    {
        /// <summary>
        /// kiküldés stratégiája (Higlabo, vagy MailMerge)
        /// </summary>
        /// <param name="strategy"></param>
        public SendContext(SendStrategies strategy )
        {
            if (strategy == SendStrategies.Higlabo)
            {
                sendStrategy = new HigLaboStrategy();
            }
            else if (strategy == SendStrategies.MailMerge)
            {
                sendStrategy = new MailMergeStrategy();
            }
        }
        
        /// <summary>
        /// stratégia interfész
        /// </summary>
        private ISendStrategy sendStrategy;

        /// <summary>
        /// kiküldés stratégia szerint
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="plain"></param>
        /// <param name="html"></param>
        /// <param name="encoding"></param>
        /// <param name="senderEmail"></param>
        /// <param name="senderName"></param>
        /// <param name="recipient"></param>
        /// <param name="dataAreaId"></param>
        public void Send(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId)
        { 
            sendStrategy.Send(subject, plain, html, encoding, senderEmail, senderName, recipient, dataAreaId);
        }

        /// <summary>
        /// kliens törlése stratégia szerint
        /// </summary>
        public void Dispose()
        {
            sendStrategy.Dispose();
        }
    }
}
