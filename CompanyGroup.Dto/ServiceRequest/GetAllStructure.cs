using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetAllStructure
    {
        public List<string> ManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        public bool ActionFilter { get; set; }

        public bool HrpFilter { get; set; }

        public bool BscFilter { get; set; }

        public bool BargainFilter { get; set; }

        public bool IsInNewsletterFilter { get; set; }

        public bool NewFilter { get; set; }

        public bool StockFilter { get; set; }

        public string TextFilter { get; set; }

        public string PriceFilter { get; set; }

        public string PriceFilterRelation { get; set; }

        public GetAllStructure()
        {
            this.ManufacturerIdList = new List<string>();

            this.Category1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.Category3IdList = new List<string>();
        }
    }
}
