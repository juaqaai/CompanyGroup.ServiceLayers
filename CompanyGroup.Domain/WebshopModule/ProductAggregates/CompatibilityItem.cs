using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class CompatibilityItem
    {
        public CompatibilityItem(string productId, string dataAreaId, int compatibilityType)
        { 
            this.ProductId = productId;

            this.DataAreaId = dataAreaId;

            this.CompatibilityType = (CompatibilityType) compatibilityType;
        }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }

        public CompatibilityType CompatibilityType { get; set; }
    }
}
