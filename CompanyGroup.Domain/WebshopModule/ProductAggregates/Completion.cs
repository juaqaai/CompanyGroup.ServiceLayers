using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// Autosuggesting funkcióhoz tartozó termék eredménylista eleme
    /// </summary>
    public class Completion
    {
        /// <summary>
        /// Id	ProductId	DataAreaId	Name	EnglishName	PictureId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="recId"></param>
        public Completion(int id, string productId, string dataAreaId, string productName, string productNameEnglish, int pictureId)
        {
            this.Id = id;

            this.ProductId = productId;

            this.DataAreaId = dataAreaId;

            this.ProductName = productName;

            this.ProductNameEnglish = productNameEnglish;

            this.PictureId = pictureId;
        }

        public int Id { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductNameEnglish{ get; set; }

        public string DataAreaId { get; set; }

        public int PictureId { get; set; }
    }

    /// <summary>
    /// Autosuggesting funkcióhoz tartozó termék eredménylista
    /// </summary>
    public class CompletionList : List<Completion>
    { 
    }
}
