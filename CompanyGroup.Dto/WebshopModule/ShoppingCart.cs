using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCart", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Items = new List<ShoppingCartItem>();
            this.SumTotal = 0;
            this.Id = "";
            this.Shipping = new Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.MinValue, InvoiceAttached = false, Street = "", ZipCode = "" };
            this.PaymentTerms = 0;
            this.DeliveryTerms = 0;
        }

        public ShoppingCart(List<ShoppingCartItem> items, double sumTotal, string id, Shipping shipping, int paymentTerms, int deliveryTerms)
        {
            this.Items = items;
            this.SumTotal = sumTotal;
            this.Id = id;
            this.Shipping = shipping;
            this.PaymentTerms = paymentTerms;
            this.DeliveryTerms = deliveryTerms;
        }

        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<ShoppingCartItem> Items { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "SumTotal", Order = 2)]
        public double SumTotal { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 3)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Shipping", Order = 4)]
        public Shipping Shipping { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DeliveryTerms", Order = 5)]
        public int DeliveryTerms { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "PaymentTerms", Order = 6)]
        public int PaymentTerms { get; set; }
    }
}
