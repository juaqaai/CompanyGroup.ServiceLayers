using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class DeliveryAddressToDeliveryAddress
    {
        /// <summary>
        /// Domain vevő szállítási cím adatok -> DTO. szállítási cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddress MapDomainToRegistrationModuleDto(CompanyGroup.Domain.PartnerModule.DeliveryAddress from)
        {
            return new CompanyGroup.Dto.RegistrationModule.DeliveryAddress() { City = from.City, CountryRegionId = from.CountryRegionId, Street = from.Street, ZipCode = from.ZipCode, RecId = from.RecId, Id = String.Empty };
        }

        public CompanyGroup.Dto.PartnerModule.DeliveryAddress MapDomainToDto(CompanyGroup.Domain.PartnerModule.DeliveryAddress from)
        {
            return new CompanyGroup.Dto.PartnerModule.DeliveryAddress() { City = from.City, CountryRegionId = from.CountryRegionId, Street = from.Street, ZipCode = from.ZipCode, RecId = from.RecId };
        }
    }
}
