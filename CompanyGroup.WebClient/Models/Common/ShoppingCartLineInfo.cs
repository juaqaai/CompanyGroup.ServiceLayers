using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ShoppingCartLineInfo
    {
        public ShoppingCartLineInfo(Visitor visitor, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCartAndLeasingOptions)
        {
            this.Visitor = visitor;

            this.ShoppingCartAndLeasingOptions = shoppingCartAndLeasingOptions;
        }

        public Visitor Visitor { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions ShoppingCartAndLeasingOptions { get; set; }
    }
}