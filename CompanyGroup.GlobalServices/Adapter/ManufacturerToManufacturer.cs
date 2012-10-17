using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    public class ManufacturerToManufacturer
    {
        public CompanyGroup.GlobalServices.Dto.Manufacturer Map(CompanyGroup.Dto.WebshopModule.Manufacturer manufacturer)
        {
            try
            {
                return new CompanyGroup.GlobalServices.Dto.Manufacturer() { Id = manufacturer.Id, Name = manufacturer.Name };
            }
            catch { return new CompanyGroup.GlobalServices.Dto.Manufacturer(); }
        }
    }
}
