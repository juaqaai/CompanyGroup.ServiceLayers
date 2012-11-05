﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class NewsletterCollection : CompanyGroup.Dto.WebshopModule.NewsletterCollection
    {
        public NewsletterCollection(CompanyGroup.Dto.WebshopModule.NewsletterCollection newsletterCollection)
        {
            this.Items = newsletterCollection.Items;
        }
    }
}
