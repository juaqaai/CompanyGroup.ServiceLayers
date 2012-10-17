using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// terméklista szűrő összeállítása
    /// </summary>
    public class GetAllProduct
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

        public string NameOrPartNumberFilter { get; set; }

        public int Sequence { get; set; }

        public int CurrentPageIndex { get; set; }

        public int ItemsOnPage { get; set; }

        public string VisitorId { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// konstruktorban listák létrehozása - nullreference exception 
        /// </summary>
        public GetAllProduct()
        {
            this.ManufacturerIdList = new List<string>();

            this.Category1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.Category3IdList = new List<string>();
        }
    }
}
