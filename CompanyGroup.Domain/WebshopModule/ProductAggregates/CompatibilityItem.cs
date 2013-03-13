using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// eredetei termékazonosítóhoz kapcsolt kompatibilis termékkód
    /// </summary>
    public class CompatibilityItem
    {
        public CompatibilityItem(string productId, string dataAreaId, string compatibleProductId, int compatibilityType)
        {
            this.ProductId = compatibleProductId;

            this.DataAreaId = dataAreaId;

            this.CompatibilityType = (CompatibilityType) compatibilityType;
        }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }

        public CompatibilityType CompatibilityType { get; set; }
    }
}
