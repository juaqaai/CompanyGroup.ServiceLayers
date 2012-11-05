using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ShoppingCartCollection : CompanyGroup.Dto.WebshopModule.ShoppingCartCollection
    {
        public ShoppingCartCollection() : base() 
        {
            this.Carts = new List<CompanyGroup.Dto.WebshopModule.ShoppingCart>();
        }
    }
}
