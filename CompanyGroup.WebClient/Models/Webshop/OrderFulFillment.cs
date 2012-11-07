using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class OrderFulFillment : CompanyGroup.Dto.WebshopModule.OrderFulFillment
    {
        public OrderFulFillment(CompanyGroup.Dto.WebshopModule.OrderFulFillment orderFulFillment, bool catalogueOpenStatus, bool shoppingCartOpenStatus, Visitor visitor)
        {
            this.Created = orderFulFillment.Created;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.WaitForAutoPost = orderFulFillment.WaitForAutoPost;

            this.Message = orderFulFillment.Message;

            this.StoredItems = orderFulFillment.StoredItems;

            this.OpenedItems = orderFulFillment.OpenedItems;

            this.ActiveCart = orderFulFillment.ActiveCart;

            this.LeasingOptions = orderFulFillment.LeasingOptions;

            LineMessages = orderFulFillment.LineMessages;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.Visitor = visitor;
        }

        public Visitor Visitor { get; set; }

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }
    }
}