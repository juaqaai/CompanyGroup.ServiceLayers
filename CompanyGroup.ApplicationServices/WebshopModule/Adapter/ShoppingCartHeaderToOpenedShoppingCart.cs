using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain bevásárlókosár fejléc -> DTO nyitott bevásárlókosár fejléc
    /// </summary>
    public class ShoppingCartHeaderToOpenedShoppingCart
    {
        public CompanyGroup.Dto.WebshopModule.OpenedShoppingCart Map(CompanyGroup.Domain.WebshopModule.ShoppingCartHeader from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.OpenedShoppingCart()
                {
                    Id = from.Id,
                    Name = from.Name,
                    Active = from.Active
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.OpenedShoppingCart(); }
        }
    }
}
