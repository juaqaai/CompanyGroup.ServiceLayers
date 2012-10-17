using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// model factory class
    /// </summary>
    public static class Factory
    {
        public static CompanyGroup.Domain.PartnerModule.Visitor CreateVisitor(CompanyGroup.Domain.PartnerModule.LoginInfo loginInfo)
        {
            return new CompanyGroup.Domain.PartnerModule.Visitor(loginInfo);
        }

        public static CompanyGroup.Domain.PartnerModule.Visitor CreateVisitor()
        {
            return new CompanyGroup.Domain.PartnerModule.Visitor();
        }
    }
}
