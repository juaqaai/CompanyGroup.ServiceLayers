using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public interface IProductService
    {
        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.Products GetProducts(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request);

        CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.WebshopModule.GetBannerListRequest request);

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista termékelem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.WebshopModule.GetPriceListRequest request);

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request);

        CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request);

        CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request);

        /// <summary>
        /// részletes adatlap log lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList GetCatalogueDetailsLogList(CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogListRequest request);

        /// <summary>
        /// készlet databszám befrissítése
        /// </summary>
        /// <param name="request"></param>
        void StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request);
    }
}
