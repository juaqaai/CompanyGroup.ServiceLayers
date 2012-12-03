using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public class ThirdLevelCategory
    {
        public ThirdLevelCategory(string category3Id, string category3Name, string category3NameEnglish, string dataAreaId)
        {
            this.Category3Id = category3Id;

            this.Category3Name = category3Name;

            this.Category3NameEnglish = category3NameEnglish;

            this.DataAreaId = dataAreaId;
        }

        public string Category3Id { get; set; }

        public string Category3Name { get; set; }

        public string Category3NameEnglish { get; set; }

        public string DataAreaId { get; set; }
    }
}
