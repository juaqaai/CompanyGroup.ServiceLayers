using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class ProductCatalogueItem
    {
        /// <summary>
        /// termékkatalógus elem
        /// </summary>
        /// <param name="product"></param>
        /// <param name="compatibleProducts"></param>
        /// <param name="reverseCompatibleProducts"></param>
        /// <param name="visitor"></param>
        public ProductCatalogueItem(CompanyGroup.Dto.WebshopModule.Product product, 
                                    List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> compatibleProducts,
                                    List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> reverseCompatibleProducts,
                                    CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Product = product;

            this.ReverseCompatibleProducts = reverseCompatibleProducts;

            this.CompatibleProducts = compatibleProducts;

            this.Visitor = visitor;
        }

        public CompanyGroup.Dto.WebshopModule.Product Product { get; set; }

        /// <summary>
        /// kompatibilitás lista - mibe jó?
        /// </summary>
        public List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> CompatibleProducts { get; set; }

        /// <summary>
        /// kompatibilitás lista - mi jó hozzá?
        /// </summary>
        public List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> ReverseCompatibleProducts { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }
    }
}