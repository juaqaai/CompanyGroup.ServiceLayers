using System;
using System.Collections.Generic;

namespace CompanyGroup.Data
{
    public abstract class AbstractNoSqlDao  //<T> where T : class
    {
        public AbstractNoSqlDao(string serverHostName, int port, string dataBaseName, string collectionName)
        {
            CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(serverHostName), "ServerHostName may not be null nor empty");

            CompanyGroup.Core.Utils.Check.Require(port > 0, "Port may not be zero");

            CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(dataBaseName), "DataBaseName may not be null nor empty");

            CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(collectionName), "CollectionName may not be null nor empty");

            this.serverHostName = serverHostName;

            this.port = port;

            this.dataBaseName = dataBaseName;

            this.collectionName = collectionName;
        }

        private string serverHostName = String.Empty;

        private int port = 0;

        private string dataBaseName = String.Empty;

        private string collectionName = String.Empty;

        private MongoDB.Driver.MongoServer server = null;

        private MongoDB.Driver.MongoDatabase database = null;

        private string ConnectionString { get { return String.Format("mongodb://{0}:{1}/{2}", serverHostName, port, dataBaseName, collectionName); } }

        /// <summary>
        /// connect to database
        /// </summary>
        protected void Connect()
        {
            try
            {
                CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(serverHostName), "ServerHostName may not be null nor empty");

                CompanyGroup.Core.Utils.Check.Require(port > 0, "Port may not be zero");

                CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(dataBaseName), "DataBaseName may not be null nor empty");

                CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(collectionName), "CollectionName may not be null nor empty");

                this.server = MongoDB.Driver.MongoServer.Create(this.ConnectionString);

                this.database = server.GetDatabase(this.dataBaseName);

                return;
            }
            catch { }
        }

        /// <summary>
        /// disconnect from database
        /// </summary>
        protected void Disconnect()
        {
            try
            {
                CompanyGroup.Core.Utils.Check.Require(this.server != null, "Server may not be null");

                this.database = null;

                this.server.Disconnect();
            }
            catch { }
        }

        /// <summary>
        /// read collection by collection name
        /// </summary>
        protected MongoDB.Driver.MongoCollection<T> GetCollection<T>() where T : class
        {
            return this.database.GetCollection<T>(this.collectionName);
        }

        /// <summary>
        /// delete collection by collection name
        /// </summary>
        /// <returns></returns>
        protected void DropCollection()
        {
            try
            {
                CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(collectionName), "CollectionName may not be null nor empty");

                this.database.DropCollection(this.collectionName);
            }
            catch { }
        }

        /// <summary>
        /// get nosql database
        /// </summary>
        protected MongoDB.Driver.MongoDatabase Database
        {
            get { return this.database; }
        }

        /// <summary>
        /// törli a kollekcióhoz tartozó összes indexet 
        /// </summary>
        /// <returns></returns>
        public void DeleteIndexes<T>() where T : class
        {
            try
            {
                Connect();

                MongoDB.Driver.MongoCollection<T> collection = this.GetCollection<T>();

                collection.DropAllIndexes();

                return;
            }
            catch
            {
                return;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// convert string identity to nosql objectId
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        protected MongoDB.Bson.ObjectId ConvertStringToBsonObjectId(string from)
        {
            try
            {
                CompanyGroup.Core.Utils.Check.Require(!string.IsNullOrEmpty(from), "Object identifier may not be null nor empty");

                MongoDB.Bson.ObjectId to = MongoDB.Bson.ObjectId.Empty;

                bool b = MongoDB.Bson.ObjectId.TryParse(from, out to);

                return (b) ? to : MongoDB.Bson.ObjectId.Empty;
            }
            catch { return MongoDB.Bson.ObjectId.Empty; }
        }

        protected MongoDB.Driver.Builders.QueryComplete ConstructQueryDocument(string manufacturerId,
                                                                             string category1Id,
                                                                             string category2Id,
                                                                             string category3Id,
                                                                             bool actionFilter,
                                                                             bool bargainFilter,
                                                                             bool newFilter,
                                                                             bool stockFilter,
                                                                             string textFilter,
                                                                             string dataAreaId)
        {
            bool isCommonCompany = !(dataAreaId.ToLower().Equals("ser"));

            MongoDB.Driver.Builders.QueryComplete queryComplete = isCommonCompany ? MongoDB.Driver.Builders.Query.NE("DataAreaId", "ser") : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

            queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.LT("ItemState", 2));

            if (!String.IsNullOrEmpty(manufacturerId))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("ManufacturerId", manufacturerId));
            }
            if (!String.IsNullOrEmpty(category1Id))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category1Id", category1Id));
            }
            if (!String.IsNullOrEmpty(category2Id))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category2Id", category2Id));
            }
            if (!String.IsNullOrEmpty(category3Id))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category3Id", category3Id));
            }
            if (actionFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Action", actionFilter));
            }
            if (bargainFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("BargainCounter", bargainFilter));
            }
            if (newFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("New", newFilter));
            }
            if (stockFilter)
            {

                if (isCommonCompany)
                {
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.GT("HrpInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpOuterStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscOuterStock", 0))
                        );
                }
                else
                {
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.GT("SerbianStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpOuterStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscOuterStock", 0))
                        );
                }
            }

            if (!String.IsNullOrEmpty(textFilter))
            {
                MongoDB.Bson.BsonRegularExpression regex = new MongoDB.Bson.BsonRegularExpression(String.Format(".*{0}.*", textFilter), "i");

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                    MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ItemName", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("ItemNameEnglish", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("Description", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("DescriptionEnglish", regex))
                    );

            }

            return queryComplete == null ? new MongoDB.Driver.Builders.QueryComplete(new MongoDB.Bson.BsonDocument()) : queryComplete;
        }
    }
}
