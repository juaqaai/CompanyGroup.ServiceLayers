using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class OrderFulFillment
    {
        public OrderFulFillment(CompanyGroup.Dto.WebshopModule.OrderFulFillment orderFulFillment, bool catalogueOpenStatus, bool shoppingCartOpenStatus, 
                                                                                                  Visitor visitor)
        {
            this.Created = orderFulFillment.Created;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.WaitForAutoPost = orderFulFillment.WaitForAutoPost;

            this.Message = orderFulFillment.Message;

            this.StoredItems = orderFulFillment.StoredItems;

            this.OpenedItems = orderFulFillment.OpenedItems;

            this.ActiveCart = orderFulFillment.ActiveCart;

            this.LeasingOptions = orderFulFillment.LeasingOptions;

            this.LineMessages = orderFulFillment.LineMessages;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.Visitor = visitor;

            this.HrpOrderId = orderFulFillment.HrpOrderId;

            this.BscOrderId = orderFulFillment.BscOrderId;

            this.HrpSecondHandOrderId = orderFulFillment.HrpSecondHandOrderId;

            this.BscSecondHandOrderId = orderFulFillment.BscSecondHandOrderId;
        }

        public Visitor Visitor { get; set; }

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

        /// <summary>
        /// direktben elkészült-e a rendelés, vagy nem
        /// </summary>
        public bool Created { get; set; }

        /// <summary>
        /// ha direktben nem készülz el a rendelés, akkor ennek a státusznak kell igaznak lennie.
        /// </summary>
        public bool WaitForAutoPost { get; set; }

        public string Message { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        public List<string> LineMessages { get; set; }

        public string HrpOrderId { get; set; }

        public string BscOrderId { get; set; }

        public string BscSecondHandOrderId { get; set; }

        public string HrpSecondHandOrderId { get; set; }

    }
}