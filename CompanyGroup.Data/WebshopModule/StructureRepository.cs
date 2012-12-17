using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.WebshopModule
{
    /// <summary>
    /// termékstruktúra repository
    /// </summary>
    public class StructureRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.WebshopModule.Product>, CompanyGroup.Domain.WebshopModule.IStructureRepository
    {
        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("StructureCollectionName", "ProductList");

        /// <summary>
        /// termékstruktúra repository konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public StructureRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) {  }

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
        public CompanyGroup.Domain.WebshopModule.Structures GetList(string dataAreaId,
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

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.Product> catalogue = this.GetCollection(StructureRepository.CollectionName);

                //IEqualityComparer<Shared.Web.NoSql.Entities.CatalogueItem> catalogueComparer = new CatalogueComparer();

                MongoDB.Driver.MongoCursor<CompanyGroup.Domain.WebshopModule.Product> filteredList = catalogue.Find(query); //.SetSortOrder(sortFields).SetFields(fields);MongoDB.Bson.BsonDocument

                CompanyGroup.Domain.WebshopModule.Structures resultList = new CompanyGroup.Domain.WebshopModule.Structures();

                foreach (CompanyGroup.Domain.WebshopModule.Product item in filteredList)
                {
                    resultList.Add(item.Structure);
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
                return resultList;
            }
            catch(Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Domain.WebshopModule.Structures();
            }
            finally
            {
                this.Disconnect();
            }
        }


    }
}
