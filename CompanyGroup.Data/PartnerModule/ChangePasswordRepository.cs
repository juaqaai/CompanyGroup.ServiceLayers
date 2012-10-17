using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// jelszómódosítás műveletek
    /// </summary>
    public class ChangePasswordRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.PartnerModule.ChangePassword>, CompanyGroup.Domain.PartnerModule.IChangePasswordRepository
    {
        private static readonly string ClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ChangePasswordServiceClassName", "ContactPersonService");

        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("ChangePasswordCollectionName", "ChangePassword");

        /// <summary>
        /// jelszómódosításhoz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public ChangePasswordRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// jelszómódosítás kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ChangePassword GetItemByKey(string id)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The _id parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.PartnerModule.ChangePassword> collection = this.GetCollection(ChangePasswordRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                CompanyGroup.Domain.PartnerModule.ChangePassword result = collection.FindOne(query);

                return result;
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
        /// jelszómódosítás új bejegyzés hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        public void Add(CompanyGroup.Domain.PartnerModule.ChangePassword changePassword)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.ChangePassword> collection = this.GetCollection(ChangePasswordRepository.CollectionName);

                collection.Insert(changePassword);

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
        /// jelszómódosítás státusz állapot beállítása 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataAreaId"></param>
        public void SetStatus(string id, CompanyGroup.Domain.PartnerModule.ChangePasswordStatus status)
        {
            try
            {
                this.ReConnect();

                MongoCollection<CompanyGroup.Domain.PartnerModule.ChangePassword> collection = this.GetCollection(ChangePasswordRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                collection.FindAndModify(query, MongoDB.Driver.Builders.SortBy.Descending("_id"), MongoDB.Driver.Builders.Update.Set("Status", MongoDB.Bson.BsonInt32.Create(status)));
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
        /// jelszómódosítás AX  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult Change(CompanyGroup.Domain.PartnerModule.ChangePasswordCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.PartnerModule.ChangePasswordCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(ChangePasswordRepository.UserName,
                                                                                                         ChangePasswordRepository.Password,
                                                                                                         ChangePasswordRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         ChangePasswordRepository.Language,
                                                                                                         ChangePasswordRepository.ObjectServer,
                                                                                                         ChangePasswordRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("changePwd", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult response = this.DeSerialize<CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult>(xml);

            return response;
        }
    }
}
