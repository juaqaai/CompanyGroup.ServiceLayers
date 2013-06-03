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
        public List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> GetList(string customerId, bool debit, bool overdue, string itemId, string itemName,
                                                                                       string invoiceId, string serialNumber, string salesId, int dateIntervall,
                                                                                       int sequence, int currentPageIndex, int itemsOnPage)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoiceSelect2")
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
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo).GetConstructors()[0]));

                return query.List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>() as List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>;

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
    }
}
