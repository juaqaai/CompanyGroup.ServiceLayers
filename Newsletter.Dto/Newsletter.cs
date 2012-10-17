using System;
using System.Collections.Generic;

namespace Newsletter.Dto
{
    public class Newsletter
    {
        public Newsletter( int headerId, string dataAreaId, string newsletterId, string senderName, string senderEmail, int decodingId, string htmlPath, string subject)
        {
            this.HeaderId = headerId;

            this.DataAreaId = dataAreaId;

            this.NewsletterId = newsletterId;

            this.SenderName = senderName;

            this.SenderEmail = senderEmail;

            this.DecodingId = decodingId;

            this.HtmlPath = htmlPath;

            this.Subject = subject;
        }

        public int HeaderId { get; set; }

        public string DataAreaId { get; set; }

        public string NewsletterId { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public int DecodingId { get; set; }

        public string HtmlPath { get; set; }

        public string Subject { get; set; }
    }
}
