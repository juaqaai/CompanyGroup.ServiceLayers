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
            return new CompanyGroup.Domain.PartnerModule.Visitor()
                       {
                           AutoLogin = false,
                           Currency = String.Empty,
                           CustomerId = String.Empty,
                           CustomerName = String.Empty,
                           DataAreaId = String.Empty,
                           CustomerPriceGroups = new List<CustomerPriceGroup>(),
                           DefaultPriceGroupId = String.Empty,
                           Email = String.Empty,
                           ExpireDate = DateTime.MinValue,
                           Id = 0,
                           InventLocationId = String.Empty,
                           LanguageId = String.Empty,
                           LoggedIn = false,
                           LoginDate = DateTime.MinValue,
                           LoginIP = String.Empty,
                           LoginType = LoginType.None,
                           LogoutDate = DateTime.MinValue,
                           PartnerModel = PartnerModel.None,
                           PaymTermId = String.Empty,
                           Permission = new Permission(),
                           PersonId = String.Empty,
                           PersonName = String.Empty,
                           RecId = 0,
                           Representative = new Representative(),
                           Roles = new List<string>(),
                           Status = LoginStatus.Passive,
                           Valid = false,
                           VisitorId = String.Empty
                       };
        }
    }
}
