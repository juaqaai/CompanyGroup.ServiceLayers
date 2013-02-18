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
    public class InvoiceRepository : RepositoryBase, CompanyGroup.Domain.PartnerModule.IInvoiceRepository
    {
        /// <summary>
        /// vevőrendeléshez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public InvoiceRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
        }

        /// <summary>
        /// számlalista kiolvasása vevőazonosító alapján
        /// [InternetUser].[InvoiceSelect]( @CustomerId NVARCHAR(10) = '',	--vevokod
		///									     @Debit BIT = 0,				--0: mind, 1 kifizetetlen
		///									     @OverDue BIT = 0 )				--0: mind, 1 lejart 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> GetList(string customerId, bool debit, bool overdue)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceSelect")
                                                .SetString("CustomerId", customerId)
                                                .SetBoolean("Debit", debit)
                                                .SetBoolean("Overdue", overdue)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>() as List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// összes számlainfo elem kiolvasása
        /// </summary>
        /// <returns></returns>
        //public List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> GetAll()
        //{
        //    try
        //    {
        //        this.ReConnect();

        //        MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

        //        MongoCursor<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = collection.FindAll().SetSortOrder(MongoDB.Driver.Builders.SortBy.Descending("InvoiceId"));

        //        List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> result = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

        //        foreach (CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo in invoiceInfoList)
        //        {
        //            result.Add(invoiceInfo);
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        this.Disconnect();
        //    }        
        //}

        /// <summary>
        /// számlainfo elem kiolvasása
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        //public CompanyGroup.Domain.PartnerModule.InvoiceInfo GetById(string invoiceId)
        //{
        //    try
        //    {
        //        this.ReConnect();

        //        CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(invoiceId), "The invoiceId parameter cannot be null!");

        //        MongoCollection<CompanyGroup.Domain.PartnerModule.InvoiceInfo> collection = this.GetCollection(InvoiceRepository.CollectionName);

        //        MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("InvoiceId", MongoDB.Bson.BsonString.Create(invoiceId)));

        //        CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = collection.FindOne(query);

        //        return invoiceInfo;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Disconnect();
        //    }        
        //}

    }
}
