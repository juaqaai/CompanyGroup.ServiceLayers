using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain product -> DTO product 
    /// </summary>
    public class BannerProductToBannerProduct
    {
        /// <summary>
        /// domain product -> dto product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.BannerProduct Map(CompanyGroup.Domain.WebshopModule.BannerProduct from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.BannerProduct()
                           {
                               Currency = from.Prices.Currency,
                               ItemName = from.ItemName,
                               ItemNameEnglish = from.ItemNameEnglish,
                               PartNumber = from.PartNumber,
                               PictureId = from.Picture.Id, //new PictureToPicture().Map(from.Picture),  
                               Price = String.Format( "{0}", from.CustomerPrice),
                               ProductId = from.ProductId,
                               DataAreaId = from.DataAreaId, 
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.BannerProduct(); }
        }
    }
}
