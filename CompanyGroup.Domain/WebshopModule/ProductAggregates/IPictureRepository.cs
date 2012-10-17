using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IPictureRepository
    {
        /// <summary>
        /// képek lista termékazonosító és vállalatkód szerint 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.Picture> GetListByProduct(string productId, string dataAreaId);

        /// <summary>
        /// képek lista elsődleges kulcs szerint
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.Picture> GetListByKey(string objectId);
    }
}
