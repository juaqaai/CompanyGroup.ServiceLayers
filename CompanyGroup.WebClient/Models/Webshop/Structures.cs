using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// struktúra model 
    /// </summary>
    public class Structures
    {
        public Structures()
        {
            this.Manufacturers = new Manufacturers();

            this.FirstLevelCategories = new FirstLevelCategories();

            this.SecondLevelCategories = new SecondLevelCategories();

            this.ThirdLevelCategories = new ThirdLevelCategories();
        }

        public Structures(CompanyGroup.Dto.WebshopModule.Structures structures)
        {
            this.Manufacturers = new Manufacturers();

            this.FirstLevelCategories = new FirstLevelCategories();

            this.SecondLevelCategories = new SecondLevelCategories();

            this.ThirdLevelCategories = new ThirdLevelCategories();

            this.FirstLevelCategories.AddRange(structures.FirstLevelCategories.ConvertAll(x => new StructureItem(x.Id, x.Name)));

            this.Manufacturers.AddRange(structures.Manufacturers.ConvertAll(x => new ManufacturerItem(x.Id, x.Name)));

            this.SecondLevelCategories.AddRange(structures.SecondLevelCategories.ConvertAll(x => new StructureItem(x.Id, x.Name)));

            this.ThirdLevelCategories.AddRange(structures.ThirdLevelCategories.ConvertAll(x => new StructureItem(x.Id, x.Name)));
        }

        public Manufacturers Manufacturers { get; set; }

        public FirstLevelCategories FirstLevelCategories { get; set; }

        public SecondLevelCategories SecondLevelCategories { get; set; }

        public ThirdLevelCategories ThirdLevelCategories { get; set; }
    }
}
