using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// model factory class
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// látogató létrehozás 
        /// </summary>
        /// <returns></returns>
        public static CompanyGroup.Domain.PartnerModule.Visitor CreateVisitor()
        {
            return new CompanyGroup.Domain.PartnerModule.Visitor(new List<CompanyGroup.Domain.PartnerModule.VisitorData>());
        }
    }
}
