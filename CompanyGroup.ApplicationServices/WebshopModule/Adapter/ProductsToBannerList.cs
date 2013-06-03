using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ProductsToBannerList
    {
        /// <summary>
        /// domain terméklista -> DTO terméklista
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.BannerList Map(List<CompanyGroup.Domain.WebshopModule.Product> products)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.BannerList result = new CompanyGroup.Dto.WebshopModule.BannerList();

                result.Items = new List<CompanyGroup.Dto.WebshopModule.BannerProduct>();

                result.Items.AddRange(products.ConvertAll(x => new ProductToBannerProduct().Map(x)));

                return result;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.BannerList(); }
        }

    }
}
