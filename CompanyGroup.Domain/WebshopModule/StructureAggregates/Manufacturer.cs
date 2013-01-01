using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// gyártó 
    /// </summary>
    public class Manufacturer : CompanyGroup.Domain.Core.ValueObject<Manufacturer>
    {
        public Manufacturer(string manufacturerId, string manufacturerName, string manufacturerEnglishName)
        {
            this.ManufacturerId = manufacturerId;

            this.ManufacturerName = manufacturerName;

            this.ManufacturerEnglishName = manufacturerEnglishName;
        }
        
        public Manufacturer() : this(String.Empty, String.Empty, String.Empty) { }

        public string ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public string ManufacturerEnglishName { get; set; }
    }
}
