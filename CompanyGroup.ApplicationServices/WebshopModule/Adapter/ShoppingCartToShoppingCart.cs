using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain ShoppingCart -> DTO ShoppingCart 
    /// </summary>
    public class ShoppingCartToShoppingCart
    {
        public CompanyGroup.Dto.WebshopModule.ShoppingCart Map(CompanyGroup.Domain.WebshopModule.ShoppingCart from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.ShoppingCart()
                {
                    Items = from.Items.ToList().ConvertAll(x => new ShoppingCartItemToShoppingCartItem().Map(x, from.Currency)),
                    SumTotal = from.SumTotal, 
                    Id = from.Id, 
                    DeliveryTerms = Convert.ToInt32(from.DeliveryTerms),
                    PaymentTerms =  Convert.ToInt32(from.PaymentTerms), 
                    Shipping =  new ShippingToShipping().Map(from.Shipping)
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCart(); }
        }

    }
}
