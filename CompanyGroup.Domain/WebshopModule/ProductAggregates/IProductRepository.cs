using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IProductRepository
    {
        /// <summary>
        /// lapozható terméklista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="structureXml"></param>
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
                                                            string structureXml,
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
                                                            ref long count);

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
        /// <param name="structureXml"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.CompletionList GetComplationList(string prefix,
                                                                           string dataAreaId,
                                                                           string structureXml,
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
        /// <param name="count"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.BannerProducts GetBannerList(string dataAreaId, string structureXml);

        /// <summary>
        ///  árlista lekérdezés szűrőparaméterek alapján
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="structureXml"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="nameOrPartNumberFilter"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.PriceList GetPriceList(string dataAreaId,
                                                                string structureXml,
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
        /// <param name="part"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.CompatibilityItem> GetCompatibilityItemList(string productId, string dataAreaId, bool part);

        /// <summary>
        /// leértékelt lista
        /// </summary>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.SecondHandList GetSecondHandList();

    }
}
