using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection
    {
        /// <summary>
        /// Domain ShoppingCartHeaderCollection -> StoredOpenedShoppingCartCollection DTO 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection Map(CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection from)
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> openedCarts = from.OpenedShoppingCartHeaders;

                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> storedCarts = from.StoredShoppingCartHeaders;

                return new CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection()
                {
                    StoredItems = storedCarts.ConvertAll(x => new ShoppingCartHeaderToStoredShoppingCart().Map(x)), 
                    OpenedItems = openedCarts.ConvertAll(x => new ShoppingCartHeaderToOpenedShoppingCart().Map(x))
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection(); }
        }

    }
}
