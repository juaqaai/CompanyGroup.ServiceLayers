using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// termékkatalógus
    /// </summary>
    public class Catalogue
    {
        public Catalogue() { }

        public Catalogue(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.Dto.WebshopModule.Structures structures)
        {
            Products = products;

            Structures = structures;
        }

        /// <summary>
        /// terméklista
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        /// <summary>
        /// struktúrák
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.Structures Structures { get; set; }
    }
}
