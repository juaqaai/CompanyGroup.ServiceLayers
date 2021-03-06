﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.PartnerModule
{
    public class SalesOrderRepository : RepositoryBase, CompanyGroup.Domain.PartnerModule.ISalesOrderRepository
    {
        private static readonly string OrderServiceClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("OrderServiceClassName", "SalesOrderServiceWeb");

        private static readonly string SecondhandOrderServiceClassName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("SecondhandOrderServiceClassName", "SecondhandOrderService");

        /// <summary>
        /// vevőrendeléshez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public SalesOrderRepository() { }

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

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ProductOrderCheck")
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
                                                                                                         SalesOrderRepository.OrderServiceClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createSalesOrder", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult response = this.DeSerialize<CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult>(xml);

            return response;
        }

        /// <summary>
        /// használt megrendelés létrehozás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.SecondhandOrderCreateResult CreateSecondHandOrder(CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate request)
        {
            string tmp = this.Serialize<CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate>(request);

            CompanyGroup.Helpers.DynamicsConnector dynamics = new CompanyGroup.Helpers.DynamicsConnector(SalesOrderRepository.UserName,
                                                                                                         SalesOrderRepository.Password,
                                                                                                         SalesOrderRepository.Domain,
                                                                                                         request.DataAreaId,
                                                                                                         SalesOrderRepository.Language,
                                                                                                         SalesOrderRepository.ObjectServer,
                                                                                                         SalesOrderRepository.SecondhandOrderServiceClassName);
            dynamics.Connect();

            object result = dynamics.CallMethod("createSecondHandOrder", tmp);    //deSerializeTest

            dynamics.Disconnect();

            string xml = Helpers.ConvertData.ConvertObjectToString(result);

            CompanyGroup.Domain.PartnerModule.SecondhandOrderCreateResult response = this.DeSerialize<CompanyGroup.Domain.PartnerModule.SecondhandOrderCreateResult>(xml);

            return response;
        }

        /// <summary>
        /// részletes vevőrendelés sorok listája
        /// InternetUser.SalesOrderSelect(@CustomerId NVARCHAR(10), 
		///								  @CanBeTaken BIT = 0, 	-- 0 none, 1 sold, 2 deducted (eladva), 3 picked (kivéve), 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt), 7 Quotation issue (árajánlat kiadása))
		///								  @SalesStatus INT = 1)
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> GetOrderDetailedLineInfo(string customerId, bool canBeTaken, int salesStatus, string customerOrderNo, string itemName, string itemId, string salesOrderId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.SalesOrderSelect")
                                                                         .SetString("CustomerId", customerId)
                                                                         .SetBoolean("CanBeTaken", canBeTaken)
                                                                         .SetInt32("SalesStatus", salesStatus)
                                                                         .SetString("CustomerOrderNo", customerOrderNo)
                                                                         .SetString("ItemName", itemName)
                                                                         .SetString("ItemId", itemId)
                                                                         .SetString("SalesOrderId", salesOrderId)
                                                                         .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>() as List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>;
        }

        /// <summary>
        /// vevő összes nyitott rendeléseinek értéke
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public decimal OpenOrderAmount(string customerId)
        {
            try
            {
                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.SalesOrderOpenOrderAmount").SetString("CustomerId", customerId);

                return query.UniqueResult<decimal>();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //private static string ConvertOrderHeaderToSeparatedString(CompanyGroup.Domain.PartnerModule.SalesOrderCreate request)
        //{
        //    return AddBraketsToInstance(request.CustomerId) + '|' +
        //           AddBraketsToInstance(Helpers.ConvertData.ConvertIntToString(Helpers.ConvertData.ConvertBoolToInt(request.RequiredDelivery), "0")) + '|' +
        //           AddBraketsToInstance(request.DeliveryDate) + '|' +
        //           AddBraketsToInstance(Helpers.ConvertData.ConvertIntToString(Helpers.ConvertData.ConvertBoolToInt(request.PartialDelivery))) + '|' +
        //           AddBraketsToInstance(request.DeliveryId) + '|' +
        //           AddBraketsToInstance(request.DeliveryCompanyName) + '|' +
        //           AddBraketsToInstance(request.DeliveryZip) + '|' +
        //           AddBraketsToInstance(request.DeliveryCity) + '|' +
        //           AddBraketsToInstance(request.DeliveryStreet) + '|' +
        //           AddBraketsToInstance(request.DeliveryPhone) + '|' +
        //           AddBraketsToInstance(request.DeliveryEmail) + '|' +
        //           AddBraketsToInstance(String.Format("{0} {1} {2}", request.DeliveryZip, request.DeliveryCity, request.DeliveryStreet) ) + '|' +
        //           AddBraketsToInstance(Helpers.ConvertData.ConvertIntToString((int)request.SalesSource, "0")) + '|' +
        //           AddBraketsToInstance(request.ContactPersonId) + '|' +
        //           AddBraketsToInstance(request.InventLocationId);
        //}

        ///// <summary>
        ///// rendeles sor elemet egy pipline-al hatarolt karakterlancba fuzi ossze
        ///// </summary>
        ///// <param name="oItem"></param>
        ///// <returns></returns>
        //private static string ConvertOrderLineItemToSeparatedString(CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate request)
        //{
        //    return AddBraketsToInstance(request.ItemId) + "|" +
        //           AddBraketsToInstance(Helpers.ConvertData.ConvertIntToString(request.Qty, "0")) + "|" +
        //           AddBraketsToInstance(request.ConfigId);
        //}

        //private static string AddBraketsToInstance(string s)
        //{
        //    return "[" + s + "]";
        //}
    }
}
