using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain bevásárlókosár fejléc -> DTO tárolt bevásárlókosár fejléc
    /// </summary>
    public class ShoppingCartHeaderToStoredShoppingCart
    {
        public CompanyGroup.Dto.WebshopModule.StoredShoppingCart Map(CompanyGroup.Domain.WebshopModule.ShoppingCartHeader from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.StoredShoppingCart()
                {
                    Id = from.Id,
                    Name = from.Name,
                    Active = from.Active
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.StoredShoppingCart(); }
        }
    }
}
