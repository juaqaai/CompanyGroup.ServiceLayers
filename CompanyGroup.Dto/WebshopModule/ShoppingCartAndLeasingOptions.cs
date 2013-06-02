using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// ShoppingCart + LeasingOptions
    /// </summary>
    public class ShoppingCartAndLeasingOptions
    {
        public ShoppingCartAndLeasingOptions() : this(new List<ShoppingCartItem>(), 0, 0, new Shipping(), 0, 0, new CompanyGroup.Dto.WebshopModule.LeasingOptions(), String.Empty, false)
        {
        }

        public ShoppingCartAndLeasingOptions(List<ShoppingCartItem> items, double sumTotal, int id, Shipping shipping, int paymentTerms, int deliveryTerms, CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions, string currency, bool allInStock)
        {
            this.Items = items;

            this.SumTotal = sumTotal;

            this.Id = id;

            this.Shipping = shipping;

            this.PaymentTerms = paymentTerms;

            this.DeliveryTerms = deliveryTerms;

            this.LeasingOptions = leasingOptions;

            this.Currency = currency;

            this.AllInStock = allInStock;
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
