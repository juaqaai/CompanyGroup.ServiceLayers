using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class CatalogueStockUpdate
    {
        public CatalogueStockUpdate(string dataAreaId, string productId, int stock)
        {
            this.Stock = stock;

            this.ProductId = productId;

            this.DataAreaId = dataAreaId;
        }

        public CatalogueStockUpdate() : this( "", "", 0)
        { 
        }

        public int Stock { get; set; }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }
    }
}
