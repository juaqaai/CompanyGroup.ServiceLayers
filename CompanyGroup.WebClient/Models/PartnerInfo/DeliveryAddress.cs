using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class DeliveryAddress : CompanyGroup.Dto.PartnerModule.DeliveryAddress
    {
        public DeliveryAddress(CompanyGroup.Dto.PartnerModule.DeliveryAddress deliveryAddress)
        {
            this.City = deliveryAddress.City;

            this.CountryRegionId = deliveryAddress.CountryRegionId;

            this.Id = deliveryAddress.Id;

            this.RecId = deliveryAddress.RecId;

            this.Street = deliveryAddress.Street;

            this.ZipCode = deliveryAddress.ZipCode;
        }
    }

    public class DeliveryAddresses
    {
        public List<DeliveryAddress> Items { get; set; }

        public DeliveryAddresses(CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new DeliveryAddress(x));
        }
    }
}