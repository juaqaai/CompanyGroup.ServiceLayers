using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    //[Serializable]
    //[System.Runtime.Serialization.DataContract(Name = "ShoppingCartInfo", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCartInfo
    {
        public ShoppingCartInfo()
        {
            this.StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

            this.OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

            this.ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions();

            //this.FinanceOffer = new CompanyGroup.Dto.WebshopModule.FinanceOffer(); 
        }

        //[System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 1)]
        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 2)]
        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 3)]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "LeasingOptions", Order = 4)]
        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        //[System.Runtime.Serialization.DataMember(Name = "FinanceOffer", Order = 5)]
        //public CompanyGroup.Dto.WebshopModule.FinanceOffer FinanceOffer { get; set; }
    }
}
