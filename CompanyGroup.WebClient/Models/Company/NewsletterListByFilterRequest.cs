using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class NewsletterListByFilterRequest
    {
        public List<string> NewsletterIdList { get; set; }

        public List<string> ManufacturerIdList { get; set; }

        public List<string> DisplayConditionManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> DisplayConditionCategory1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> DisplayConditionCategory2IdList { get; set; }

        public bool DisplayIfConditionsAreEmpty { get; set; }

        public NewsletterListByFilterRequest()
        {
            this.NewsletterIdList = new List<string>();

            this.ManufacturerIdList = new List<string>();

            this.DisplayConditionManufacturerIdList = new List<string>();

            this.Category1IdList = new List<string>();

            this.DisplayConditionCategory1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.DisplayConditionCategory2IdList = new List<string>();

            this.DisplayIfConditionsAreEmpty = true;
        }
    }
}