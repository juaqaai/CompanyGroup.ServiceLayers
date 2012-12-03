using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    public class InvoiceRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.PartnerModule.InvoiceInfo>, CompanyGroup.Domain.PartnerModule.IInvoiceRepository
    {

        public InvoiceRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("InvoiceCollectionName", "Invoice");

        /// <summary>
        /// részletes számla sorok listája
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> GetList(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            try
            {
                this.ReConnect();

                MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                MongoCursor<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = collection.Find(query);

                return invoiceInfoList;
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
