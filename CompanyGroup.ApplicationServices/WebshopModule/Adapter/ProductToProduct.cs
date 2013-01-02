using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
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
        public CompanyGroup.Dto.WebshopModule.Product Map(CompanyGroup.Domain.WebshopModule.Product product)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.Product()
                           {
                               Available = product.Available, 
                               Comparable = product.Comparable, 
                               Currency = product.Prices.Currency,
                               DataAreaId = product.DataAreaId, // CompanyGroup.Domain.Core.Adapter.ConvertDataAreaIdEnumToString(
                               Description = product.Description,
                               DescriptionEnglish = product.DescriptionEnglish,
                               Discount = product.Discount,
                               EndOfSales = CompanyGroup.Domain.Core.Adapter.ConvertItemStateEnumToBool(product.ItemState),
                               FirstLevelCategory = new CategoryToCategory().Map(product.Structure.Category1),
                               GarantyMode = product.Garanty.Mode,
                               GarantyTime = product.Garanty.Time,
                               IsInNewsletter = product.IsInNewsletter,
                               InnerStock = product.Stock.Inner,
                               OuterStock = product.Stock.Outer,
                               IsInStock = product.IsInStock,
                               IsInCart = product.IsInCart,
                               ItemName = product.ProductName,
                               ItemNameEnglish = product.ProductNameEnglish, 
                               Manufacturer = new ManufacturerToManufacturer().Map(product.Structure.Manufacturer),
                               New = product.New,
                               PartNumber = product.PartNumber,
                               PictureId = product.PictureId, //new PicturesToPictures().Map(product.Pictures),
                               //PrimaryPicture = new PictureToPicture().Map(product.PrimaryPicture),  
                               Price = String.Format( "{0}", product.CustomerPrice),
                               ProductId = product.ProductId,
                               PurchaseInProgress = product.PurchaseInProgress(), 
                               SecondLevelCategory = new CategoryToCategory().Map(product.Structure.Category2),
                               SecondHandList = new SecondHandToSecondHand().Map(product.SecondHandList),
                               ShippingDate = String.Format("{0}.{1}.{2}", product.ShippingDate.Year, product.ShippingDate.Month, product.ShippingDate.Day),
                               ThirdLevelCategory = new CategoryToCategory().Map(product.Structure.Category3), 
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Product(); }
        }
    }
}
