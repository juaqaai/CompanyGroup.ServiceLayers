using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IProductRepository
    {
        /// <summary>
        /// lista elemeinek száma
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="sequence"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <returns></returns>
        int GetListCount(string dataAreaId,
                                        string manufacturers,
                                        string category1,
                                        string category2,
                                        string category3,
                                        bool discountFilter,
                                        bool secondHandFilter,
                                        bool isInNewsletterFilter,
                                        bool newFilter,
                                        bool stockFilter,
                                        int sequence,
                                        string textFilter,
                                        string priceFilter,
                                        int priceFilterRelation);

        /// <summary>
        /// lapozható terméklista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="sequence"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Products GetList(string dataAreaId,
                                                           string manufacturers,
                                                           string category1,
                                                           string category2,
                                                           string category3,
                                                           bool discountFilter,
                                                           bool secondHandFilter,
                                                           bool isInNewsletterFilter,
                                                           bool newFilter,
                                                           bool stockFilter,
                                                           int sequence,
                                                           string textFilter,
                                                           string priceFilter,
                                                           int priceFilterRelation,
                                                           int currentPageIndex,
                                                           int itemsOnPage,
                                                           long count);

        /// <summary>
        /// terméklista lekérdezés
        /// </summary>
        /// <returns></returns>
        //CompanyGroup.Domain.WebshopModule.ProductList GetProductList();

        /// <summary>
        /// kiegészítő lista
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.CompletionList GetCompletionList(string prefix,
                                                                           string dataAreaId,
                                                                           string manufacturers,
                                                                           string category1,
                                                                           string category2,
                                                                           string category3,
                                                                           bool discountFilter,
                                                                           bool secondHandFilter,
                                                                           bool isInNewsletterFilter,
                                                                           bool newFilter,
                                                                           bool stockFilter,
                                                                           string textFilter,
                                                                           string priceFilter,
                                                                           int priceFilterRelation);

        /// <summary>
        /// banner lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.BannerProducts GetBannerList(string dataAreaId,
                                                                       string manufacturers, 
                                                                       string category1,
                                                                       string category2,
                                                                       string category3);

        /// <summary>
        /// árlista lekérdezés szűrőparaméterek alapján
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.PriceList GetPriceList(string dataAreaId,
                                                                 string manufacturers,
                                                                 string category1,
                                                                 string category2,
                                                                 string category3,
                                                                 bool discountFilter,
                                                                 bool secondHandFilter,
                                                                 bool isInNewsletterFilter,
                                                                 bool newFilter,
                                                                 bool stockFilter,
                                                                 string textFilter,
                                                                 string priceFilter,
                                                                 int priceFilterRelation,
                                                                 int sequence);

        /// <summary>
        /// termékelem lekérdezés termékazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Product GetItem(string productId, string dataAreaId);

        /// <summary>
        /// mihez jó, és mi jó hozzá 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.CompatibilityItem> GetCompatibilityItemList(string productId, string dataAreaId, bool reverse);

        /// <summary>
        /// leértékelt lista
        /// </summary>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.SecondHandList GetSecondHandList();

        /// <summary>
        /// részletes adatlap log lista
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog> CatalogueDetailsLogList(string visitorId);

        /// <summary>
        /// részletes adatlap log hozzáadás
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="customerId"></param>
        /// <param name="personId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="productId"></param>
        void AddCatalogueDetailsLog(string visitorId, string customerId, string personId, string dataAreaId, string productId);
    }
}
