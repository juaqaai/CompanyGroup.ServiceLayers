using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// finance ajánlatkérés válasz objektum
    /// </summary>
    //[Serializable]
    //[System.Runtime.Serialization.DataContract(Name = "FinanceOfferFulFillment", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class FinanceOfferFulFillment
    {
        public FinanceOfferFulFillment()
        {
            this.EmaiNotification = false;

            this.Message = String.Empty;

            //this.StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

            //this.OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

            //this.ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions(); 
        }

        //[System.Runtime.Serialization.DataMember(Name = "EmaiNotification", Order = 1)]
        public bool EmaiNotification { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "Message", Order = 2)]
        public string Message { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 3)]
        //public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 4)]
        //public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 5)]
        //public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "LeasingOptions", Order = 6)]
        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }
}
