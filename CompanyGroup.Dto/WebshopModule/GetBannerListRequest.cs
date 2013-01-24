using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetBannerListRequest
    {
        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        public bool HrpFilter { get; set; }

        public bool BscFilter { get; set; }

        public string VisitorId { get; set; }

        public string Currency { get; set; }

        /// <summary>
        /// konstruktorban inicializálás
        /// </summary>
        public GetBannerListRequest()
        {
            this.Category1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.Category3IdList = new List<string>();

            this.HrpFilter = false;

            this.BscFilter = false;

            this.VisitorId = String.Empty;

            this.Currency = String.Empty;
        }
    }
}
