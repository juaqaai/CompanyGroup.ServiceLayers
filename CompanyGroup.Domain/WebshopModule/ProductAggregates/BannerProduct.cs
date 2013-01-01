using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// termék banner   
    /// </summary>
    public class BannerProduct
    {

        public BannerProduct(int id, string productId, string partNumber, string dataAreaId, string itemName, string itemNameEnglish, int innerStock, int outerStock, int price1, int price2, int price3, int price4, int price5, int pictureId)
        {
            this.Id = id;

            this.ProductId = productId;

            this.PartNumber = partNumber;

            this.DataAreaId = dataAreaId;

            this.ItemName = itemName;

            this.ItemNameEnglish = itemNameEnglish;

            this.Stock = new Stock(innerStock, outerStock);

            this.Prices = new Prices(price1, price2, price3, price4, price5, "HUF");

            this.PictureId = pictureId;
        }

        public int Id { get; set; }

        public string ProductId { get; set; }

        public string PartNumber { get; set; }

        public string ItemName { get; set; }

        public string ItemNameEnglish { get; set; }

        /// <summary>
        /// készletek, inner - outer
        /// </summary>
        public Stock Stock { get; set; }

        /// <summary>
        /// árak 1-5 és a valutanem, melyben értelmezve van
        /// </summary>
        public Prices Prices { set; get; }

        public int PictureId { get; set; }

        public string DataAreaId { get; set; }
    }

    /// <summary>
    /// termék banner lista
    /// </summary>
    public class BannerProducts : List<BannerProduct> 
    { 
    }
}
