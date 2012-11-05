using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Newsletter", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Newsletter
    {
        public Newsletter()
        {
            this.NewsletterId = String.Empty;
            this.Title = String.Empty;
            this.Description = String.Empty;
            this.HtmlPath = String.Empty;
            this.EndDateTime = String.Empty;
            this.PicturePath = String.Empty;
            this.AllowedDateTime = String.Empty;
            this.Body = String.Empty;
        }

        [System.Runtime.Serialization.DataMember(Name = "NewsletterId", Order = 1)]
        public string NewsletterId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Title", Order = 2)]
        public string Title { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Description", Order = 3)]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "HtmlPath", Order = 4)]
        public string HtmlPath { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "EndDateTime", Order = 5)]
        public string EndDateTime { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PicturePath", Order = 6)]
        public string PicturePath { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "AllowedDateTime", Order = 7)]
        public string AllowedDateTime { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Body", Order = 8)]
        public string Body { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "NewsletterCollection", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class NewsletterCollection
    {
        public NewsletterCollection()
        {
            this.Items = new List<Newsletter>();
        }

        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<Newsletter> Items { get; set; }
    }
}
