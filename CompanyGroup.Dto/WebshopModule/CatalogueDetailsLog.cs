using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// részletes termékadatlap látogatottsági listaelem DTO
    /// </summary>
    public class CatalogueDetailsLog
    {
        public CatalogueDetailsLog()
        {
            this.ProductId = String.Empty;

            this.DataAreaId = String.Empty;

            this.ProductName = String.Empty;

            this.EnglishProductName = String.Empty;

            this.PictureId = 0;
        }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }

        public string ProductName { get; set; }

        public string EnglishProductName { get; set; }

        public int PictureId { get; set; }
    }

    /// <summary>
    /// részletes termékadatlap látogatottsági lista DTO    
    /// </summary>
    public class CatalogueDetailsLogList
    {
        public CatalogueDetailsLogList()
        {
            this.Items = new List<CatalogueDetailsLog>();
        }

        public List<CatalogueDetailsLog> Items { get; set; }
    }
}
