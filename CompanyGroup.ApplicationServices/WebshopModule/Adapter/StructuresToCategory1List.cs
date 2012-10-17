using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class StructuresToCategory1List
    {
        /// <summary>
        /// domain struktúra lista -> dto kategória lista
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="structures"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.WebshopModule.Category> Map(List<string> manufacturerIdList, List<string> category2IdList, List<string> category3IdList, CompanyGroup.Domain.WebshopModule.Structures structures)
        {
            List<CompanyGroup.Dto.WebshopModule.Category> category1List = (from structure in structures
                                                             where (structure.Category1.CategoryId != "") && (structure.Category1.CategoryName != "") &&
                                                                   (manufacturerIdList.Exists(m => m.Equals(structure.Manufacturer.ManufacturerId)) || (manufacturerIdList.Count.Equals(0))) &&
                                                                   (category2IdList.Exists(c2 => c2.Equals(structure.Category2.CategoryId)) || (category2IdList.Count.Equals(0))) &&
                                                                   (category3IdList.Exists(c3 => c3.Equals(structure.Category3.CategoryId)) || (category3IdList.Count.Equals(0)))
                                                             orderby structure.Category1.CategoryName
                                                                           group structure by new { structure.Category1.CategoryId, structure.Category1.CategoryName, structure.Category1.CategoryEnglishName }
                                                                 into grp select ConstructCategory(grp.Key.CategoryId, grp.Key.CategoryName, grp.Key.CategoryEnglishName)).ToList();
            return category1List;
        }

        private CompanyGroup.Dto.WebshopModule.Category ConstructCategory(string id, string name, string englishName)
        {
            return new CompanyGroup.Dto.WebshopModule.Category() { Id = id, Name = name, EnglishName = englishName };
        }

    }
}
