using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public interface IProductRepository
    {
        List<CompanyGroup.Domain.MaintainModule.Manufacturer> GetManufacturerList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> GetFirstLevelCategoryList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> GetSecondLevelCategoryList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> GetThirdLevelCategoryList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.ProductManager> GetProductManagerList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.ProductDescription> GetProductDescriptionList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.Product> GetProductList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.Product> GetSecondHandProductList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.Picture> GetPictureList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.Stock> GetStockList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.SecondHand> GetSecondHandList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.InventName> GetInventNameEnglishList(string dataAreaId);

        List<CompanyGroup.Domain.MaintainModule.CompatibilityItem> GetCompatibilityItemList(string productId, string dataAreaId, bool part);

        List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine> GetPurchaseOrderLineList(string dataAreaId);
    }
}
