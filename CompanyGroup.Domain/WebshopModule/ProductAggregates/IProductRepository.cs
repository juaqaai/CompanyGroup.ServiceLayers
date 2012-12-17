using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IProductRepository : CompanyGroup.Domain.Core.IRepository<CompanyGroup.Domain.WebshopModule.Product>
    {
        void InsertList(List<CompanyGroup.Domain.WebshopModule.Product> items);

        /// <summary>
        /// terméklista lekérdezés paraméterek alapján
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturerIdList"></param>
        /// <param name="category1IdList"></param>
        /// <param name="category2IdList"></param>
        /// <param name="category3IdList"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="nameOrPartNumberFilter"></param>
        /// <param name="sequence"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Products GetList(string dataAreaId,
                         List<string> manufacturerIdList,
                         List<string> category1IdList,
                         List<string> category2IdList,
                         List<string> category3IdList,
                         bool actionFilter,
                         bool bargainFilter,
                         bool isInNewsletterFilter, 
                         bool newFilter,
                         bool stockFilter,
                         string textFilter,
                         string priceFilter,
                         int priceFilterRelation,
                         string nameOrPartNumberFilter,
                         int sequence,
                         int currentPageIndex,
                         int itemsOnPage,
                         ref long count);

        /// <summary>
        /// termékstruktúra lekérdezés
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="nameOrPartNumberFilter"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Structures GetStructure(string dataAreaId,
                                                            bool actionFilter,
                                                            bool bargainFilter,
                                                            bool isInNewsletterFilter,
                                                            bool newFilter,
                                                            bool stockFilter,
                                                            string textFilter,
                                                            string priceFilter,
                                                            int priceFilterRelation,
                                                            string nameOrPartNumberFilter);

        /// <summary>
        /// kiegészítő lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="prefix"></param>
        /// <param name="limit"></param>
        /// <param name="completionType"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.CompletionList GetComplationList(string dataAreaId, string prefix, int limit, CompanyGroup.Domain.WebshopModule.CompletionType completionType);

        /// <summary>
        /// banner lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Products GetBannerList(string dataAreaId, int limit);

        /// <summary>
        /// árlista lekérdezés szűrőparaméterek alapján
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturerIdList"></param>
        /// <param name="category1IdList"></param>
        /// <param name="category2IdList"></param>
        /// <param name="category3IdList"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
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
                                                                List<string> manufacturerIdList,
                                                                List<string> category1IdList,
                                                                List<string> category2IdList,
                                                                List<string> category3IdList,
                                                                bool actionFilter,
                                                                bool bargainFilter,
                                                                bool isInNewsletterFilter,
                                                                bool newFilter,
                                                                bool stockFilter,
                                                                string textFilter,
                                                                string priceFilter,
                                                                int priceFilterRelation,
                                                                string nameOrPartNumberFilter,
                                                                int sequence);

        /// <summary>
        /// termékelem lekérdezés mongoDb azonosító alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Product GetItem(string objectId);

        /// <summary>
        /// termékelem lekérdezés termékazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Product GetItem(string productId, string dataAreaId);

        void CreateIndexes();
    }
}
