using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// terméklista szűrő összeállítása
    /// </summary>
    public class GetAllProductRequest
    {
        public List<string> ManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        public bool ActionFilter { get; set; }

        public bool BargainFilter { get; set; }

        public bool HrpFilter { get; set; }

        public bool BscFilter { get; set; }

        public bool IsInNewsletterFilter { get; set; }

        public bool NewFilter { get; set; }

        public bool StockFilter { get; set; }

        public string TextFilter { get; set; }

        public string PriceFilter { get; set; }

        public string PriceFilterRelation { get; set; }

        public int Sequence { get; set; }

        public int CurrentPageIndex { get; set; }

        public int ItemsOnPage { get; set; }

        public string VisitorId { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// konstruktor - inicializálás
        /// </summary>
        public GetAllProductRequest()
        {
            this.ManufacturerIdList = new List<string>();
            this.Category1IdList = new List<string>();
            this.Category2IdList = new List<string>();
            this.Category3IdList = new List<string>();

            this.ActionFilter = false;
            this.BargainFilter = false;
            this.HrpFilter = false;
            this.BscFilter = false;
            this.IsInNewsletterFilter = false;
            this.NewFilter = false;
            this.StockFilter = false;
            this.TextFilter = String.Empty;
            this.PriceFilter = "0";
            this.PriceFilterRelation = "0";
            this.Sequence = 0;
            this.CurrentPageIndex = 1;
            this.ItemsOnPage = 30;
            this.VisitorId = String.Empty;
            this.Currency = String.Empty;
        }
    }
}
