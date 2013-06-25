using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// ShoppingCart + LeasingOptions
    /// </summary>
    public class ShoppingCartAndLeasingOptions
    {
        public ShoppingCartAndLeasingOptions() : this(new List<ShoppingCartItem>(), 0, 0, new Shipping(), 0, 0, new CompanyGroup.Dto.WebshopModule.LeasingOptions(), String.Empty, false, false, false)
        {
        }

        public ShoppingCartAndLeasingOptions(List<ShoppingCartItem> items, double sumTotal, int id, Shipping shipping, int paymentTerms, int deliveryTerms, 
                                             CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions, string currency, bool allInStock,
                                             bool hasNotEnoughSecondHandStock, bool hasNotEnoughEndOfSalesStock)
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

            this.HasNotEnoughSecondHandStock = hasNotEnoughSecondHandStock;

            this.HasNotEnoughEndOfSalesStock = hasNotEnoughEndOfSalesStock;
        }

        /// <summary>
        /// kosár elemek
        /// </summary>
        public List<ShoppingCartItem> Items { get; set; }

        /// <summary>
        /// kosár összesen
        /// </summary>
        public double SumTotal { get; set; }

        public int Id { get; set; }

        public Shipping Shipping { get; set; }

        public int DeliveryTerms { get; set; }

        public int PaymentTerms { get; set; }

        /// <summary>
        /// lízing opciók
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// minden termék a kosárban raktáron van-e?
        /// </summary>
        public bool AllInStock { get; set; }

        /// <summary>
        /// van-e olyan leértékelt cikk a kosárban, aminek nincs elég készlete?
        /// </summary>
        /// <returns></returns>
        public bool HasNotEnoughSecondHandStock { get; set; }

        /// <summary>
        /// van-e olyan kifutó cikk a kosárban, aminek nincs elég készlete?
        /// </summary>
        /// <returns></returns>
        public bool HasNotEnoughEndOfSalesStock { get; set; }
    }
}
