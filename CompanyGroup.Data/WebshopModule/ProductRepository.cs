using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    public class ProductRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.WebshopModule.Product>, CompanyGroup.Domain.WebshopModule.IProductRepository
    {
        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("ProductListCollectionName", "ProductList");

        public ProductRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// terméklista beilesztése
        /// </summary>
        /// <param name="products"></param>
        public void InsertList(List<CompanyGroup.Domain.WebshopModule.Product> products)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection(ProductRepository.CollectionName);

                collection.InsertBatch(products);

                return;
            }
            catch
            {
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// terméklista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturerIdList"></param>
        /// <param name="category1IdList"></param>
        /// <param name="category2IdList"></param>
        /// <param name="category3IdList"></param>
        /// <param name="actionFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="sequence"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Products GetList(string dataAreaId,
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
                                                                  ref long count)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((currentPageIndex > 0), "The currentPageIndex parameter must be a positive number!");

                CompanyGroup.Domain.Utils.Check.Require((itemsOnPage > 0), "The itemsOnPage parameter must be a positive number!");

                MongoDB.Driver.IMongoQuery query = ConstructQueryDocument(manufacturerIdList, category1IdList, category2IdList, category3IdList,
                                                                                     actionFilter, bargainFilter, isInNewsletterFilter, newFilter, stockFilter, 
                                                                                     textFilter, priceFilter, priceFilterRelation, nameOrPartNumberFilter, dataAreaId);

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> products = this.GetCollection(ProductRepository.CollectionName);

                count = products.Find(query).Count();

                MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = products.Find(query).SetSortOrder(GetSortOrderFieldName(sequence)).SetSkip((currentPageIndex - 1) * itemsOnPage).SetLimit(itemsOnPage);

                //IQueryable<CompanyGroup.Domain.WebshopModule.Product> list = filteredList.AsQueryable().OrderBy(x => x.ProductName).Skip((currentPageIndex - 1) * itemsOnPage).Take(itemsOnPage);

                CompanyGroup.Domain.WebshopModule.Products resultList = new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(currentPageIndex, count, itemsOnPage));

                foreach (CompanyGroup.Domain.WebshopModule.Product product in filteredList)
                {
                    resultList.Add(product);
                }

                //var prm = System.Linq.Expressions.Expression.Parameter(typeof(CompanyGroup.Domain.WebshopModule.Product), "root");

                //List<CompanyGroup.Domain.WebshopModule.Product> tmp = Sort(filteredList, prm, "ProductName", SortDirection.Ascending)
                //    .Skip((currentPageIndex - 1) * itemsOnPage).Take(itemsOnPage).ToList();

                //resultList.AddRange(tmp);

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(currentPageIndex, count, itemsOnPage));
            }
            finally
            {
                this.Disconnect();
            }
        }

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
        public CompanyGroup.Domain.WebshopModule.Structures GetStructure(string dataAreaId,
                                                                    bool actionFilter,
                                                                    bool bargainFilter,
                                                                    bool isInNewsletterFilter,
                                                                    bool newFilter,
                                                                    bool stockFilter,
                                                                    string textFilter,
                                                                    string priceFilter,
                                                                    int priceFilterRelation,
                                                                    string nameOrPartNumberFilter)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.IMongoQuery query = this.ConstructQueryDocument(new List<string>(), new List<string>(), new List<string>(), new List<string>(),
                                                                                          actionFilter,
                                                                                          bargainFilter,
                                                                                          isInNewsletterFilter,
                                                                                          newFilter,
                                                                                          stockFilter,
                                                                                          textFilter,
                                                                                          priceFilter,
                                                                                          priceFilterRelation,
                                                                                          nameOrPartNumberFilter,
                                                                                          dataAreaId);

                //string[] sortFields = new string[] { "Structure.Manufacturer.ManufacturerName", "Structure.Category1.CategoryName", "Structure.Category2.CategoryName", "Structure.Category3.CategoryName" };

                //string[] fields = new string[] { "Structure.Manufacturer.ManufacturerId", "Structure.Manufacturer.ManufacturerName", "Structure.Category1.CategoryId", "Structure.Category1.CategoryName", "Structure.Category2.CategoryId", "Structure.Category2.CategoryName", "Structure.Category3.CategoryId", "Structure.Category3.CategoryName" };

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.Product> catalogue = this.GetCollection(ProductRepository.CollectionName);

                //IEqualityComparer<Shared.Web.NoSql.Entities.CatalogueItem> catalogueComparer = new CatalogueComparer();

                MongoDB.Driver.MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = catalogue.Find(query); //.SetSortOrder(sortFields).SetFields(fields);MongoDB.Bson.BsonDocument

                List<CompanyGroup.Domain.WebshopModule.Product> resultList = new List<CompanyGroup.Domain.WebshopModule.Product>();

                foreach (CompanyGroup.Domain.WebshopModule.Product item in filteredList)
                {
                    resultList.Add(item);
                    //CompanyGroup.Domain.WebshopModule.Factory.CreateStructure(ConvertBsonToString(item["Structure"].AsBsonDocument["Manufacturer"].AsBsonDocument["ManufacturerId"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Manufacturer"].AsBsonDocument["ManufacturerName"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category1"].AsBsonDocument["CategoryId"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category1"].AsBsonDocument["CategoryName"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category2"].AsBsonDocument["CategoryId"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category2"].AsBsonDocument["CategoryName"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category3"].AsBsonDocument["CategoryId"]),
                    //                                                          ConvertBsonToString(item["Structure"].AsBsonDocument["Category3"].AsBsonDocument["CategoryName"]))
                    //);
                }
                CompanyGroup.Domain.WebshopModule.Structures structures = new CompanyGroup.Domain.WebshopModule.Structures();
                
                List<CompanyGroup.Domain.WebshopModule.Structure> structureList = resultList.ConvertAll( x => new CompanyGroup.Domain.WebshopModule.Structure(){ Manufacturer = x.Structure.Manufacturer, Category1 = x.Structure.Category1, Category2 = x.Structure.Category2, Category3 = x.Structure.Category3 });

                structures.AddRange(structureList);

                return structures;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Domain.WebshopModule.Structures();
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// terméknév kiegészítése lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="prefix"></param>
        /// <param name="limit"></param>
        /// <param name="completionType">0: nincs megadva, 1: termékazonosító-cikkszám, 2: minden</param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.CompletionList GetComplationList(string dataAreaId, string prefix, int limit, CompanyGroup.Domain.WebshopModule.CompletionType completionType)
        {
            try
            {
                this.ReConnect();

                if (String.IsNullOrWhiteSpace(prefix))
                {
                    return new CompanyGroup.Domain.WebshopModule.CompletionList();      
                }

                MongoDB.Driver.IMongoQuery query = (String.IsNullOrWhiteSpace(dataAreaId)) ? MongoDB.Driver.Builders.Query.NE("DataAreaId", CompanyGroup.Domain.Core.Constants.DataAreaIdSerbia) : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

                query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.LT("ItemState", 2));

                MongoDB.Bson.BsonRegularExpression regex = MongoDB.Bson.BsonRegularExpression.Create(new System.Text.RegularExpressions.Regex(prefix, System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                string[] sortOrder;

                if (completionType.Equals(CompanyGroup.Domain.WebshopModule.CompletionType.Full))
                {
                    query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Matches("ProductName", regex));

                    sortOrder = new string[] { "ProductName" };
                }
                else
                {
                    query = MongoDB.Driver.Builders.Query.And(query, 
                                                              MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ProductId", regex), MongoDB.Driver.Builders.Query.Matches("PartNumber", regex)));

                    sortOrder = new string[] { "ProductId", "PartNumber" };
                }

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> products = this.GetCollection(ProductRepository.CollectionName);

                MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = products.Find(query).SetSortOrder(sortOrder).SetLimit(limit);

                CompanyGroup.Domain.WebshopModule.CompletionList result = new CompanyGroup.Domain.WebshopModule.CompletionList();

                foreach (CompanyGroup.Domain.WebshopModule.Product item in filteredList)
                {
                    result.Add(new CompanyGroup.Domain.WebshopModule.Completion(item.ProductId, item.ProductName, item.DataAreaId, item.PrimaryPicture.RecId));
                }

                return result;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.CompletionList();
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Products GetBannerList(string dataAreaId, int limit)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                MongoDB.Driver.IMongoQuery query = ConstructQueryDocument(new List<string>(), new List<string>(), new List<string>(), new List<string>(),
                                                                                     true, false, false, false, true, "", "", 0, "", dataAreaId);

                query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Where("this.Pictures.length > 0"));

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> products = this.GetCollection(ProductRepository.CollectionName);

                MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = products.Find(query)
                                                                                              .SetSortOrder(GetSortOrderFieldName(1))
                                                                                              .SetLimit(limit);

                CompanyGroup.Domain.WebshopModule.Products resultList = new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(0, 0, limit));

                foreach (CompanyGroup.Domain.WebshopModule.Product product in filteredList)
                {
                    resultList.Add(product);
                }
                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(0, 0, limit));
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// árlista lekérdezés
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
        public CompanyGroup.Domain.WebshopModule.PriceList GetPriceList(string dataAreaId,
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
                                                                        int sequence)
        { 
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require((dataAreaId != null), "The dataAreaId parameter cannot be null!");

                MongoDB.Driver.IMongoQuery query = ConstructQueryDocument(manufacturerIdList, category1IdList, category2IdList, category3IdList,
                                                                                      actionFilter, bargainFilter, isInNewsletterFilter, newFilter, stockFilter,
                                                                                      textFilter, priceFilter, priceFilterRelation, nameOrPartNumberFilter, dataAreaId);

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> products = this.GetCollection(ProductRepository.CollectionName);

                MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = products.Find(query).SetSortOrder(GetSortOrderFieldName(sequence));

                CompanyGroup.Domain.WebshopModule.PriceList resultList = new CompanyGroup.Domain.WebshopModule.PriceList(filteredList.ToList());

                return resultList;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.PriceList(new List<CompanyGroup.Domain.WebshopModule.Product>());
            }
            finally
            {
                this.Disconnect();
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
        private MongoDB.Driver.Builders.SortByBuilder GetSortOrderFieldName(int sequence)
        {
            //List<string> fields = new List<string>();

            MongoDB.Driver.Builders.SortByBuilder builder;

            switch (sequence)
            {
                case 0:
                    {
                        //átlagos életkor csökkenő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
                        break;
                    }
                case 1:
                    {
                        //átlagos életkor növekvő, akciós csökkenő, gyártó növekvő, termékazonosító szerint növekvőleg,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
                        break;
                    }
                case 2:
                    {
                        //azonosito novekvo,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("PartNumber"); //ProductId
                        break;
                    }
                case 3:
                    {
                        //azonosito csokkeno,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("PartNumber");  //ProductId
                        break;
                    }
                case 4:
                    {
                        //nev novekvo,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("ProductName");
                        break;
                    }
                case 5:
                    {
                        //nev csokkeno,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("ProductName");
                        break;
                    }
                case 6:
                    {
                        //ar novekvo, 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Prices.Price5");
                        break;
                    }
                case 7:
                    {
                        //ar csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Prices.Price5");
                        break;
                    }
                case 8:
                    {
                        //belső készlet növekvően 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Stock.Inner").Ascending("ProductId");
                        break;
                    }
                case 9:
                    {
                        //belső készlet csökkenően 
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Stock.Inner").Ascending("ProductId");
                        break;
                    }
                case 10:
                    {
                        //külső készlet növekvően
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Stock.Outer").Ascending("ProductId");    //HrpInnerStock + HrpOuterStock + BscInnerStock + BscOuterStock
                        break;
                    }
                case 11:
                    {
                        //külső készlet csökkenően
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Stock.Outer").Ascending("ProductId");    //HrpInnerStock + HrpOuterStock + BscInnerStock + BscOuterStock
                        break;
                    }
                case 12:
                    {
                        //garancia növekvően
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Garanty.Time").Ascending("Garanty.Mode").Ascending("ProductId");
                        break;
                    }
                case 13:
                    {
                        //garancia csökkelnőleg
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Garanty.Time").Ascending("Garanty.Mode").Ascending("ProductId");
                        break;
                    }
                default:
                    builder = MongoDB.Driver.Builders.SortBy.Descending("AverageInventory").Descending("Discount").Ascending("Structure.Manufacturer.ManufacturerId").Ascending("ProductId");
                    break;
            }
            return builder;
        }

        /// <summary>
        /// termékelem kiolvasása kulcsmező szerint
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Product GetItem(string objectId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(objectId), "The _id parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection(ProductRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(objectId)));

                CompanyGroup.Domain.WebshopModule.Product product = collection.FindOne(query);

                return product;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Product();
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        ///  termékelem kiolvasása termékazonosító és vállalatkód szerint szerint
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Product GetItem(string productId, string dataAreaId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(productId), "The productId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(dataAreaId), "The dataAreaId parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection(ProductRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", productId),
                                                                                     MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId));

                CompanyGroup.Domain.WebshopModule.Product product = collection.FindOne(query);

                return product;  
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Product();
            }
            finally
            {
                this.Disconnect();
            }
        }

        public void UpdateProductPictures(CompanyGroup.Domain.MaintainModule.Picture picture)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection(ProductRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", picture.ItemId),
                                                                                     MongoDB.Driver.Builders.Query.EQ("DataAreaId", picture.DataAreaId));

                MongoCursor<CompanyGroup.Domain.WebshopModule.Product> products = collection.Find(query);

                foreach (CompanyGroup.Domain.WebshopModule.Product product in products)
                { 
                    //collection.Update(MongoDB.Driver.Builders.Query.EQ("Id", product.Id), MongoDB.Driver.Builders.Update.Pull("Pictures", MongoDB.Driver.Builders.Query.EQ("FileName", MongoDB.Bson.BsonValue.Create(picture.FileName))));

                    collection.Update(MongoDB.Driver.Builders.Query.EQ("_id", product.Id), MongoDB.Driver.Builders.Update.PushWrapped("Pictures", product.Pictures));
                }
            }
            catch
            {
            }
            finally
            {
                Disconnect();
            }            
        }

    }

}
