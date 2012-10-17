﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain product -> DTO product 
    /// </summary>
    public class ProductToProductDetails
    {
        /// <summary>
        /// domain product -> dto product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ProductDetails Map(CompanyGroup.Domain.WebshopModule.Product product,
                                                                 List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> compatibilityList,
                                                                 List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> reverseCompatibilityList)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.ProductDetails()
                           {
                               CannotCancel = product.CannotCancel,
                               Comparable = product.Comparable, 
                               Currency = product.Prices.Currency,
                               DataAreaId = product.DataAreaId, // CompanyGroup.Domain.Core.Adapter.ConvertDataAreaIdEnumToString(
                               Description = product.Description,
                               DescriptionEnglish = product.DescriptionEnglish,
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
                               Pictures = new PicturesToPictures().Map(product.Pictures),
                               PrimaryPicture = new PictureToPicture().Map(product.PrimaryPicture),  
                               Price = String.Format( "{0}", product.CustomerPrice),
                               ProductId = product.ProductId,
                               ProductManager = new ProductManagerToProductManager().Map(product.ProductManager),   
                               PurchaseInProgress = product.PurchaseInProgress(), 
                               SecondLevelCategory = new CategoryToCategory().Map(product.Structure.Category2),
                               SecondHandList = new SecondHandToSecondHand().Map(product.SecondHandList),
                               SequenceNumber = product.SequenceNumber, 
                               ShippingDate = product.ShippingDate,
                               ThirdLevelCategory = new CategoryToCategory().Map(product.Structure.Category3),
                               CompatibilityList = compatibilityList.ToArray(), 
                               ReverseCompatibilityList = reverseCompatibilityList.ToArray()
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ProductDetails(); }
        }
    }
}
