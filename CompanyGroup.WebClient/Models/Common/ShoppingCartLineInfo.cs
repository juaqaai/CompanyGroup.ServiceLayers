using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// visitor + ShoppingCart + LeasingOptions
    /// </summary>
    public class ShoppingCartLineInfo
    {
        public ShoppingCartLineInfo(Visitor visitor, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCartAndLeasingOptions)
        {
            this.Visitor = visitor;

            this.ActiveCart = new Dto.WebshopModule.ShoppingCart(shoppingCartAndLeasingOptions.Items, 
                                                                 shoppingCartAndLeasingOptions.SumTotal, 
                                                                 shoppingCartAndLeasingOptions.Id, 
                                                                 shoppingCartAndLeasingOptions.Shipping, 
                                                                 shoppingCartAndLeasingOptions.PaymentTerms, 
                                                                 shoppingCartAndLeasingOptions.DeliveryTerms);

            this.LeasingOptions = shoppingCartAndLeasingOptions.LeasingOptions;
        }

        public Visitor Visitor { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }
}