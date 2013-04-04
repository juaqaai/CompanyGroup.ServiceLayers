﻿using System;
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
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> GetOrderDetailedLineInfo(string customerId);
    }
}
