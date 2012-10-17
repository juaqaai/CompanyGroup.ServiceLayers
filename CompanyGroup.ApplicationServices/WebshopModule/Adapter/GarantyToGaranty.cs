using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class GarantyToGaranty
    {
        /// <summary>
        /// Garanty domain -> Garanty dto.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Garanty Map(CompanyGroup.Domain.WebshopModule.Garanty from)
        {
            return new CompanyGroup.Dto.WebshopModule.Garanty() { Mode = from.Mode, Time = from.Time };
        }
    }
}
