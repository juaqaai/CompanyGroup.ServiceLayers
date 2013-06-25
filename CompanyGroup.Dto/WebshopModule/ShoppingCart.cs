using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class ShoppingCart
    {
        public ShoppingCart() : this(new List<ShoppingCartItem>(), 0, 0, new Shipping(), 0, 0, false, false, false)
        {
        }

        public ShoppingCart(List<ShoppingCartItem> items, double sumTotal, int id, Shipping shipping, int paymentTerms, int deliveryTerms, bool allInStock, bool hasNotEnoughSecondHandStock, bool hasNotEnoughEndOfSalesStock)
        {
            this.Items = items;
            this.SumTotal = sumTotal;
            this.Id = id;
            this.Shipping = shipping;
            this.PaymentTerms = paymentTerms;
            this.DeliveryTerms = deliveryTerms;
            this.AllInStock = allInStock;
            this.HasNotEnoughSecondHandStock = hasNotEnoughSecondHandStock;
            this.HasNotEnoughEndOfSalesStock = hasNotEnoughEndOfSalesStock;
        }

        public List<ShoppingCartItem> Items { get; set; }

        public double SumTotal { get; set; }

        public int Id { get; set; }

        public Shipping Shipping { get; set; }

        public int DeliveryTerms { get; set; }

        public int PaymentTerms { get; set; }

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
