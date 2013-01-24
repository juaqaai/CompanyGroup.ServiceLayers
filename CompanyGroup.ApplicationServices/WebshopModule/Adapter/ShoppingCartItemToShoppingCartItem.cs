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
                DataAreaId = from.DataAreaId,   //CompanyGroup.Domain.Core.Adapter.ConvertDataAreaIdEnumToString
                //Flags = new FlagsToFlags().Map(from.Flags),
                //Garanty = new GarantyToGaranty().Map(from.Garanty),
                Id = from.CartId,
                //IsInStock = from.IsInStock,
                ItemState = (int)from.ItemState,
                ItemTotal = from.ItemTotal,
                PartNumber = from.PartNumber,
                ProductId = from.ProductId,
                //PurchaseInProgress = from.PurchaseInProgress(),
                Quantity = from.Quantity, 
                ProductName = from.ProductName,
                //ShippingDate = from.ShippingDate,
                Stock = new StockToStock().Map(from.Stock)
            };
        }
    }
}
