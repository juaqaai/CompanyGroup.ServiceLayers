using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class ComparableCollection
    {
        /// <summary>
        /// látogató, aki az összehasonlítást használja
        /// </summary>
        public CompanyGroup.Domain.PartnerModule.Visitor Visitor { get; set; }

        /// <summary>
        /// összehasonlításra kijelölt termékek
        /// </summary>
        public CompanyGroup.Domain.WebshopModule.Products Products { get; set; }

        public Structure Structure { get; set; }

        /// <summary>
        /// összehasonlítható-e a cikk, vagy nem (ha a jelleg1 és a jelleg2 egyezik, akkor igen, egyébként nem)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Comparable(Product product)
        {
            return this.Structure.Category1.CategoryId.Equals(product.Structure.Category1.CategoryId) && this.Structure.Category2.CategoryId.Equals(product.Structure.Category2.CategoryId);
        }

    }
}
