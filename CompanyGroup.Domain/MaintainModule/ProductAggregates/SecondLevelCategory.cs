using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public class SecondLevelCategory
    {
        public SecondLevelCategory(string category2Id, string category2Name, string category2NameEnglish, string dataAreaId)
        {
            this.Category2Id = category2Id;

            this.Category2Name = category2Name;

            this.Category2NameEnglish = category2NameEnglish;

            this.DataAreaId = dataAreaId;
        }

        public string Category2Id { get; set; }

        public string Category2Name { get; set; }

        public string Category2NameEnglish { get; set; }

        public string DataAreaId { get; set; }
    }
}
