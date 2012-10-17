using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// Domain megrendelés info -> DTO megrendelés info
    /// </summary>
    public class OrderInfoToOrderInfo
    {
        public List<CompanyGroup.Dto.PartnerModule.OrderInfo> Map(List<CompanyGroup.Domain.PartnerModule.OrderInfo> from)
        {
            return from.ConvertAll(x => ConvertOrderInfo(x));
        }

        private CompanyGroup.Dto.PartnerModule.OrderInfo ConvertOrderInfo(CompanyGroup.Domain.PartnerModule.OrderInfo from)
        {
            return new CompanyGroup.Dto.PartnerModule.OrderInfo() { CreatedDate = from.CreatedDate, SalesId = from.SalesId, Lines = from.OrderLines.ConvertAll(x => ConvertOrderLineInfo(x)) };
        }

        private CompanyGroup.Dto.PartnerModule.OrderLineInfo ConvertOrderLineInfo(CompanyGroup.Domain.PartnerModule.OrderLineInfo from)
        {
            return new CompanyGroup.Dto.PartnerModule.OrderLineInfo()
                       {
                           CurrencyCode = from.CurrencyCode,
                           InventLocationId = from.InventLocationId,
                           ItemId = from.ItemId,
                           LineAmount = from.LineAmount,
                           Name = from.Name,
                           Payment = from.Payment,
                           Quantity = from.Quantity,
                           SalesPrice = String.Format(((Math.Round(from.SalesPrice) == from.SalesPrice) ? "{0:0}" : "{0:0.00}"), from.SalesPrice),
                           ShippingDateRequested = from.ShippingDateRequested,
                           StatusIssue = (int) from.StatusIssue
                       };
        }
    }
}
