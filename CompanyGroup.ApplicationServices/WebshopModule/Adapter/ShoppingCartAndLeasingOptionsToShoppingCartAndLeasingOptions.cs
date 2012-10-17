using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain ShoppingCart -> DTO ShoppingCart 
    /// </summary>
    public class ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions
    {
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions Map(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart, CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions()
                {
                    Items = shoppingCart.Items.ConvertAll(x => new ShoppingCartItemToShoppingCartItem().Map(x)),
                    SumTotal = shoppingCart.SumTotal,
                    Id = shoppingCart.Id.ToString(),
                    DeliveryTerms = Convert.ToInt32(shoppingCart.DeliveryTerms),
                    PaymentTerms = Convert.ToInt32(shoppingCart.PaymentTerms),
                    Shipping = new ShippingToShipping().Map(shoppingCart.Shipping), 
                    LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions)
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions(); }
        }

    }
}
