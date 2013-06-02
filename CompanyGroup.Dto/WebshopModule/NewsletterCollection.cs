using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// hírlevél elem
    /// </summary>
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

        public string NewsletterId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string HtmlPath { get; set; }

        public string EndDateTime { get; set; }

         public string PicturePath { get; set; }

        public string AllowedDateTime { get; set; }

        public string Body { get; set; }
    }

    /// <summary>
    /// hírlevél lekérdezése eredménye
    /// </summary>
    public class NewsletterCollection
    {
        public NewsletterCollection() : this(new List<Newsletter>(), new CompanyGroup.Dto.PartnerModule.Visitor()) { }

        public NewsletterCollection(List<Newsletter> items, CompanyGroup.Dto.PartnerModule.Visitor visitor)
        {
            this.Items = items;

            this.Visitor = visitor;
        }

        public List<Newsletter> Items { get; set; }

        public CompanyGroup.Dto.PartnerModule.Visitor Visitor { get; set; }
    }
}
