using System;
using System.Collections.Generic;

namespace Newsletter.ApplicationService
{
    /// <summary>
    /// alapértelmezés szerinti implementáció
    /// </summary>
    public class DefaultStrategy : BaseStrategy, ISendStrategy, IDisposable
    {
        public void Send(string subject, string plain, string html, int encoding, string senderEmail, string senderName, Newsletter.Dto.Recipient recipient, string dataAreaId)
        {
        
        }

        public void Dispose()
        { 
        
        }
    }
}
