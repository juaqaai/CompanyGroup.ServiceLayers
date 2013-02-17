using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ProductCatalogue  
    {
        /// <summary>
        /// termékkatalógus
        /// </summary>
        /// <param name="products"></param>
        /// <param name="visitor"></param>
        /// <param name="isCatalogueOpenStatus"></param>
        public ProductCatalogue(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.WebClient.Models.Visitor visitor, bool catalogueOpenStatus)
        {
            this.Products = products;

            //this.Structures = structures;

            this.Visitor = visitor;

            this.CatalogueOpenStatus = catalogueOpenStatus;
        }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        //public CompanyGroup.Dto.WebshopModule.Structures Structures { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

        /// <summary>
        /// terméklista nyitott állapotú-e, vagy nem
        /// </summary>
        public bool CatalogueOpenStatus { get; set; }

    }
}
