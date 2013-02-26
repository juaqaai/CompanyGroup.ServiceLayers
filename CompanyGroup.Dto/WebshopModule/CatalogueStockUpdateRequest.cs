using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// készletváltozás értesítés a web adatbázis felé
    /// </summary>
    public class CatalogueStockUpdateRequest
    {
        public CatalogueStockUpdateRequest(string dataAreaId, string inventLocationId, string productId)
        {
            this.DataAreaId = dataAreaId;

            this.InventLocationId = inventLocationId;

            this.ProductId = productId;
        }

        public CatalogueStockUpdateRequest() : this("", "", "")
        { 
        }

        public string InventLocationId{ get; set; }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }
    }
}
