using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// rendelés válasz objektum
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "OrderFulFillment", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class OrderFulFillment
    {
        public OrderFulFillment()
        {
            this.Created = false;

            this.WaitForAutoPost = false;

            this.Message = String.Empty;

            this.StoredItems = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

            this.OpenedItems = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

            this.ActiveCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions();

            this.LineMessages = new List<string>();
        }

        /// <summary>
        /// direktben elkészült-e a rendelés, vagy nem
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Created", Order = 1)]
        public bool Created { get; set; }

        /// <summary>
        /// ha direktben nem készülz el a rendelés, akkor ennek a státusznak kell igaznak lennie.
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "WaitForAutoPost", Order = 2)]
        public bool WaitForAutoPost { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 3)]
        public string Message { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "StoredItems", Order = 4)]
        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "OpenedItems", Order = 5)]
        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ActiveCart", Order = 6)]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LeasingOptions", Order = 7)]
        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LineMessages", Order = 8)]
        public List<string> LineMessages { get; set; }
    }
}
