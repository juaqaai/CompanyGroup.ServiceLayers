using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class DeliveryAddressToDeliveryAddress
    {
        /// <summary>
        /// Domain vevő szállítási cím adatok -> DTO. szállítási cím adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddress MapDomainToDto(CompanyGroup.Domain.RegistrationModule.DeliveryAddress from)
        {
            return new CompanyGroup.Dto.RegistrationModule.DeliveryAddress() { Id = from.Id.ToString(), City = from.City, CountryRegionId = from.CountryRegionId, Street = from.Street, ZipCode = from.ZipCode, RecId = from.RecId };
        }

        /// <summary>
        /// Dto. vevő szállítási cím adatok -> Domain szállítási cím adatok  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.DeliveryAddress MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.DeliveryAddress from)
        {
            return new CompanyGroup.Domain.RegistrationModule.DeliveryAddress() { Id = MongoDB.Bson.ObjectId.Parse(from.Id), City = from.City, CountryRegionId = from.CountryRegionId, Street = from.Street, ZipCode = from.ZipCode, RecId = from.RecId };
        }
    }
}
