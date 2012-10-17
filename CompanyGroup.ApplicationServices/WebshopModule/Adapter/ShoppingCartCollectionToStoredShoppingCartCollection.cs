using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ShoppingCartCollectionToStoredOpenedShoppingCartCollection
    {

        public CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection Map(CompanyGroup.Domain.WebshopModule.ShoppingCartCollection from)
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> openedCarts = from.Carts.FindAll(x => x.Status.Equals(CartStatus.Created));

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> storedCarts = from.Carts.FindAll(x => x.Status.Equals(CartStatus.Stored));

                return new CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection()
                {
                    StoredItems = storedCarts.ConvertAll(x => new ShoppingCartToStoredShoppingCart().Map(x)), 
                    OpenedItems = openedCarts.ConvertAll(x => new ShoppingCartToOpenedShoppingCart().Map(x))
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection(); }
        }

    }
}
