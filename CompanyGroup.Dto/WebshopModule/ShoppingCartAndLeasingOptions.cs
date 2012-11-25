using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// ShoppingCart + LeasingOptions
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "ShoppingCartAndLeasingOptions", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class ShoppingCartAndLeasingOptions
    {
        public ShoppingCartAndLeasingOptions()
        {
            this.Items = new List<ShoppingCartItem>();
            this.SumTotal = 0;
            this.Id = "";
            this.Shipping = new Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.MinValue, InvoiceAttached = false, Street = "", ZipCode = "" };
            this.PaymentTerms = 0;
            this.DeliveryTerms = 0;

            this.LeasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions(); 
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

        [System.Runtime.Serialization.DataMember(Name = "LeasingOptions", Order = 7)]
        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }
    }
}
