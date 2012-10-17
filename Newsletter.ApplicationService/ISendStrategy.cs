using System;
using System.Collections.Generic;

namespace Newsletter.ApplicationService
{
    /// <summary>
    /// kiküldési stratégia interfész
    /// </summary>
    public interface ISendStrategy
    {
        /// <summary>
        /// küldés művelete 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="plain"></param>
        /// <param name="html"></param>
        /// <param name="encoding"></param>
        /// <param name="senderEmail"></param>
        /// <param name="senderName"></param>
        /// <param name="recipient"></param>
        /// <param name="dataAreaId"></param>
        void Send(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId);

        /// <summary>
        /// smtp kliens törlése
        /// </summary>
        void Dispose();
    }
}
