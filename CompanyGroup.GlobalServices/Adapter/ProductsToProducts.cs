using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    public class ProductsToProducts
    {
        /// <summary>
        /// domain terméklista -> DTO terméklista
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public List<CompanyGroup.GlobalServices.Dto.Product> Map(CompanyGroup.Dto.WebshopModule.Products products)
        {
            try
            {
                List<CompanyGroup.GlobalServices.Dto.Product> result = new List<CompanyGroup.GlobalServices.Dto.Product>();

                result.AddRange(products.Items.ConvertAll(x => new ProductToProduct().Map(x)));

                return result;
            }
            catch { return new List<CompanyGroup.GlobalServices.Dto.Product>(); }
        }

    }
}
