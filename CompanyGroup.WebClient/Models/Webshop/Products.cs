using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class Products : CompanyGroup.Dto.WebshopModule.Products
    {
        public Products(CompanyGroup.Dto.WebshopModule.Products products)
        {
            this.Currency = products.Currency;

            this.Items = products.Items;

            this.ListCount = products.ListCount;

            this.Pager = products.Pager;
        }
    }
}