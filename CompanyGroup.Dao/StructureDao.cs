using System;
using System.Collections.Generic;

namespace CompanyGroup.Data
{
    public class StructureDao : AbstractNoSqlDao, CompanyGroup.Core.DataInterfaces.IStructureDao
    {

        public StructureDao(string serverHostName, int port, string dataBaseName, string collectionName) : base(serverHostName, port, dataBaseName, collectionName) { }

        /// <summary>
        /// termékstruktúra lekérdezés
        /// </summary>
        /// <example>
        /// var query = Query.And(
        /// Query.EQ("author", "Kurt Vonnegut"),
        /// Query.EQ("title", "Cats Craddle") );
        /// </example>
        /// <param name="dataAreaId"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <returns></returns>
        public CompanyGroup.Core.Domain.Structures GetList(string dataAreaId,
                                                           bool actionFilter,
                                                           bool bargainFilter,
                                                           bool newFilter,
                                                           bool stockFilter,
                                                           string textFilter)
        {
            try
            {
                Connect();

                MongoDB.Driver.Builders.QueryComplete query = this.ConstructQueryDocument(String.Empty, String.Empty, String.Empty, String.Empty,
                                                                                          actionFilter,
                                                                                          bargainFilter,
                                                                                          newFilter,
                                                                                          stockFilter,
                                                                                          textFilter,
                                                                                          dataAreaId);

                string[] sortFields = new string[] { "ManufacturerName", "Category1Name", "Category2Name", "Category3Name" };

                string[] fields = new string[] { "ManufacturerId", "ManufacturerName", "Category1Id", "Category1Name", "Category2Id", "Category2Name", "Category3Id", "Category3Name" };

                MongoDB.Driver.MongoCollection<MongoDB.Bson.BsonDocument> catalogue = this.GetCollection<MongoDB.Bson.BsonDocument>();

                //IEqualityComparer<Shared.Web.NoSql.Entities.CatalogueItem> catalogueComparer = new CatalogueComparer();

                MongoDB.Driver.MongoCursor<MongoDB.Bson.BsonDocument> filteredList = catalogue.Find(query).SetSortOrder(sortFields).SetFields(fields);

                CompanyGroup.Core.Domain.Structures resultList = new CompanyGroup.Core.Domain.Structures();

                foreach (MongoDB.Bson.BsonDocument item in filteredList)
                {
                    resultList.Add(
                       new CompanyGroup.Core.Domain.Structure()
                       {
                           ManufacturerId = item["ManufacturerId"].IsString ? item["ManufacturerId"].AsString : String.Empty,
                           ManufacturerName = item["ManufacturerName"].IsString ? item["ManufacturerName"].AsString : String.Empty,
                           Category1Id = item["Category1Id"].IsString ? item["Category1Id"].AsString : String.Empty,
                           Category1Name = item["Category1Name"].IsString ? item["Category1Name"].AsString : String.Empty,
                           Category2Id = item["Category2Id"].IsString ? item["Category2Id"].AsString : String.Empty,
                           Category2Name = item["Category2Name"].IsString ? item["Category2Name"].AsString : String.Empty,
                           Category3Id = item["Category3Id"].IsString ? item["Category3Id"].AsString : String.Empty,
                           Category3Name = item["Category3Name"].IsString ? item["Category3Name"].AsString : String.Empty
                       }
                    );
                }
                return resultList;
            }
            catch
            {
                return new CompanyGroup.Core.Domain.Structures();
            }
            finally
            {
                Disconnect();
            }
        }


    }
}
