using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IPictureRepository
    {
        /// <summary>
        /// képek lista termékazonosító szerint 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.Picture> GetListByProduct(string productId);
    }
}
