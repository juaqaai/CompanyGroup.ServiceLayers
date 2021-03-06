﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetPriceListRequest
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

        public string VisitorId { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// konstruktorban listák létrehozása - nullreference exception 
        /// </summary>
        public GetPriceListRequest()
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
            this.PriceFilter = String.Empty;
            this.PriceFilterRelation = String.Empty;
            this.Sequence = 0;
            this.VisitorId = String.Empty;
            this.Currency = String.Empty;
        }
    }
}
