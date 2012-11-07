using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class Products
    {
        public Products(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Products = products;

            this.ListCount = products.Items.Count;

            this.Visitor = visitor;
        }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        public long ListCount { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

    }
}
