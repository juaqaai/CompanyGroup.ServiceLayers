using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class ProductListComplationRequest
    {
        public string Prefix { get; set; }

        public List<string> ManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        public bool DiscountFilter { get; set; }

        public bool SecondhandFilter { get; set; }

        public bool HrpFilter { get; set; }

        public bool BscFilter { get; set; }

        public bool IsInNewsletterFilter { get; set; }

        public bool NewFilter { get; set; }

        public bool StockFilter { get; set; }

        public string TextFilter { get; set; }

        public string PriceFilter { get; set; }

        public string PriceFilterRelation { get; set; }

        /// <summary>
        /// konstruktorban listák létrehozása - nullreference exception 
        /// </summary>
        public ProductListComplationRequest()
        {
            this.Prefix = String.Empty;

            this.ManufacturerIdList = new List<string>();
            this.Category1IdList = new List<string>();
            this.Category2IdList = new List<string>();
            this.Category3IdList = new List<string>();

            this.DiscountFilter = false;
            this.SecondhandFilter = false;
            this.HrpFilter = false;
            this.BscFilter = false;
            this.IsInNewsletterFilter = false;
            this.NewFilter = false;
            this.StockFilter = false;
            this.TextFilter = String.Empty;
            this.PriceFilter = String.Empty;
            this.PriceFilterRelation = String.Empty;
        }
    }
}

