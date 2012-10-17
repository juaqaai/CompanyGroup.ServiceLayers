using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    public class CategoryToCategory
    {
        public CompanyGroup.GlobalServices.Dto.Category Map(CompanyGroup.Dto.WebshopModule.Category category)
        {
            try
            {
                return new CompanyGroup.GlobalServices.Dto.Category() { Id = category.Id, Name = category.Name };
            }
            catch { return new CompanyGroup.GlobalServices.Dto.Category(); }
        }
    }
}
