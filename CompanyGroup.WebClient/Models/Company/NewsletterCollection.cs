using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// hírlevél model 
    /// </summary>
    public class Newsletter
    {
        public Newsletter(CompanyGroup.Dto.WebshopModule.NewsletterCollection newsletterCollection)
        {
            this.Items = newsletterCollection.Items;

            this.Visitor = new Visitor(newsletterCollection.Visitor);
        }

        public List<CompanyGroup.Dto.WebshopModule.Newsletter> Items { get; set; }

        public Visitor Visitor { get; set; }
    }
}
