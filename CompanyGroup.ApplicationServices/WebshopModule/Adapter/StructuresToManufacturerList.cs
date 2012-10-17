using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class StructuresToManufacturerList
    {
        /// <summary>
        /// domain struktúra lista -> dto gyártó lista
        /// </summary>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="structures"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.WebshopModule.Manufacturer> Map(List<string> category1IdList, List<string> category2IdList, List<string> category3IdList, CompanyGroup.Domain.WebshopModule.Structures structures)
        {
            List<CompanyGroup.Dto.WebshopModule.Manufacturer> manufacturerList = (from structure in structures
                                                                    where (structure.Manufacturer.ManufacturerId != "") && (structure.Manufacturer.ManufacturerName != "") &&
                                                                          (category1IdList.Exists(c1 => c1.Equals(structure.Category1.CategoryId)) || (category1IdList.Count.Equals(0))) &&
                                                                          (category2IdList.Exists(c2 => c2.Equals(structure.Category2.CategoryId)) || (category2IdList.Count.Equals(0))) &&
                                                                          (category3IdList.Exists(c3 => c3.Equals(structure.Category3.CategoryId)) || (category3IdList.Count.Equals(0)))
                                                                    orderby structure.Manufacturer.ManufacturerName
                                                                    group structure by new { structure.Manufacturer.ManufacturerId, structure.Manufacturer.ManufacturerName, structure.Manufacturer.ManufacturerEnglishName}
                                                                    into grp
                                                                    select ConstructManufacturer(grp.Key.ManufacturerId, grp.Key.ManufacturerName, grp.Key.ManufacturerEnglishName)).ToList();
            return manufacturerList;
        }

        private CompanyGroup.Dto.WebshopModule.Manufacturer ConstructManufacturer(string id, string name, string englishName)
        {
            return new CompanyGroup.Dto.WebshopModule.Manufacturer() { Id = id, Name = name, EnglishName = englishName };
        }
    }
}
