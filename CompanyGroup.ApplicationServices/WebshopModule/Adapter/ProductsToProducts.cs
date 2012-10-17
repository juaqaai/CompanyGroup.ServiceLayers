using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ProductsToProducts
    {
        /// <summary>
        /// domain terméklista -> DTO terméklista
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Products Map(CompanyGroup.Domain.WebshopModule.Products products)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.Products result = new CompanyGroup.Dto.WebshopModule.Products();

                result.Items = new List<CompanyGroup.Dto.WebshopModule.Product>();

                result.Items.AddRange(products.ConvertAll(x => new ProductToProduct().Map(x)));

                result.ListCount = products.ListCount;

                return result;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Products(); }
        }

    }
}
