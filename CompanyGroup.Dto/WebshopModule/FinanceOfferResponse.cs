using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// finance ajánlatkérés válasz objektum
    /// </summary>
    public class FinanceOfferResponse
    {
        public FinanceOfferResponse()
        {
            this.EmaiNotification = false;

            this.Message = String.Empty;

            //this.StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

            //this.OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

            //this.ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions(); 
        }

        public bool EmaiNotification { get; set; }

        public string Message { get; set; }

        //public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        //public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        //public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }
}
