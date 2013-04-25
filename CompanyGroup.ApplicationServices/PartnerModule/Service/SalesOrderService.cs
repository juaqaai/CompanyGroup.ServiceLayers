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

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public SalesOrderService(CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                                 CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository,
                                 CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
            if (salesOrderRepository == null)
            {
                throw new ArgumentNullException("SalesOrderRepository");
            }

            this.salesOrderRepository = salesOrderRepository;
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

                //látogató alapján kikeresett vevő rendelések listája
                List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> lineInfos = salesOrderRepository.GetOrderDetailedLineInfo(visitor.CustomerId, request.CanBeTaken, request.SalesStatus, request.CustomerOrderNo, request.ItemName, request.ItemId, request.SalesOrderId);

                 //megrendelés info aggregátum elkészítése
                IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>> groupedLineInfos = lineInfos.GroupBy(x => x.SalesId).OrderBy(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

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
