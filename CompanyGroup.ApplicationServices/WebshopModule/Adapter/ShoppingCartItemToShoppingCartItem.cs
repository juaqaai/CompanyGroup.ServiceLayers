using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ShoppingCartItemToShoppingCartItem
    {
        /// <summary>
        /// domain ShoppingCartItem -> dto ShoppingCartItem
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartItem Map(CompanyGroup.Domain.WebshopModule.ShoppingCartItem from, string currency)
        {
            return new CompanyGroup.Dto.WebshopModule.ShoppingCartItem()
            {
                Currency = currency,
                CustomerPrice = from.CustomerPrice,
                DataAreaId = from.DataAreaId,
                Id = from.LineId,
                ItemState = (int)from.ItemState,
                ItemTotal = from.ItemTotal,
                PartNumber = from.PartNumber,
                ProductId = from.ProductId,
                Quantity = from.Quantity, 
                ProductName = from.ProductName,
                Stock = new StockToStock().Map(from.Stock)
            };
        }
    }
}
