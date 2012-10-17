using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class StructuresToStructures
    {

        /// <summary>
        /// domain struktúra -> DTO struktúra
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Structures Map(List<string> manufacturerIdList, List<string> category1IdList, List<string> category2IdList, List<string> category3IdList, CompanyGroup.Domain.WebshopModule.Structures from)
        {
            manufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));
            
            category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            //CompanyGroup.Dto.Structures to = new CompanyGroup.Dto.Structures();

            //List<CompanyGroup.Core.Domain.Structure> structures0 = new List<CompanyGroup.Core.Domain.Structure>(from);
            //List<CompanyGroup.Core.Domain.Structure> structures1 = new List<CompanyGroup.Core.Domain.Structure>(from);
            //List<CompanyGroup.Core.Domain.Structure> structures2 = new List<CompanyGroup.Core.Domain.Structure>(from);
            //List<CompanyGroup.Core.Domain.Structure> structures3 = new List<CompanyGroup.Core.Domain.Structure>(from);

            //List<CompanyGroup.Dto.Manufacturer> manufacturerList = (from structure in structures0
            //                        where (structure.ManufacturerId != "") && (structure.ManufacturerName != "") &&
            //                              (structure.Category1Id.Equals(category1Id) || String.IsNullOrEmpty(category1Id)) &&
            //                              (structure.Category2Id.Equals(category2Id) || String.IsNullOrEmpty(category2Id)) &&
            //                              (structure.Category3Id.Equals(category3Id) || String.IsNullOrEmpty(category3Id))
            //                        orderby structure.ManufacturerName
            //                        group structure by new { structure.ManufacturerId, structure.ManufacturerName }
            //                            into grp
            //                            select new CompanyGroup.Dto.Manufacturer
            //                            {
            //                                Id = grp.Key.ManufacturerId,
            //                                Name = grp.Key.ManufacturerName
            //                            }).ToList();

            //List<CompanyGroup.Dto.Category> category1List = (from structure in structures1
            //                     where (structure.Category1Id != "") && (structure.Category1Name != "") &&
            //                           (structure.ManufacturerId.Equals(manufacturerId) || String.IsNullOrEmpty(manufacturerId)) &&
            //                           (structure.Category2Id.Equals(category2Id) || String.IsNullOrEmpty(category2Id)) &&
            //                           (structure.Category3Id.Equals(category3Id) || String.IsNullOrEmpty(category3Id))
            //                     orderby structure.Category1Name
            //                     group structure by new { structure.Category1Id, structure.Category1Name }
            //                         into grp
            //                         select new CompanyGroup.Dto.Category
            //                         {
            //                             Id = grp.Key.Category1Id,
            //                             Name = grp.Key.Category1Name
            //                         }).ToList();

            //List<CompanyGroup.Dto.Category> category2List = (from structure in structures2
            //                     where (structure.Category2Id != "") && (structure.Category2Name != "") &&
            //                           (structure.ManufacturerId.Equals(manufacturerId) || String.IsNullOrEmpty(manufacturerId)) &&
            //                           (structure.Category1Id.Equals(category1Id) || String.IsNullOrEmpty(category1Id)) &&
            //                           (structure.Category3Id.Equals(category3Id) || String.IsNullOrEmpty(category3Id))
            //                     orderby structure.Category2Name
            //                     group structure by new { structure.Category2Id, structure.Category2Name }
            //                         into grp
            //                         select new CompanyGroup.Dto.Category
            //                         {
            //                             Id = grp.Key.Category2Id,
            //                             Name = grp.Key.Category2Name
            //                         }).ToList();

            //List<CompanyGroup.Dto.Category> category3List = (from structure in structures3
            //                     where (structure.Category3Id != "") && (structure.Category3Name != "") &&
            //                           (structure.ManufacturerId.Equals(manufacturerId) || String.IsNullOrEmpty(manufacturerId)) &&
            //                           (structure.Category1Id.Equals(category1Id) || String.IsNullOrEmpty(category1Id)) &&
            //                           (structure.Category2Id.Equals(category2Id) || String.IsNullOrEmpty(category2Id))
            //                     orderby structure.Category3Name
            //                     group structure by new { structure.Category3Id, structure.Category3Name }
            //                         into grp
            //                         select new CompanyGroup.Dto.Category
            //                         {
            //                             Id = grp.Key.Category3Id,
            //                             Name = grp.Key.Category3Name
            //                         }).ToList();

            return new CompanyGroup.Dto.WebshopModule.Structures()
            {
                Manufacturers = new StructuresToManufacturerList().Map(category1IdList, category2IdList, category3IdList, from),
                FirstLevelCategories = new StructuresToCategory1List().Map(manufacturerIdList, category2IdList, category3IdList, from),
                SecondLevelCategories = new StructuresToCategory2List().Map(manufacturerIdList, category1IdList, category3IdList, from),
                ThirdLevelCategories = new StructuresToCategory3List().Map(manufacturerIdList, category1IdList, category2IdList, from)
            };
        }            


    }
}
