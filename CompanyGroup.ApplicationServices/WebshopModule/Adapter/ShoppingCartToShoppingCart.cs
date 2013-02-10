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
        /// <summary>
        /// kosár sorokat domain típusról DTO típusra konvertálja
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCart Map(CompanyGroup.Domain.WebshopModule.ShoppingCart from)
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem> items = from.Items.ToList().Where(x => x.LineId > 0).ToList();

                return new CompanyGroup.Dto.WebshopModule.ShoppingCart()
                {
                    Id = from.Id, 
                    SumTotal = from.SumTotal, 
                    DeliveryTerms = Convert.ToInt32(from.DeliveryTerms),
                    PaymentTerms =  Convert.ToInt32(from.PaymentTerms), 
                    Shipping =  new ShippingToShipping().Map(from.Shipping),
                    Items = items.ConvertAll(x => new ShoppingCartItemToShoppingCartItem().Map(x, from.Currency)),
                };
            }
            catch(Exception ex) { throw ex; }
        }

    }
}
