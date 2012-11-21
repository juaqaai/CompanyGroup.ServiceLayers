using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ProductCatalogue  
    {
        public ProductCatalogue(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Products = products;

            this.Visitor = visitor;
        }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

    }
}
