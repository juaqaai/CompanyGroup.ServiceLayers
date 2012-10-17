using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.WebshopModule
{
    public static class StructureFactory
    {
        public static CompanyGroup.Domain.WebshopModule.Structure CreateStructure(string manufacturerId, string manufacturerName, 
                                                                                  string category1Id, string category1Name, 
                                                                                  string category2Id, string category2Name, 
                                                                                  string category3Id, string category3Name)
        {
            return new CompanyGroup.Domain.WebshopModule.Structure()
            {
                Manufacturer = new CompanyGroup.Domain.WebshopModule.Manufacturer() { Id = manufacturerId, Name = manufacturerName },
                Category1 = new CompanyGroup.Domain.WebshopModule.Category() { Id = category1Id, Name = category1Name },
                Category2 = new CompanyGroup.Domain.WebshopModule.Category { Id = category2Id, Name = category2Name },
                Category3 = new CompanyGroup.Domain.WebshopModule.Category { Id = category3Id, Name = category3Name }
            };            
        }

        public static CompanyGroup.Domain.WebshopModule.Structure CreateStructure(Manufacturer manufacturer, Category category1, Category category2, Category category3)
        {
            return new CompanyGroup.Domain.WebshopModule.Structure() {Manufacturer =  manufacturer, Category1 = category1, Category2 = category2, Category3 = category3};           
        }
    }
}
