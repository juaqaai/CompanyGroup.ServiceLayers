using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    public class ProductRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.WebshopModule.IProductRepository
    {

        public ProductRepository(NHibernate.ISession session) : base(session) { }

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
        public CompanyGroup.Domain.WebshopModule.Products GetList(string dataAreaId,
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
                                                                  ref long count)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((currentPageIndex > 0), "The currentPageIndex parameter must be a positive number!");

                CompanyGroup.Domain.Utils.Check.Require((itemsOnPage > 0), "The itemsOnPage parameter must be a positive number!");

                count = Session.GetNamedQuery("InternetUser.ProductListCount")
                                               .SetString("DataAreaId", dataAreaId)
                                                .SetString("Manufacturers", manufacturers)
                                                .SetString("Category1", category1)
                                                .SetString("Category2", category2)
                                                .SetString("Category3", category3)
                                               .SetBoolean("Discount", discountFilter)
                                               .SetBoolean("SecondHand", secondHandFilter)
                                               .SetBoolean("New", newFilter)
                                               .SetBoolean("Stock", stockFilter)
                                               .SetString("FindText", textFilter)
                                               .SetString("PriceFilter", priceFilter)
                                               .SetInt32("PriceFilterRelation", priceFilterRelation).UniqueResult<int>();

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CatalogueSelect")
                                                .SetString("DataAreaId", dataAreaId)
                                                .SetString("Manufacturers", manufacturers)
                                                .SetString("Category1", category1)
                                                .SetString("Category2", category2)
                                                .SetString("Category3", category3)
                                                .SetBoolean("Discount", discountFilter)
                                                .SetBoolean("SecondHand", secondHandFilter)
                                                .SetBoolean("New", newFilter)
                                                .SetBoolean("Stock", stockFilter)
                                                .SetString("FindText", textFilter)
                                                .SetString("PriceFilter", priceFilter)
                                                .SetInt32("PriceFilterRelation", priceFilterRelation)
                                                .SetInt32("Sequence", sequence)
                                                .SetInt32("CurrentPageIndex", currentPageIndex)
                                                .SetInt32("ItemsOnPage", itemsOnPage)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Product).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Product> products = query.List<CompanyGroup.Domain.WebshopModule.Product>() as List<CompanyGroup.Domain.WebshopModule.Product>;

                CompanyGroup.Domain.WebshopModule.Products resultList = new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(currentPageIndex, count, itemsOnPage));

                resultList.AddRange(products);

                return resultList;
            }
            catch(Exception ex)
            {
                throw (ex);
                //return new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(currentPageIndex, count, itemsOnPage));
            }
        }

        /// <summary>
        /// terméknév kiegészítő lista
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
        public CompanyGroup.Domain.WebshopModule.CompletionList GetComplationList(string prefix, 
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
                                                                                  int priceFilterRelation)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((!String.IsNullOrEmpty(prefix)), "The prefix parameter cannot be null or empty!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CatalogueCompletionSelect")
                                                                .SetString("Prefix", prefix)
                                                                .SetString("DataAreaId", dataAreaId)
                                                                .SetString("Manufacturers", manufacturers)
                                                                .SetString("Category1", category1)
                                                                .SetString("Category2", category2)
                                                                .SetString("Category3", category3)
                                                                .SetBoolean("Discount", discountFilter)
                                                                .SetBoolean("SecondHand", secondHandFilter)
                                                                .SetBoolean("New", newFilter)
                                                                .SetBoolean("Stock", stockFilter)
                                                                .SetString("FindText", textFilter)
                                                                .SetString("PriceFilter", priceFilter)
                                                                .SetInt32("PriceFilterRelation", priceFilterRelation)
                                                                .SetResultTransformer(
                                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Completion).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Completion> products = query.List<CompanyGroup.Domain.WebshopModule.Completion>() as List<CompanyGroup.Domain.WebshopModule.Completion>;

                CompanyGroup.Domain.WebshopModule.CompletionList resultList = new CompanyGroup.Domain.WebshopModule.CompletionList();

                resultList.AddRange(products);

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.CompletionList();
            }
        }

        /// <summary>
        /// termék banner lista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.BannerProducts GetBannerList(string dataAreaId, 
                                                                              string manufacturers,
                                                                              string category1,
                                                                              string category2,
                                                                              string category3)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CatalogueBannerSelect")
                                                                .SetString("DataAreaId", dataAreaId)
                                                                .SetString("Manufacturers", manufacturers)
                                                                .SetString("Category1", category1)
                                                                .SetString("Category2", category2)
                                                                .SetString("Category3", category3)
                                                                .SetResultTransformer(
                                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.BannerProduct).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.BannerProduct> products = query.List<CompanyGroup.Domain.WebshopModule.BannerProduct>() as List<CompanyGroup.Domain.WebshopModule.BannerProduct>;

                CompanyGroup.Domain.WebshopModule.BannerProducts resultList = new CompanyGroup.Domain.WebshopModule.BannerProducts();

                resultList.AddRange(products);

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.BannerProducts();
            }
        }

        /// <summary>
        /// árlista lekérdezés
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
        public CompanyGroup.Domain.WebshopModule.PriceList GetPriceList(string dataAreaId,
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
                                                                        int sequence)
        { 
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.PricelistSelect")
                                                .SetString("DataAreaId", dataAreaId)
                                                .SetString("Manufacturers", manufacturers)
                                                .SetString("Category1", category1)
                                                .SetString("Category2", category2)
                                                .SetString("Category3", category3)
                                                .SetBoolean("Discount", discountFilter)
                                                .SetBoolean("SecondHand", secondHandFilter)
                                                .SetBoolean("New", newFilter)
                                                .SetBoolean("Stock", stockFilter)
                                                .SetString("FindText", textFilter)
                                                .SetString("PriceFilter", priceFilter)
                                                .SetInt32("PriceFilterRelation", priceFilterRelation)
                                                .SetInt32("Sequence", sequence)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.PriceListItem).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.PriceListItem> priceListItems = query.List<CompanyGroup.Domain.WebshopModule.PriceListItem>() as List<CompanyGroup.Domain.WebshopModule.PriceListItem>;

                CompanyGroup.Domain.WebshopModule.PriceList resultList = new CompanyGroup.Domain.WebshopModule.PriceList(priceListItems);

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.PriceList(new List<CompanyGroup.Domain.WebshopModule.PriceListItem>());
            }
        }

        /// <summary>
        /// leértékelt lista
        /// </summary>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.SecondHandList GetSecondHandList()
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.SecondHandSelect")
                                                                .SetResultTransformer(
                                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.SecondHand).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.SecondHand> secondHandList = query.List<CompanyGroup.Domain.WebshopModule.SecondHand>() as List<CompanyGroup.Domain.WebshopModule.SecondHand>;

                CompanyGroup.Domain.WebshopModule.SecondHandList resultList = new CompanyGroup.Domain.WebshopModule.SecondHandList(secondHandList);

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.SecondHandList(new List<CompanyGroup.Domain.WebshopModule.SecondHand>());
            }
        }

        /// <summary>
        /// katalógus elemeket tartalmazó lista sorrendezése
        /// 0: átlagos életkor csökkenő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
        /// 1: átlagos életkor növekvő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg, 
        /// 2: azonosito növekvő,
        /// 3: azonosito csökkenő,
        /// 4: nev növekvő,
        /// 5: nev csökkenő,
        /// 6: ar növekvő,
        /// 7: ar csökkenő, 
        /// 8: belső készlet növekvően, 
        /// 9: belső készlet csökkenően
        /// 10: külső készlet növekvően
        /// 11: külső készlet csökkenően
        /// 12: garancia növekvően
        /// 13: garancia csökkenő
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        //private MongoDB.Driver.Builders.SortByBuilder GetSortOrderFieldName(int sequence)
        //{
        //    //List<string> fields = new List<string>();

        //    MongoDB.Driver.Builders.SortByBuilder builder;

        //    switch (sequence)
        //    {
        //        case 0:
        //            {
        //                //átlagos életkor csökkenő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
        //                break;
        //            }
        //        case 1:
        //            {
        //                //átlagos életkor növekvő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
        //                break;
        //            }
        //        case 2:
        //            {
        //                //azonosito novekvo,
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("PartNumber"); //ProductId
        //                break;
        //            }
        //        case 3:
        //            {
        //                //azonosito csokkeno,
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("PartNumber");  //ProductId
        //                break;
        //            }
        //        case 4:
        //            {
        //                //nev novekvo,
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("ProductName");
        //                break;
        //            }
        //        case 5:
        //            {
        //                //nev csokkeno,
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("ProductName");
        //                break;
        //            }
        //        case 6:
        //            {
        //                //ar novekvo, 
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("Prices.Price5");
        //                break;
        //            }
        //        case 7:
        //            {
        //                //ar csokkeno
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("Prices.Price5");
        //                break;
        //            }
        //        case 8:
        //            {
        //                //belső készlet növekvően 
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("Stock.Inner").Ascending("ProductId");
        //                break;
        //            }
        //        case 9:
        //            {
        //                //belső készlet csökkenően 
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("Stock.Inner").Ascending("ProductId");
        //                break;
        //            }
        //        case 10:
        //            {
        //                //külső készlet növekvően
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("Stock.Outer").Ascending("ProductId");    //HrpInnerStock + HrpOuterStock + BscInnerStock + BscOuterStock
        //                break;
        //            }
        //        case 11:
        //            {
        //                //külső készlet csökkenően
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("Stock.Outer").Ascending("ProductId");    //HrpInnerStock + HrpOuterStock + BscInnerStock + BscOuterStock
        //                break;
        //            }
        //        case 12:
        //            {
        //                //garancia növekvően
        //                builder = MongoDB.Driver.Builders.SortBy.Ascending("Garanty.Time").Ascending("Garanty.Mode").Ascending("ProductId");
        //                break;
        //            }
        //        case 13:
        //            {
        //                //garancia csökkelnőleg
        //                builder = MongoDB.Driver.Builders.SortBy.Descending("Garanty.Time").Ascending("Garanty.Mode").Ascending("ProductId");
        //                break;
        //            }
        //        default:
        //            builder = MongoDB.Driver.Builders.SortBy.Descending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
        //            break;
        //    }
        //    return builder;
        //}

        /// <summary>
        ///  termékelem kiolvasása termékazonosító és vállalatkód szerint
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Product GetItem(string productId, string dataAreaId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(productId), "The productId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ItemSelect")
                                                .SetString("DataAreaId", dataAreaId)
                                                .SetString("ProductId", productId)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Product).GetConstructors()[0]));

                CompanyGroup.Domain.WebshopModule.Product product = query.UniqueResult<CompanyGroup.Domain.WebshopModule.Product>();

                return product;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Product();
            }
        }

        /// <summary>
        /// InternetUser.CompatibilityList( @DataAreaId nvarchar (3), 
        ///									@ProductId nvarchar (20), 
        ///									@Reverse BIT = 0	0 = a termekazonositohoz tartozo alkatreszeket keressuk, 1 = a termekazonositohoz mint alkatreszhez tartozo termekeket keressuk )
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.CompatibilityItem> GetCompatibilityItemList(string productId, string dataAreaId, bool reverse)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.CompatibilityList").SetString("DataAreaId", dataAreaId)
                                                                                             .SetString("ProductId", productId)
                                                                                             .SetBoolean("Reverse", reverse).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.CompatibilityItem).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.CompatibilityItem>() as List<CompanyGroup.Domain.WebshopModule.CompatibilityItem>;
        }
    }

}
