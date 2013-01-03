using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class CompatibilityItem
    {
        public CompatibilityItem(string itemId, string dataAreaId)
        { 
            ItemId = itemId;

            DataAreaId = dataAreaId;
        }

        public string ItemId { get; set; }

        public string DataAreaId { get; set; }
    }
}
