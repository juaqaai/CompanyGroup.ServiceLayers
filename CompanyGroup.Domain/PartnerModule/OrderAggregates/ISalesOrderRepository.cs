using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface ISalesOrderRepository
    {
        CompanyGroup.Domain.PartnerModule.ProductOrderCheck GetProductOrderCheck(string productId, string dataAreaId, int quantity);

        CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult Create(CompanyGroup.Domain.PartnerModule.SalesOrderCreate request);

        CompanyGroup.Domain.PartnerModule.SecondhandOrderCreateResult CreateSecondHandOrder(CompanyGroup.Domain.PartnerModule.SecondhandOrderCreate request);

        /// <summary>
        /// részletes vevőrendelés sorok listája
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="canBeTaken"></param>
        /// <param name="salesStatus"></param>
        /// <param name="customerOrderNo"></param>
        /// <param name="itemName"></param>
        /// <param name="itemId"></param>
        /// <param name="salesOrderId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> GetOrderDetailedLineInfo(string customerId, bool canBeTaken, int salesStatus, string customerOrderNo, string itemName, string itemId, string salesOrderId);

        /// <summary>
        /// vevő összes nyitott rendeléseinek értéke
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        decimal OpenOrderAmount(string customerId);
    }
}
