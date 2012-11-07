using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class FinanceOfferFulFillment : CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment
    {
        public FinanceOfferFulFillment(CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment financeOfferFulFillment, bool catalogueOpenStatus, bool shoppingCartOpenStatus, Visitor visitor)
        {
            this.ActiveCart = financeOfferFulFillment.ActiveCart;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.EmaiNotification = financeOfferFulFillment.EmaiNotification;

            this.LeasingOptions = financeOfferFulFillment.LeasingOptions;

            this.Message = financeOfferFulFillment.Message;

            this.OpenedItems = financeOfferFulFillment.OpenedItems;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.StoredItems = financeOfferFulFillment.StoredItems;

            this.Visitor = visitor;
        }
        
        public Visitor Visitor {get; set;}

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

    }
}