using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class CatalogueStockUpdateRequest
    {
        public CatalogueStockUpdateRequest(string dataAreaId, string productId, int stock)
        {
            this.Stock = stock;

            this.ProductId = productId;

            this.DataAreaId = dataAreaId;
        }

        public CatalogueStockUpdateRequest() : this("", "", 0)
        { 
        }

        public int Stock { get; set; }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }
    }
}
