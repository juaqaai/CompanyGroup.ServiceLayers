using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// elfelejtett jelszó műveletek
    /// </summary>
    public class ForgetPasswordRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.PartnerModule.ForgetPassword>, CompanyGroup.Domain.PartnerModule.IForgetPasswordRepository
    {
        private static readonly string ClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ForgetPasswordServiceClassName", "ContactPersonService");

        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("ForgetPasswordCollectionName", "ChangePassword");

        /// <summary>
        /// elfelejtett jelszó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public ForgetPasswordRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// jelszómódosítás kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ForgetPassword GetItemByKey(string id)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The _id parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.PartnerModule.ForgetPassword> collection = this.GetCollection(ForgetPasswordRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(id)));

                CompanyGroup.Domain.PartnerModule.ForgetPassword result = collection.FindOne(query);

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
        /// elfelejtett jelszó új bejegyzés hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        public void Add(CompanyGroup.Domain.PartnerModule.ForgetPassword forgetPassword)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.ForgetPassword> collection = this.GetCollection(ForgetPasswordRepository.CollectionName);

                collection.Insert(forgetPassword);

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
        /// elfelejtett jelszó státusz állapot beállítása 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataAreaId"></param>
        public void SetStatus(string id, CompanyGroup.Domain.PartnerModule.ForgetPasswordStatus status)
        {
            try
            {
                this.ReConnect();

                MongoCollection<CompanyGroup.Domain.PartnerModule.ForgetPassword> collection = this.GetCollection(ForgetPasswordRepository.CollectionName);

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
        /// elfelejtett jelszó AX  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ForgetPasswordCreateResult Forget(CompanyGroup.Domain.PartnerModule.ForgetPasswordCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.PartnerModule.ForgetPasswordCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(ForgetPasswordRepository.UserName,
                                                                                                         ForgetPasswordRepository.Password,
                                                                                                         ForgetPasswordRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         ForgetPasswordRepository.Language,
                                                                                                         ForgetPasswordRepository.ObjectServer,
                                                                                                         ForgetPasswordRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("forgetPwd", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.PartnerModule.ForgetPasswordCreateResult response = this.DeSerialize<CompanyGroup.Domain.PartnerModule.ForgetPasswordCreateResult>(xml);

            return response;
        }
    }
}
