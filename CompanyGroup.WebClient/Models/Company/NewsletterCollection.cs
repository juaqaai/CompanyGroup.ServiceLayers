using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class Newsletter
    {
        public Newsletter(CompanyGroup.Dto.WebshopModule.NewsletterCollection newsletterCollection, Visitor visitor)
        {
            this.Items = newsletterCollection.Items;

            this.Visitor = visitor;
        }

        public List<CompanyGroup.Dto.WebshopModule.Newsletter> Items { get; set; }

        public Visitor Visitor { get; set; }
    }
}
