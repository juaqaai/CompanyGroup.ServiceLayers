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

        public BannerProduct(int id, string productId, string partNumber, string dataAreaId, string itemName, string itemNameEnglish, 
            		         string manufacturerId, string manufacturerName, string manufacturerEnglishName,	
		                     string category1Id, string category1Name, string category1EnglishName, 
		                     string category2Id, string category2Name, string category2EnglishName,
                             string category3Id, string category3Name, string category3EnglishName,
                             int stock, int price1, int price2, int price3, int price4, int price5, 
                             int pictureId, string fileName, bool primary, long recId)
        {
            this.Id = id;

            this.ProductId = productId;

            this.PartNumber = partNumber;

            this.DataAreaId = dataAreaId;

            this.ItemName = itemName;

            this.ItemNameEnglish = itemNameEnglish;

            this.Structure = new Structure(manufacturerId, manufacturerName, manufacturerEnglishName,
                               category1Id, category1Name, category1EnglishName,
                               category2Id, category2Name, category2EnglishName,
                               category3Id, category3Name, category3EnglishName);   

            this.Stock = stock;

            this.Prices = new Prices(price1, price2, price3, price4, price5, "HUF");

            this.Picture = new Picture(pictureId, fileName, primary, recId);

            this.CustomerPrice = 0;
        }

        /// <summary>
        /// azonosító
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// megnevezés
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// angol megnevezés
        /// </summary>
        public string ItemNameEnglish { get; set; }

        /// <summary>
        /// struktúra az árképzéshez
        /// </summary>
        public Structure Structure { get; set; }

        /// <summary>
        /// készlet 
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// árak 1-5 és a valutanem, melyben értelmezve van
        /// </summary>
        public Prices Prices { set; get; }

        /// <summary>
        /// elsődleges kép
        /// </summary>
        public Picture Picture { get; set; }

        /// <summary>
        /// vállalat
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// vevőre érvényes ár, kalkulált érték
        /// </summary>
        public decimal CustomerPrice { get; set; }
    }

    /// <summary>
    /// termék banner lista
    /// </summary>
    public class BannerProducts : List<BannerProduct> 
    { 
    }
}
