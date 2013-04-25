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
            string month = (from.CreatedDate.Month < 10) ? String.Format("0{0}", from.CreatedDate.Month) : String.Format("{0}", from.CreatedDate.Month);

            string day = (from.CreatedDate.Day < 10) ? String.Format("0{0}", from.CreatedDate.Day) : String.Format("{0}", from.CreatedDate.Day);

            return new CompanyGroup.Dto.PartnerModule.OrderInfo() 
                       {
                           CreatedDate = String.Format("{0}.{1}.{2}", from.CreatedDate.Year, month, day), 
                           DataAreaId = from.DataAreaId, 
                           HeaderSalesStatus = from.HeaderSalesStatus, 
                           Payment = from.Payment, 
                           SalesHeaderType = from.SalesHeaderType, 
                           SalesId = from.SalesId, 
                           WithDelivery = from.WithDelivery, 
                           Lines = from.OrderLines.ConvertAll(x => ConvertOrderLineInfo(x)),
                           SumAmountBrutto = String.Format(((Math.Round(from.SumAmountBrutto) == from.SumAmountBrutto) ? "{0:0}" : "{0:0.00}"), from.SumAmountBrutto),
                           SumAmountNetto = String.Format(((Math.Round(from.SumAmountNetto) == from.SumAmountNetto) ? "{0:0}" : "{0:0.00}"), from.SumAmountNetto)
                       };
        }

        private CompanyGroup.Dto.PartnerModule.OrderLineInfo ConvertOrderLineInfo(CompanyGroup.Domain.PartnerModule.OrderLineInfo from)
        {
            return new CompanyGroup.Dto.PartnerModule.OrderLineInfo()
                       {
                           CurrencyCode = from.CurrencyCode,
                           InventLocationId = from.InventLocationId, Id = from.Id, 
                           ItemId = from.ItemId,
                           LineAmount = from.LineAmount,
                           LineAmountBrutto = from.LineAmountBrutto,
                           Name = from.Name,
                           Payment = from.Payment,
                           Quantity = from.Quantity,
                           RemainSalesPhysical = from.RemainSalesPhysical, 
                           SalesPrice = String.Format(((Math.Round(from.SalesPrice) == from.SalesPrice) ? "{0:0}" : "{0:0.00}"), from.SalesPrice),
                           ShippingDateRequested = from.ShippingDateRequested,
                           StatusIssue = (int) from.StatusIssue, 
                           SalesDeliverNow = from.SalesDeliverNow, 
                           AvailableInWebShop = from.AvailableInWebShop, 
                           InStock = from.InStock,
                           PictureExists = from.PictureExists                    
                       };
        }
    }
}
