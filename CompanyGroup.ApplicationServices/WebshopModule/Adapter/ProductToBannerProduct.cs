using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain product -> DTO product 
    /// </summary>
    public class ProductToBannerProduct
    {
        /// <summary>
        /// domain product -> dto product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.BannerProduct Map(CompanyGroup.Domain.WebshopModule.Product product)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.BannerProduct()
                           {
                               Currency = product.Prices.Currency,
                               ItemName = product.ProductName,
                               ItemNameEnglish = product.ProductNameEnglish, 
                               PartNumber = product.PartNumber,
                               PrimaryPicture = new PictureToPicture().Map(product.PrimaryPicture),  
                               Price = String.Format( "{0}", product.CustomerPrice),
                               ProductId = product.ProductId, 
                               DataAreaId = product.DataAreaId
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.BannerProduct(); }
        }
    }
}
