using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.PartnerModule
{
    public class SalesOrderRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.PartnerModule.ISalesOrderRepository
    {
        private static readonly string ClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("OrderServiceClassName", "SalesOrderService");

        /// <summary>
        /// vevőrendeléshez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public SalesOrderRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// InternetUser.cms_ProductOrderCheck( @ProductId nvarchar(20), @DataAreaId nvarchar(3), @OrderedQty int = 0) 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ProductOrderCheck GetProductOrderCheck(string productId, string dataAreaId, int quantity)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(productId), "productId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "quantity must be greather than zero");

            CompanyGroup.Domain.Utils.Check.Require((quantity > 0), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ProductOrderCheck")
                                             .SetString("ProductId", productId)
                                             .SetString("DataAreaId", dataAreaId)    //.UniqueResult<CompanyGroup.Domain.PartnerModule.MailAddress>();
                                             .SetInt32("OrderedQty", quantity)
                                             .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ProductOrderCheck).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.ProductOrderCheck response = query.UniqueResult<CompanyGroup.Domain.PartnerModule.ProductOrderCheck>();

            return response;        
        }

        /// <summary>
        /// rendelés létrehozás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult Create(CompanyGroup.Domain.PartnerModule.SalesOrderCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.PartnerModule.SalesOrderCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         SalesOrderRepository.ClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createSalesOrder", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult response = this.DeSerialize<CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult>(xml);

            return new CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult(response.ResultCode, response.Message);
        }

        /// <summary>
        /// részletes vevőrendelés sorok listája
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> GetOrderDetailedLineInfo(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_SalesOrderList")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>() as List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>;
        }
    }
}
