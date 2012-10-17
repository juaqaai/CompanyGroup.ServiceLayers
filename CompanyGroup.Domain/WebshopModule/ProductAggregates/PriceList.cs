using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// domain árlista entitások listája
    /// </summary>
    public class PriceList : List<CompanyGroup.Domain.WebshopModule.Product>
    {
        public PriceList(List<CompanyGroup.Domain.WebshopModule.Product> products)
        {
            this.AddRange(products);
        }
    }
}
