using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class Products
    {
        public Products(CompanyGroup.Dto.WebshopModule.Products items, CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Items = items;

            this.Visitor = visitor;
        }

        public CompanyGroup.Dto.WebshopModule.Products Items { get; set; }

        public long ListCount { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

    }
}
