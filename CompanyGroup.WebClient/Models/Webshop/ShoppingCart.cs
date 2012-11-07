using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class ShoppingCart : CompanyGroup.Dto.WebshopModule.ShoppingCart
    {
        public ShoppingCart(CompanyGroup.Dto.WebshopModule.ShoppingCart shoppingCart)
        {
            this.DeliveryTerms = shoppingCart.DeliveryTerms;
            this.Id = shoppingCart.Id;
            this.Items = shoppingCart.Items;
            this.PaymentTerms = shoppingCart.PaymentTerms;
            this.Shipping = shoppingCart.Shipping;
            this.SumTotal = shoppingCart.SumTotal;
        }
    }
}