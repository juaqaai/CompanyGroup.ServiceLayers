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
                List<CompanyGroup.Dto.WebshopModule.ShoppingCartItem> items = shoppingCart.Items.ToList().ConvertAll(x => new ShoppingCartItemToShoppingCartItem().Map(x, shoppingCart.Currency));

                CompanyGroup.Dto.WebshopModule.Shipping shipping = new ShippingToShipping().Map(shoppingCart.Shipping);

                CompanyGroup.Dto.WebshopModule.LeasingOptions options = new LeasingOptionsToLeasingOptions().Map(leasingOptions);

                return new CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions(items, 
                                                                                        shoppingCart.SumTotal, 
                                                                                        shoppingCart.Id, 
                                                                                        shipping, 
                                                                                        Convert.ToInt32(shoppingCart.PaymentTerms), 
                                                                                        Convert.ToInt32(shoppingCart.DeliveryTerms), 
                                                                                        options, 
                                                                                        shoppingCart.Currency, 
                                                                                        shoppingCart.AllInStock);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions(); }
        }

    }
}
