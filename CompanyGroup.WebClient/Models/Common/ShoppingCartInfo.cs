using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ShoppingCartInfo
    {
        public ShoppingCartInfo(List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedItems,
                                List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedItems,
                                CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart,
                                CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions, 
                                Visitor visitor, 
                                bool catalogueOpenStatus,
                                bool shoppingCartOpenStatus, 
                                string currency)
        {
            this.StoredItems = storedItems;

            this.OpenedItems = openedItems;

            this.ActiveCart = activeCart;

            this.LeasingOptions = leasingOptions;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.Visitor = visitor;

            this.Currency = currency;
        }

        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        public Visitor Visitor { get; set; }

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
