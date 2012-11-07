using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    //public class StoredOpenedShoppingCartCollection : CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection
    //{
    //    public StoredOpenedShoppingCartCollection() : this(new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>(), new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>()) { }

    //    public StoredOpenedShoppingCartCollection(List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedShoppingCart, List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedShoppingCart)
    //    {
    //        this.StoredItems.AddRange(storedShoppingCart);

    //        this.OpenedItems.AddRange(openedShoppingCart);

    //        this.ErrorMessage = String.Empty;
    //    }

    //    public string ErrorMessage { get; set; }
    //}

    //public class StoredShoppingCartCollection : List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>
    //{
    //    public StoredShoppingCartCollection(List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedShoppingCarts)
    //    {
    //        this.AddRange(storedShoppingCarts);
    //    }
    //}

    //public class OpenedShoppingCartCollection : List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>
    //{
    //    public OpenedShoppingCartCollection(List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedShoppingCarts)
    //    {
    //        this.AddRange(openedShoppingCarts);
    //    }
    //}

    public class ShoppingCartInfo : CompanyGroup.Dto.WebshopModule.ShoppingCartInfo
    {
        public ShoppingCartInfo(CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo, Visitor visitor, bool catalogueOpenStatus, bool shoppingCartOpenStatus)
        { 
            this.ActiveCart = shoppingCartInfo.ActiveCart;
            this.CatalogueOpenStatus = catalogueOpenStatus;
            this.ErrorMessage = "";
            this.FinanceOffer = shoppingCartInfo.FinanceOffer;
            this.LeasingOptions = shoppingCartInfo.LeasingOptions;
            this.OpenedItems = shoppingCartInfo.OpenedItems;
            this.StoredItems = shoppingCartInfo.StoredItems;
            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;
            this.Visitor = visitor;
        }

        public string ErrorMessage { get; set; }

        public Visitor Visitor { get; set; }

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

    }
}
