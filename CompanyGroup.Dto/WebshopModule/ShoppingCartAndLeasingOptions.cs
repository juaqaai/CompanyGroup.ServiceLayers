using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// ShoppingCart + LeasingOptions
    /// </summary>
    public class ShoppingCartAndLeasingOptions
    {
        public ShoppingCartAndLeasingOptions()
        {
            this.Items = new List<ShoppingCartItem>();
            this.SumTotal = 0;
            this.Id = 0;
            this.Shipping = new Shipping();
            this.PaymentTerms = 0;
            this.DeliveryTerms = 0;

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions();

            this.Currency = String.Empty;

            this.AllInStock = false;
        }

        public List<ShoppingCartItem> Items { get; set; }

        public double SumTotal { get; set; }

        public int Id { get; set; }

        public Shipping Shipping { get; set; }

        public int DeliveryTerms { get; set; }

        public int PaymentTerms { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// minden termék a kosárban raktáron van-e?
        /// </summary>
        public bool AllInStock { get; set; }
    }
}
