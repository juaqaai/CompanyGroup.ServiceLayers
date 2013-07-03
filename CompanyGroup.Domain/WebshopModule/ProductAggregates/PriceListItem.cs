using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class PriceListItem : CompanyGroup.Domain.WebshopModule.Product
    { 
        public PriceListItem(int id, string productId, string axStructCode, string dataAreaId, string standardConfigId, string name, string englishName, string partNumber, 	
                       string manufacturerId, string manufacturerName, string manufacturerEnglishName, 
                       string category1Id, string category1Name, string category1EnglishName, 
                       string category2Id, string category2Name, string category2EnglishName, 
                       string category3Id, string category3Name, string category3EnglishName,	
                       int stock, int averageInventory, int price1, int price2, int price3, int price4, int price5, int customerPrice, 
                       string garantyTime, string garantyMode, 
                       bool discount, bool newItem, int itemState, string description, string englishDescription, int productManagerId,
                       DateTime shippingDate, bool isPurchaseOrdered, DateTime createdDate, DateTime updated, bool available, int pictureId, bool secondHand, bool valid) : base(id, productId, axStructCode, dataAreaId, standardConfigId, name, englishName, partNumber, 	
                       manufacturerId, manufacturerName, manufacturerEnglishName, 
                       category1Id, category1Name, category1EnglishName, 
                       category2Id, category2Name, category2EnglishName, 
                       category3Id, category3Name, category3EnglishName,	
                       stock, averageInventory, price1, price2, price3, price4, price5, customerPrice, 
                       garantyTime, garantyMode, 
                       discount, newItem, itemState, description, englishDescription, productManagerId,
                       shippingDate, isPurchaseOrdered, createdDate, updated, available, pictureId, secondHand, valid) { }
    }

    /// <summary>
    /// domain árlista entitások listája
    /// </summary>
    public class PriceList : List<CompanyGroup.Domain.WebshopModule.PriceListItem>
    {
        public PriceList(List<CompanyGroup.Domain.WebshopModule.PriceListItem> priceListItems)
        {
            this.AddRange(priceListItems);
        }
    }
}
