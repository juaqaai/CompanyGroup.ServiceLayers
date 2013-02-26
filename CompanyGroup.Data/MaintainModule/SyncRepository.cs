using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.MaintainModule
{
    /// <summary>
    /// sync repository
    /// </summary>
    public class SyncRepository : CompanyGroup.Domain.MaintainModule.ISyncRepository
    {
        public SyncRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

        /// <summary>
        /// [InternetUser].[StockExtract] (@DataAreaId nvarchar(3), @InventLocationId nvarchar(20), @ProductId nvarchar(20))
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetStockChange(string dataAreaId, string inventLocationId, string productId)
        {
            try
            {
                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.StockExtract").SetString("DataAreaId", dataAreaId)
                                                                                            .SetString("InventLocationId", inventLocationId)
                                                                                            .SetString("ProductId", productId);

                int stock = query.UniqueResult<int>();

                return stock;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
