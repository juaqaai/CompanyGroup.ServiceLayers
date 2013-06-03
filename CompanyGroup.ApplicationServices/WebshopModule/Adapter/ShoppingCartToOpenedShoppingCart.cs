using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain bevásárlókosár -> DTO bevásárlókosár 
    /// </summary>
    public class ShoppingCartToOpenedShoppingCart
    {
        public CompanyGroup.Dto.WebshopModule.OpenedShoppingCart Map(CompanyGroup.Domain.WebshopModule.ShoppingCart from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.OpenedShoppingCart()
                {
                    Id = from.Id.ToString(),
                    Name = from.Name,
                    Active = from.Active
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.OpenedShoppingCart(); }
        }
    }
}
