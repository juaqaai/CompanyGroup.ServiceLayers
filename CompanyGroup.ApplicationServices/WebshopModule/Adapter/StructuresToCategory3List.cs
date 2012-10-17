using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class StructuresToCategory3List
    {
        /// <summary>
        /// domain struktúra lista -> dto kategória lista
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="structures"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.WebshopModule.Category> Map(List<string> manufacturerIdList, List<string> category1IdList, List<string> category2IdList, CompanyGroup.Domain.WebshopModule.Structures structures)
        {

            List<CompanyGroup.Dto.WebshopModule.Category> category3List = (from structure in structures
                                                             where (structure.Category3.CategoryId != "") && (structure.Category3.CategoryName != "") &&
                                                                   (manufacturerIdList.Exists(m => m.Equals(structure.Manufacturer.ManufacturerId)) || (manufacturerIdList.Count.Equals(0))) &&
                                                                   (category1IdList.Exists(c1 => c1.Equals(structure.Category1.CategoryId)) || (category1IdList.Count.Equals(0))) &&
                                                                   (category2IdList.Exists(c2 => c2.Equals(structure.Category2.CategoryId)) || (category2IdList.Count.Equals(0)))
                                                             orderby structure.Category3.CategoryName
                                                             group structure by new { structure.Category3.CategoryId, structure.Category3.CategoryName, structure.Category3.CategoryEnglishName }
                                                                 into grp select ConstructCategory(grp.Key.CategoryId, grp.Key.CategoryName, grp.Key.CategoryEnglishName)).ToList();
            return category3List;
        }

        private CompanyGroup.Dto.WebshopModule.Category ConstructCategory(string id, string name, string englishName)
        {
            return new CompanyGroup.Dto.WebshopModule.Category() { Id = id, Name = name, EnglishName = englishName };
        }
    }
}
