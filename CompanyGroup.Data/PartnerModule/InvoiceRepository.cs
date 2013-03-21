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
        /// [InternetUser].[InvoiceCount]( @CustomerId NVARCHAR(10) = '',	--vevokod
		///						@Debit BIT = 0,				--0: mind, 1 kifizetetlen
		///						@OverDue BIT = 0,				--0: mind, 1 lejart 
		///						@ItemId NVARCHAR(20) = '', 
		///						@ItemName NVARCHAR(300) = '',
		///						@SalesId NVARCHAR(20) = '',
		///						@SerialNumber NVARCHAR(40) = '',
		///						@InvoiceId NVARCHAR(20) = '',
		///						@DateIntervall INT = 0 )
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="debit"></param>
        /// <param name="overdue"></param>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="salesId"></param>
        /// <param name="serialNumber"></param>
        /// <param name="invoiceId"></param>
        /// <param name="dateIntervall"></param>
        /// <returns></returns>
        public int GetListCount(string customerId, bool debit, bool overdue, string itemId, string itemName,
                                string salesId, string serialNumber, string invoiceId, int dateIntervall)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceCount")
                                                 .SetString("CustomerId", customerId)
                                                 .SetBoolean("Debit", debit)
                                                 .SetBoolean("Overdue", overdue)
                                                 .SetString("ItemId", itemId)
                                                 .SetString("ItemName", itemName)
                                                 .SetString("SalesId", salesId)
                                                 .SetString("SerialNumber", serialNumber)
                                                 .SetString("InvoiceId", invoiceId)
                                                 .SetInt32("DateIntervall", dateIntervall);

                return query.UniqueResult<int>();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// számla fejléc lista
        /// [InternetUser].[InvoiceSelect]( @CustomerId NVARCHAR(10) = '',	--vevokod
		///									     @Debit BIT = 0,				--0: mind, 1 kifizetetlen
		///									     @OverDue BIT = 0,				--0: mind, 1 lejart 
		///										 @ItemId NVARCHAR(20) = '', 
		///										 @ItemName NVARCHAR(300) = '',
        ///										 @SalesId NVARCHAR(20) = '',
		///										 @SerialNumber NVARCHAR(40) = '',
		///										 @InvoiceId NVARCHAR(20) = '',
		///										 @DateIntervall INT = 0,
		///										 @Sequence int = 0,	
		///										 @CurrentPageIndex INT = 1, 
		///										 @ItemsOnPage INT = 30 )
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="debit"></param>
        /// <param name="overdue"></param>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="invoiceId"></param>
        /// <param name="serialNumber"></param>
        /// <param name="invoiceId"></param>
        /// <param name="dateIntervall"></param>
        /// <param name="sequence"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceHeader> GetList(string customerId, bool debit, bool overdue, string itemId, string itemName,
                                                                             string invoiceId, string serialNumber, string salesId, int dateIntervall,
                                                                             int sequence, int currentPageIndex, int itemsOnPage)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceSelect")
                                                 .SetString("CustomerId", customerId)
                                                 .SetBoolean("Debit", debit)
                                                 .SetBoolean("Overdue", overdue)
                                                 .SetString("ItemId", itemId)
                                                 .SetString("ItemName", itemName)
                                                 .SetString("SalesId", salesId)
                                                 .SetString("SerialNumber", serialNumber)
                                                 .SetString("InvoiceId", invoiceId)
                                                 .SetInt32("DateIntervall", dateIntervall)
                                                 .SetInt32("Sequence", sequence)
                                                 .SetInt32("CurrentPageIndex", currentPageIndex)
                                                 .SetInt32("ItemsOnPage", itemsOnPage)
                                                 .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.InvoiceHeader).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.InvoiceHeader>() as List<CompanyGroup.Domain.PartnerModule.InvoiceHeader>;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// számla listaelemek kiolvasása 
        /// [InternetUser].[InvoiceDetailsSelect]( @InvoiceId NVARCHAR(20) = '' )
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceLine> GetDetails(int id)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceDetailsSelect").SetInt32("Id", id)
                                                                                                    .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.InvoiceLine).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.InvoiceLine>() as List<CompanyGroup.Domain.PartnerModule.InvoiceLine>;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// [InternetUser].[InvoiceSumValues](@CustomerId NVARCHAR(10) = '')
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.InvoiceSumAmount> InvoiceSumValues(string customerId)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceSumValues").SetString("CustomerId", customerId)
                                                                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.InvoiceSumAmount).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.InvoiceSumAmount>() as List<CompanyGroup.Domain.PartnerModule.InvoiceSumAmount>;

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
