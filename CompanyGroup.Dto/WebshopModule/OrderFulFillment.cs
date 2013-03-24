using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// rendelés válasz objektum
    /// </summary>
    [Serializable]
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

            this.IsValidated = false;

            this.HrpOrderId = String.Empty;

            this.BscOrderId = String.Empty;

            this.HrpSecondHandOrderId = String.Empty;

            this.BscSecondHandOrderId = String.Empty;
        }

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

        /// <summary>
        /// validált a kosár, vagy nem. (üzenet a Message-ben)
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// hrp megrendelés azonosító
        /// </summary>
        public string HrpOrderId { get; set; }

        /// <summary>
        /// bsc megrendelés azonosító
        /// </summary>
        public string BscOrderId { get; set; }

        /// <summary>
        /// bsc használt cikk megrendelés azonosító
        /// </summary>
        public string BscSecondHandOrderId { get; set; }

        /// <summary>
        /// hrp használt cikk megrendelés azonosító
        /// </summary>
        public string HrpSecondHandOrderId { get; set; }
    }
}
