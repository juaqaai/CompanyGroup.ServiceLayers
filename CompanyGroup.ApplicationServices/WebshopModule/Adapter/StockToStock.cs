using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class StockToStock
    {
        /// <summary>
        /// domain stock -> dto stock
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Stock Map(CompanyGroup.Domain.WebshopModule.Stock from)
        {
            return new CompanyGroup.Dto.WebshopModule.Stock() { Inner = from.Inner, Outer = from.Outer };
        }
    }
}
