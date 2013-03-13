using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// domain secondhand value object
    /// Id	ProductId	ConfigId	InventLocationId	Quantity	Price	StatusDescription	DataAreaId
    /// </summary>
    public class SecondHand
    {
        public SecondHand(int id, string productId, string configId, string inventLocationId, int quantity, int price, string statusDescription, string dataAreaId)
        {
            this.Id = id;

            this.ProductId = productId;

            this.ConfigId = configId;

            this.InventLocationId = inventLocationId;

            this.Quantity = quantity;

            this.Price = price;

            this.StatusDescription = statusDescription;

            this.DataAreaId = dataAreaId;

            this.CustomerPrice = 0;
        }

        public SecondHand() : this(0, "", "", "", 0, 0, "", "") { }

        public int Id { get; set; }

        public string ProductId { get; set; }

        public string ConfigId { get; set; }

        public string InventLocationId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        /// <summary>
        /// vevőre érvényes ár, kalkulált érték
        /// </summary>
        public decimal CustomerPrice { get; set; }

        public string StatusDescription { get; set; }

        public string DataAreaId { get; set; }
    }

    /// <summary>
    /// domain secondhand value object list
    /// </summary>
    public class SecondHandList : List<SecondHand> 
    {
        public SecondHandList(List<SecondHand> secondHandList)
        {
            this.AddRange(secondHandList);
        }

        public SecondHandList() { }
    }
}
