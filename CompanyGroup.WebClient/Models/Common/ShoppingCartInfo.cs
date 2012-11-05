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
        public string ErrorMessage { get; set; }

        public static CompanyGroup.WebClient.Models.ShoppingCartInfo CreateShoppingCartInfo()
        {
            return new CompanyGroup.WebClient.Models.ShoppingCartInfo()
            {
                ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart()
                {
                    DeliveryTerms = 1,
                    Id = "",
                    Items = new List<CompanyGroup.Dto.WebshopModule.ShoppingCartItem>(),
                    PaymentTerms = 0,
                    Shipping = new CompanyGroup.Dto.WebshopModule.Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.Now, InvoiceAttached = false, Street = "", ZipCode = "" },
                    SumTotal = 0
                },
                ErrorMessage = "",
                OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>(),
                StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>()
            };
        }
    }
}
