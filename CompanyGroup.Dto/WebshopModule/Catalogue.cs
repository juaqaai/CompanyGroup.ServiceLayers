using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// termékkatalógus
    /// </summary>
    public class Catalogue
    {
        public Catalogue() : this(new CompanyGroup.Dto.WebshopModule.Products(), new CompanyGroup.Dto.WebshopModule.Structures(), new List<CompanyGroup.Dto.WebshopModule.BannerProduct>()) { }

        public Catalogue(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.Dto.WebshopModule.Structures structures, List<CompanyGroup.Dto.WebshopModule.BannerProduct> bannerProducts)
        {
            this.Products = products;

            this.Structures = structures;

            this.BannerProducts = bannerProducts;
        }

        /// <summary>
        /// terméklista
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        /// <summary>
        /// struktúrák
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.Structures Structures { get; set; }

        /// <summary>
        /// termék bannerek
        /// </summary>
        public List<CompanyGroup.Dto.WebshopModule.BannerProduct> BannerProducts { get; set; }
    }
}
