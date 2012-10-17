using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class FlagsToFlags
    {
        /// <summary>
        /// Domain flags -> DTO flags
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Flags Map(CompanyGroup.Domain.WebshopModule.Flags from)
        {
            return new CompanyGroup.Dto.WebshopModule.Flags() { InStock = from.InStock, New = from.New, IsInNewsletter = from.IsInNewsletter };
        }
    }
}
