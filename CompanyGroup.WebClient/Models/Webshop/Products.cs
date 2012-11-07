using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class Products : CompanyGroup.Dto.WebshopModule.Products
    {
        public Products(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Currency = products.Currency;

            this.Items = products.Items;

            this.ListCount = products.ListCount;

            this.Pager = products.Pager;

            this.Visitor = visitor;
        }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

    }
}
