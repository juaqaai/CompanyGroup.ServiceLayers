using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.PartnerModule
{
    public class WaitForAutoPostRepository : RepositoryBase, CompanyGroup.Domain.PartnerModule.IWaitForAutoPostRepository
    {
        private static readonly string OrderServiceClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("OrderServiceClassName", "SalesOrderServiceWeb");

        private static readonly string SecondhandOrderServiceClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("SecondhandOrderServiceClassName", "SecondhandOrderService");

        /// <summary>
        /// vevőrendeléshez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public WaitForAutoPostRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

        private NHibernate.ISession WebInterfaceSession
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
        }

        /// <summary>
        ///  [InternetUser].[WaitingForAutoPostSelect]
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.WaitingForAutoPost> WaitingForAutoPostSelect()
        {

            NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.WaitingForAutoPostSelect")
                                                         .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.WaitingForAutoPost).GetConstructors()[0]));

            List<CompanyGroup.Domain.PartnerModule.WaitingForAutoPost> response = query.List<CompanyGroup.Domain.PartnerModule.WaitingForAutoPost>() as List<CompanyGroup.Domain.PartnerModule.WaitingForAutoPost>;

            return response;        
        }

        /// <summary>
        /// [InternetUser].[WaitingForAutoPostInsert](@ForeignKey INT = 0,			-- vagy a ShoppingCart.Id, vagy a Registration.Id	
        ///										      @ForeignKeyType INT = 0,		-- 1: kosár, 2: regisztráció	
        ///											  @Content XML = '')
        /// </summary>
        /// <param name="foreignKey">vagy a ShoppingCart.Id, vagy a Registration.Id</param>
        /// <param name="foreignKeyType">1: kosár, 2: regisztráció</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int WaitingForAutoPostSalesOrderInsert(int foreignKey, int foreignKeyType, CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate)
        {
            string content = this.Serialize<CompanyGroup.Domain.PartnerModule.SalesOrderCreate>(salesOrderCreate);

            CompanyGroup.Domain.Utils.Check.Require((foreignKey > 0), "foreignKey must be greather than zero");

            NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.WaitingForAutoPostInsert")
                                                                         .SetInt32("ForeignKey", foreignKey)
                                                                         .SetInt32("ForeignKeyType", foreignKeyType)
                                                                         .SetString("Content", content);

            return query.UniqueResult<int>();
        }


        public int WaitingForAutoPostSecondhandOrderInsert(int foreignKey, int foreignKeyType, CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate secondHandOrderCreate)
        {
            string content = this.Serialize<CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate>(secondHandOrderCreate);

            CompanyGroup.Domain.Utils.Check.Require((foreignKey > 0), "foreignKey must be greather than zero");

            NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.WaitingForAutoPostInsert")
                                                                         .SetInt32("ForeignKey", foreignKey)
                                                                         .SetInt32("ForeignKeyType", foreignKeyType)
                                                                         .SetString("Content", content);

            return query.UniqueResult<int>();
        }

        /// <summary>
        /// beállítja a WaitingForAutoPost rekord státuszát
        /// 0: törölt, 1: aktív (autopost-ra vár), 2: beküldött
        /// [InternetUser].[WaitingForAutoPostSetStatus](@Id INT = 0, @Status INT = 0)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int WaitingForAutoPostSetStatus(int id, int status)
        {
            CompanyGroup.Domain.Utils.Check.Require((id > 0), "Id must be greather than zero");

            NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.WaitingForAutoPostSetStatus")
                                                                         .SetInt32("Id", id)
                                                                         .SetInt32("Status", status);

            return query.UniqueResult<int>();
        }

        
    }
}
