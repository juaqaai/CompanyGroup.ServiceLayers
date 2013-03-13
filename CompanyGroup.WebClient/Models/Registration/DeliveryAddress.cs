using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class DeliveryAddress
    {
        public DeliveryAddress() : this(new CompanyGroup.Dto.RegistrationModule.DeliveryAddress(), String.Empty) {}

        public DeliveryAddress(CompanyGroup.Dto.RegistrationModule.DeliveryAddress deliveryAddress, string selectedId)
        {
            this.City = deliveryAddress.City;

            this.CountryRegionId = deliveryAddress.CountryRegionId;

            this.Id = deliveryAddress.Id;

            this.RecId = deliveryAddress.RecId;

            this.Street = deliveryAddress.Street;

            this.ZipCode = deliveryAddress.ZipCode;

            this.SelectedItem = (selectedId.Equals(deliveryAddress.Id));
        }

        public DeliveryAddress(CompanyGroup.Dto.PartnerModule.DeliveryAddress deliveryAddress)
        {
            this.City = deliveryAddress.City;

            this.CountryRegionId = deliveryAddress.CountryRegionId;

            this.Id = deliveryAddress.Id.ToString();

            this.RecId = deliveryAddress.RecId;

            this.Street = deliveryAddress.Street;

            this.ZipCode = deliveryAddress.ZipCode;

            this.SelectedItem = false;
        }

        public long RecId { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string CountryRegionId { get; set; }

        public string Id { set; get; }

        public bool SelectedItem { get; set; }
    }


}