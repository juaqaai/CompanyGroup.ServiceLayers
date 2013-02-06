using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// bevásárlókosár kollekció, melyben egy vásárlóhoz (visitor) több bevásárlókosár tartozhat
    /// </summary>
    public class ShoppingCartCollection // : CompanyGroup.Domain.Core.Entity, IValidatableObject
    {
        private const int MAX_SHOPPING_CART_COUNT = 10;

        /// <summary>
        /// kosár létrejötte
        /// </summary>
        public ShoppingCartCollection(List<ShoppingCart> carts)
        {
            this.Carts = carts; 
        }

        public List<ShoppingCart> Carts { get; set; }

        /// <summary>
        /// van-e a elem a kosár kollekcióban ?  
        /// </summary>
        public bool ExistsItem
        {
            get { return (this.Carts.Count > 0); }
        }

        /// <summary>
        /// létrehozható-e további kosár, vagy nem (fegfeljebb 10 kosár hozható létre)
        /// </summary>
        public bool EnableCreateCart
        {
            get { return this.Carts.Count < MAX_SHOPPING_CART_COUNT; }
        }

        /// <summary>
        /// kosár kiolvasása azonosító alapján
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShoppingCart GetCart(string id)
        {
            CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The CartId parameter cannot be null!");

            int index = this.Carts.FindIndex(x => x.Id.Equals(id));

            return (index != -1) ? this.Carts[index] : null;
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <returns></returns>
        public ShoppingCart GetActiveCart()
        {
            int index = this.Carts.FindIndex(x => x.Active);

            return (index != -1) ? this.Carts[index] : new ShoppingCart();
        }

        /// <summary>
        /// benne van-e a kosarak közül az egyik kosárba a termék?
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool IsInCart(string productId)
        {
            if (String.IsNullOrEmpty(productId))
            {
                return false;
            }

            return this.Carts.Exists(x => x.IsInCart(productId));
        }
    }
}
