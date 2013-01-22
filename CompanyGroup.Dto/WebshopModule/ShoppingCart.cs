using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCart", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCart
    {
        public ShoppingCart() : this(new List<ShoppingCartItem>(), 0, 0, new Shipping(), 0, 0)
        {
        }

        public ShoppingCart(List<ShoppingCartItem> items, double sumTotal, int id, Shipping shipping, int paymentTerms, int deliveryTerms)
        {
            this.Items = items;
            this.SumTotal = sumTotal;
            this.Id = id;
            this.Shipping = shipping;
            this.PaymentTerms = paymentTerms;
            this.DeliveryTerms = deliveryTerms;
        }

        public List<ShoppingCartItem> Items { get; set; }

        public double SumTotal { get; set; }

        public int Id { get; set; }

        public Shipping Shipping { get; set; }

        public int DeliveryTerms { get; set; }

        public int PaymentTerms { get; set; }
    }
}
