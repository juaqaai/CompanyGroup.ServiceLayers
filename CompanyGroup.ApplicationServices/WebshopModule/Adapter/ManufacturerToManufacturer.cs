using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ManufacturerToManufacturer
    {
        public CompanyGroup.Dto.WebshopModule.Manufacturer Map(CompanyGroup.Domain.WebshopModule.Manufacturer manufacturer)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.Manufacturer() { Id = manufacturer.ManufacturerId, Name = manufacturer.ManufacturerName, EnglishName = manufacturer.ManufacturerEnglishName };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Manufacturer(); }
        }
    }
}
