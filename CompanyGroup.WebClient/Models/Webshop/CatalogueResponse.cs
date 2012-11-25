using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// bejelentkezés eredményére visszaadott objektumok csoportja
    /// </summary>
    public class CatalogueResponse
    {
        public CatalogueResponse(CompanyGroup.Dto.WebshopModule.Products products,
                         CompanyGroup.WebClient.Models.Visitor visitor,
                         CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart, 
                         List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedItems, 
                         List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedItems,
                         bool shoppingCartOpenStatus, 
                         bool catalogueOpenStatus, 
                         CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses,
                         CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions)
        {
            this.Products = products;   

            this.Visitor = visitor;

            this.ActiveCart = activeCart;

            this.OpenedItems = openedItems;

            this.StoredItems = storedItems;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.DeliveryAddresses = deliveryAddresses;

            this.LeasingOptions = leasingOptions;
        }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; } 

        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; } 

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses DeliveryAddresses { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }

}
