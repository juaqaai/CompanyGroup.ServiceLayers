using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// számlaműveleteket végző osztály
    /// </summary>
    public class InvoiceRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.PartnerModule.InvoiceInfo>, CompanyGroup.Domain.PartnerModule.IInvoiceRepository
    {
        public InvoiceRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("InvoiceCollectionName", "InvoiceList");

        /// <summary>
        /// számlalista kiolvasása vevőazonosító alapján
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

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("CustomerId", MongoDB.Bson.BsonString.Create(customerId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("DataAreaId", MongoDB.Bson.BsonString.Create(dataAreaId)));

                MongoCursor<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = collection.Find(query).SetSortOrder(MongoDB.Driver.Builders.SortBy.Descending("InvoiceId"));

                List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> result = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

                foreach(CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo in invoiceInfoList)
                {
                    result.Add(invoiceInfo);
                }

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
        /// összes számlainfo elem kiolvasása
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> GetAll()
        {
            try
            {
                this.ReConnect();

                MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

                MongoCursor<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = collection.FindAll().SetSortOrder(MongoDB.Driver.Builders.SortBy.Descending("InvoiceId"));

                List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> result = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

                foreach (CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo in invoiceInfoList)
                {
                    result.Add(invoiceInfo);
                }

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
        /// számlainfo elem kiolvasása
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.InvoiceInfo GetById(string invoiceId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(invoiceId), "The invoiceId parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("InvoiceId", MongoDB.Bson.BsonString.Create(invoiceId)));

                CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = collection.FindOne(query);

                return invoiceInfo;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Disconnect();
            }        
        }

        /// <summary>
        /// számlalista beilesztése
        /// </summary>
        /// <param name="products"></param>
        public void InsertList(List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

                collection.InsertBatch(invoiceInfoList);

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
        /// összes számla eltávolítása a cache-ből
        /// </summary>
        public override void RemoveAllItemsFromCollection()
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

                collection.RemoveAll();

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
    }
}
