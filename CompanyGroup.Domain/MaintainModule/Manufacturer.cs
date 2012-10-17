using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    public class Manufacturer
    {
        public Manufacturer(string manufacturerId, string manufacturerName, string manufacturerNameEnglish, string dataAreaId)
        {
            this.ManufacturerId = manufacturerId;

            this.ManufacturerName = manufacturerName;

            this.ManufacturerNameEnglish = manufacturerNameEnglish;

            this.DataAreaId = dataAreaId;
        }

        public string ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public string ManufacturerNameEnglish { get; set; }

        public string DataAreaId { get; set; }
    }
}
