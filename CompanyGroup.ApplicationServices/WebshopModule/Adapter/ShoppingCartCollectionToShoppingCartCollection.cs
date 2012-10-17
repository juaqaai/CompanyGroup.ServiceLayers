using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ShoppingCartCollectionToShoppingCartCollection
    {

        public CompanyGroup.Dto.WebshopModule.ShoppingCartCollection Map(CompanyGroup.Domain.WebshopModule.ShoppingCartCollection from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.ShoppingCartCollection()
                {
                    Carts = from.Carts.ConvertAll(x => new ShoppingCartToShoppingCart().Map(x)),
                    ActiveCartId = from.GetActiveCart().Id.ToString()
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartCollection(); }
        }

    }
}
