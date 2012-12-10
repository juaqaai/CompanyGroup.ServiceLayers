using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// látogató repository
    /// </summary>
    public class VisitorRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.PartnerModule.Visitor>, CompanyGroup.Domain.PartnerModule.IVisitorRepository
    {
        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("VisitorCollectionName", "Visitor");

        /// <summary>
        /// látogatóhoz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public VisitorRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// látogató kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor GetItemByKey(string id)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The _id parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.PartnerModule.Visitor> collection = this.GetCollection(VisitorRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                CompanyGroup.Domain.PartnerModule.Visitor visitor = collection.FindOne(query);

                return visitor;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// új látogató hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        public void Add(CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.Visitor> collection = this.GetCollection(VisitorRepository.CollectionName);

                collection.Insert(visitor);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// bejelentkezett státusz állapot törlése 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataAreaId"></param>
        public void DisableStatus(string id, string dataAreaId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.Visitor> collection = this.GetCollection(VisitorRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)),
                                                                                                MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId));

                collection.Update(query, MongoDB.Driver.Builders.Update.Set("Status", LoginStatus.Passive));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// nyelv csere
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor ChangeLanguage(string id, string language)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.Visitor> collection = this.GetCollection(VisitorRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                var result = collection.Update(query, MongoDB.Driver.Builders.Update.Set("LanguageId", MongoDB.Bson.BsonString.Create(language)));

                return MongoDB.Bson.Serialization.BsonSerializer.Deserialize<CompanyGroup.Domain.PartnerModule.Visitor>(result.Response);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }        
        }

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.Visitor ChangeCurrency(string id, string currency)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.Visitor> collection = this.GetCollection(VisitorRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                var result = collection.Update(query, MongoDB.Driver.Builders.Update.Set("Currency", MongoDB.Bson.BsonString.Create(currency)));

                return MongoDB.Bson.Serialization.BsonSerializer.Deserialize<CompanyGroup.Domain.PartnerModule.Visitor>(result.Response);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }
    }
}
