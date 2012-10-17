using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain product -> DTO product 
    /// </summary>
    public class ProductToCompatibleProduct
    {
        /// <summary>
        /// domain product -> dto compatible product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.CompatibleProduct Map(CompanyGroup.Domain.WebshopModule.Product product)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.CompatibleProduct()
                           {
                               Currency = product.Prices.Currency,
                               DataAreaId = product.DataAreaId, 
                               Description = product.Description,
                               DescriptionEnglish = product.DescriptionEnglish,
                               InnerStock = product.Stock.Inner,
                               OuterStock = product.Stock.Outer,
                               IsInStock = product.IsInStock,
                               ItemName = product.ProductName,
                               ItemNameEnglish = product.ProductNameEnglish, 
                               PartNumber = product.PartNumber,
                               Price = String.Format( "{0}", product.CustomerPrice),
                               ProductId = product.ProductId,
                               PurchaseInProgress = product.PurchaseInProgress(), 
                               ShippingDate = product.ShippingDate,
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.CompatibleProduct(); }
        }
    }
}
