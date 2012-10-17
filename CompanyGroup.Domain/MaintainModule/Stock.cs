using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// adminisztrációs modul, készlet információ
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// készlet információ
        /// </summary>
        /// <param name="standardConfigId">IventTable.StandardConfigId, InventDim.InventDimId</param>
        /// <param name="productId">IventTable.ItemId</param>
        /// <param name="quantity">InventSum.AvailPhysical</param>
        /// <param name="inventLocationId">InventDim.InventLocationId</param>
        /// <param name="dataAreaId"></param>
        public Stock(string standardConfigId, string productId, int quantity, string inventLocationId, string dataAreaId)
        {
            this.StandardConfigId = standardConfigId;

            this.ProductId = productId;

            this.Quantity = quantity;

            this.InventLocationId = inventLocationId;

            this.DataAreaId = dataAreaId;
        }

        public string StandardConfigId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public string InventLocationId { get; set; }

        public string DataAreaId { get; set; }
    }
}
