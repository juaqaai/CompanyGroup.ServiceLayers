using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain PriceList -> DTO PriceList   
    /// </summary>
    public class PriceListToPriceList
    {
        public CompanyGroup.Dto.WebshopModule.PriceList Map(CompanyGroup.Domain.WebshopModule.PriceList priceList)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.PriceList result = new CompanyGroup.Dto.WebshopModule.PriceList();

                result.Items = new List<CompanyGroup.Dto.WebshopModule.PriceListItem>();

                result.Items.AddRange(priceList.ConvertAll(x => Convert(x)));

                return result;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.PriceList(); }
        }

        private static CompanyGroup.Dto.WebshopModule.PriceListItem Convert(CompanyGroup.Domain.WebshopModule.Product from)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.PriceListItem to = new CompanyGroup.Dto.WebshopModule.PriceListItem();
                to.Currency = from.Prices.Currency;
                to.DataAreaId = from.DataAreaId;
                to.Description = from.Description;
                to.EndOfSales = CompanyGroup.Domain.Core.Adapter.ConvertItemStateEnumToBool(from.ItemState);
                to.FirstLevelCategory = new CategoryToCategory().Map(from.Structure.Category1);
                to.GarantyMode = from.Garanty.Mode;
                to.GarantyTime = from.Garanty.Time;
                to.InnerStock = from.Stock.Inner;
                to.ItemName = from.ProductName;
                to.Manufacturer = new ManufacturerToManufacturer().Map(from.Structure.Manufacturer);
                to.New = from.New;
                to.OuterStock = from.Stock.Outer;
                to.PartNumber = from.PartNumber;
                to.Price = from.CustomerPrice;
                to.ProductId = from.ProductId;
                to.PurchaseInProgress = from.PurchaseInProgress();
                to.SecondLevelCategory = new CategoryToCategory().Map(from.Structure.Category2);
                to.ShippingDate = from.ShippingDate;
                to.ThirdLevelCategory = new CategoryToCategory().Map(from.Structure.Category3);
                return to;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.PriceListItem(); }
        }
    }
}
