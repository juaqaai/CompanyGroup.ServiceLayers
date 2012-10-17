using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    /// <summary>
    /// Domain product -> DTO product 
    /// </summary>
    public class ProductToProduct
    {
        /// <summary>
        /// domain product -> dto product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.GlobalServices.Dto.Product Map(CompanyGroup.Dto.WebshopModule.Product product)
        {
            try
            {
                decimal price = 0;

                return new CompanyGroup.GlobalServices.Dto.Product()
                           {
                               CannotCancel = product.CannotCancel,
                               DataAreaId = product.DataAreaId,
                               Description = product.Description,
                               EndOfSales = product.EndOfSales,
                               FirstLevelCategory = new CategoryToCategory().Map(product.FirstLevelCategory),
                               GarantyMode = product.GarantyMode,
                               GarantyTime = product.GarantyTime,
                               IsInNewsletter = product.IsInNewsletter,
                               Stock = product.InnerStock + product.OuterStock,
                               ItemName = product.ItemName,
                               Manufacturer = new ManufacturerToManufacturer().Map(product.Manufacturer),
                               New = product.New,
                               PartNumber = product.PartNumber,
                               Pictures = new PicturesToPictures().Map(product.Pictures),
                               Price = decimal.TryParse(product.Price, out price) ? price : 0,
                               ProductId = product.ProductId,
                               PurchaseInProgress = product.PurchaseInProgress,
                               SecondLevelCategory = new CategoryToCategory().Map(product.SecondLevelCategory),
                               ShippingDate = product.ShippingDate,
                               ThirdLevelCategory = new CategoryToCategory().Map(product.ThirdLevelCategory),
                           };
            }
            catch { return new CompanyGroup.GlobalServices.Dto.Product(); }
        }
    }
}
