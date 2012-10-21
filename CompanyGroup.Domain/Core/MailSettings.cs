using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.Core
{
    public class MailSettings
    {
        public MailSettings(string smtpHost, string subject, string plainText, string htmlText, string fromName, string fromAddress)
        {
            Subject = subject;

            PlainText = plainText;

            HtmlText = htmlText;

            SmtpHost = smtpHost;

            this.FromName = fromName;

            this.FromAddress = fromAddress;

            this.ToAddressList = new MailAddressList();

            this.BccAddressList = new MailAddressList();
        }

        public string Subject {get; set;}

        public string FromName { get; set; }

        public string FromAddress { get; set; }

        public MailAddressList ToAddressList { get; set; }  
        
        public MailAddressList BccAddressList { get; set; }  
        
        public string PlainText {get; set;}

        public string HtmlText { get; set; } 

        public string SmtpHost {get; set;} 
    }

    public class MailAddressList
    {
        public System.Collections.Generic.Dictionary<string, string> Addresses { get; private set; }

        public MailAddressList()
        {
            this.Addresses = new Dictionary<string, string>();
        }

        public void Add(string name, string address)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(address)) { return; }

            if (!this.Addresses.ContainsKey(address))
            {
                this.Addresses.Add(address, name);
            }
        }

    }
}
