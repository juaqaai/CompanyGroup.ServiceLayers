using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// megrendelések
    /// </summary>
    public class SalesOrderService : ServiceBase, ISalesOrderService
    {

        private CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository;

        private CompanyGroup.Domain.PartnerModule.IChangeTrackingRepository changeTrackingRepository;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public SalesOrderService(CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                                 CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository,
                                 CompanyGroup.Domain.PartnerModule.IChangeTrackingRepository changeTrackingRepository,
                                 CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
            if (salesOrderRepository == null)
            {
                throw new ArgumentNullException("SalesOrderRepository");
            }

            this.salesOrderRepository = salesOrderRepository;

            this.changeTrackingRepository = changeTrackingRepository;
        }

        /// <summary>
        /// megrendelés információk lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.OrderInfoList GetOrderInfo(CompanyGroup.Dto.PartnerModule.GetOrderInfoRequest request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                //vevőrendelések változáskövetése   
                List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfoCT> lineInfosCt = changeTrackingRepository.SalesLineCT(0);

                List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> lineInfos = lineInfosCt.ConvertAll(x =>
                {
                    return new CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo(0, x.DataAreaId, x.SalesId, x.CreatedDate, x.ShippingDateRequested, x.CurrencyCode, x.Payment,
                                                                                       x.SalesHeaderType, x.SalesHeaderStatus, x.CustomerOrderNo, x.WithDelivery, x.LineNum, x.SalesStatus,
                                                                                       x.ProductId, x.ProductName, x.Quantity, x.SalesPrice, x.LineAmount, x.SalesDeliverNow, x.RemainSalesPhysical,
                                                                                       (int) x.StatusIssue, x.InventLocationId, x.ItemDate, x.FileName, x.InStock, x.AvailableInWebShop);
                });

                //látogató alapján kikeresett vevő rendelések listája
                List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> lineInfosRepository = salesOrderRepository.GetOrderDetailedLineInfo(visitor.CustomerId, request.CanBeTaken, request.SalesStatus, request.CustomerOrderNo, request.ItemName, request.ItemId, request.SalesOrderId);

                lineInfos.AddRange(lineInfosRepository);

                 //megrendelés info aggregátum elkészítése
                IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>> groupedLineInfos = lineInfos.GroupBy(x => x.SalesId).OrderByDescending(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

                List<CompanyGroup.Domain.PartnerModule.OrderInfo> orderInfos = new List<CompanyGroup.Domain.PartnerModule.OrderInfo>();

                foreach (var lineInfo in groupedLineInfos)
                { 
                    CompanyGroup.Domain.PartnerModule.OrderInfo orderInfo = CompanyGroup.Domain.PartnerModule.OrderInfo.Create(lineInfo.ToList());

                    orderInfos.Add(orderInfo);
                }

                //nyitott rendelések összesen
                decimal openOrderAmount = salesOrderRepository.OpenOrderAmount(visitor.CustomerId);

                CompanyGroup.Domain.PartnerModule.OrderInfoList orderInfoList = new CompanyGroup.Domain.PartnerModule.OrderInfoList(openOrderAmount, orderInfos);

                //konverzió dto-ra
                List<CompanyGroup.Dto.PartnerModule.OrderInfo> orderInfoListDTO = new OrderInfoToOrderInfo().Map(orderInfoList);

                return new CompanyGroup.Dto.PartnerModule.OrderInfoList(openOrderAmount, orderInfoListDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
