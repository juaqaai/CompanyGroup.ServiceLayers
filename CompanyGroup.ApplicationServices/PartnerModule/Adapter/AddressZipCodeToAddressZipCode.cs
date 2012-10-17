using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    /// <summary>
    /// Domain címhez tartozó irányítószámok -> DTO címhez tartozó irányítószámok
    /// </summary>
    public class AddressZipCodeToAddressZipCode
    {
        public CompanyGroup.Dto.PartnerModule.AddressZipCodes Map(List<CompanyGroup.Domain.PartnerModule.AddressZipCode> from)
        {
            return new CompanyGroup.Dto.PartnerModule.AddressZipCodes() { Items = from.ConvertAll(x => x.ZipCode) };
        }
    }
}
