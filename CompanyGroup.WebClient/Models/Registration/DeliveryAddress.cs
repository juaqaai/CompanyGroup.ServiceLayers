using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class DeliveryAddress : CompanyGroup.Dto.RegistrationModule.DeliveryAddress
    {
        public DeliveryAddress(CompanyGroup.Dto.RegistrationModule.DeliveryAddress deliveryAddress)
        {
            this.City = deliveryAddress.City;

            this.CountryRegionId = deliveryAddress.CountryRegionId;

            this.Id = deliveryAddress.Id;

            this.RecId = deliveryAddress.RecId;

            this.Street = deliveryAddress.Street;

            this.ZipCode = deliveryAddress.ZipCode;
        }
    }


}