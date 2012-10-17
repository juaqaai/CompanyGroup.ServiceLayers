using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface INewsletterRepository
    {
        /// <summary>
        /// hírlevél lista
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="businessUnitId"></param>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.Newsletter> GetNewsletterList(int topN, string dataAreaId, string businessUnitId, string manufacturerId);
    }
}
