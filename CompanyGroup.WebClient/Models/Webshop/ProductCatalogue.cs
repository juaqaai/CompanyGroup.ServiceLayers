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
        /// <param name="catalogueOpenStatus"></param>
        /// <param name="sequence"></param>
        /// <param name="isActiveFilter"></param>
        public ProductCatalogue(CompanyGroup.Dto.WebshopModule.Products products, CompanyGroup.WebClient.Models.Visitor visitor, bool catalogueOpenStatus, int sequence, bool isActiveFilter)
        {
            this.Products = products;

            this.Visitor = visitor;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.Sequence = sequence;

            this.IsActiveFilter = isActiveFilter;
        }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

        /// <summary>
        /// terméklista nyitott állapotú-e, vagy nem
        /// </summary>
        public bool CatalogueOpenStatus { get; set; }

        /// <summary>
        /// listarendezés sorrendje
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// van-e aktív szűrési feltétel beállítva, vagy nincs
        /// </summary>
        public bool IsActiveFilter { get; set; }

    }
}
