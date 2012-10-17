using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class CategoryToCategory
    {
        public CompanyGroup.Dto.WebshopModule.Category Map(CompanyGroup.Domain.WebshopModule.Category category)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.Category() { Id = category.CategoryId, Name = category.CategoryName, EnglishName = category.CategoryEnglishName };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Category(); }
        }
    }
}
