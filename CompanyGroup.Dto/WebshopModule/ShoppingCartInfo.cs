using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    public class ShoppingCartInfo
    {
        public ShoppingCartInfo() : this(new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(), 
                                         new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(), 
                                         new CompanyGroup.Dto.WebshopModule.ShoppingCart(), 
                                         new CompanyGroup.Dto.WebshopModule.LeasingOptions())
        { 
        }

        public ShoppingCartInfo(List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedItems, 
                                List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedItems, 
                                CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart, 
                                CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions)
        {
            this.StoredItems = storedItems;

            this.OpenedItems = openedItems;

            this.ActiveCart = activeCart;

            this.LeasingOptions = leasingOptions;
        }

        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }
}
