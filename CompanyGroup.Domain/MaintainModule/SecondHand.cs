using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// adminisztrációs modul, leértékelt termékek info
    /// </summary>
    public class SecondHand
    {
        /// <summary>
        /// leértékelt termékek információ
        /// </summary>
        /// <param name="configId">.ConfigId, InventDim.InventDimId</param>
        ///<param name="inventLocationId">InventDim.InventLocationId</param>
        /// <param name="productId">IventTable.ItemId</param>
        /// <param name="quantity">InventSum.AvailPhysical</param>
        /// <param name="price"></param>
        /// <param name="dataAreaId"></param>
        public SecondHand(string configId, string inventLocationId, string productId, int quantity, int price, string statusDescription, string dataAreaId)
        {
            this.ConfigId = configId;

            this.InventLocationId = inventLocationId;

            this.ProductId = productId;

            this.Quantity = quantity;

            this.Price = price;

            this.StatusDescription = statusDescription;

            this.DataAreaId = dataAreaId;
        }

        public string ConfigId { get; set; }

        public string InventLocationId { get; set; }
        
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public string StatusDescription { get; set;}

        public string DataAreaId { get; set; }
    }
}
