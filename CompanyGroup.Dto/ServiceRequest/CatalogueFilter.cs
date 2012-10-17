using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class CatalogueFilter
    {
        public string DataAreaId { get; set; }

        public List<string> ManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        public bool ActionFilter { get; set; }

        public bool BargainFilter { get; set; }

        public bool NewFilter { get; set; }

        public bool StockFilter { get; set; }

        public string TextFilter { get; set; }

        public CatalogueFilter()
        {
            this.ManufacturerIdList = new List<string>();

            this.Category1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.Category3IdList = new List<string>();
        }
    }
}
