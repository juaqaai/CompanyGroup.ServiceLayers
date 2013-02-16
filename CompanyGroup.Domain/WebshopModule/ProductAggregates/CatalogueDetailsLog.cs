using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    public class CatalogueDetailsLog
    {
        public CatalogueDetailsLog(string productId, string dataAreaId, string productName, string englishProductName, int pictureId)
        {
            this.ProductId = productId;

            this.DataAreaId = dataAreaId;

            this.ProductName = productName;

            this.EnglishProductName = englishProductName;

            this.PictureId = pictureId;
        }

        public int Id { get; set; }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }

        public string ProductName { get; set; }
    
        public string EnglishProductName { get; set; }

        public int PictureId { get; set; }
    }
}
