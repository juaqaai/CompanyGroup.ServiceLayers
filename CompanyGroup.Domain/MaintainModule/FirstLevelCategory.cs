using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public class FirstLevelCategory
    {
        public FirstLevelCategory(string category1Id, string category1Name, string category1NameEnglish, string dataAreaId)
        {
            this.Category1Id = category1Id;

            this.Category1Name = category1Name;

            this.Category1NameEnglish = category1NameEnglish;

            this.DataAreaId = dataAreaId;
        }

        public string Category1Id { get; set; }

        public string Category1Name { get; set; }

        public string Category1NameEnglish { get; set; }

        public string DataAreaId { get; set; }
    }
}
