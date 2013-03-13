using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    public class DeliveryAddress
    {
        public DeliveryAddress() : this(0, "", "", "", "", "") { }

        public DeliveryAddress(long recId, string city, string street, string zipCode, string countryRegionId, string id)
        {
            this.RecId = recId;
            this.City = city;
            this.Street = street;
            this.ZipCode = zipCode;
            this.CountryRegionId = countryRegionId;
            this.Id = id;
        }

        public long RecId { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string CountryRegionId { get; set; }

        public string Id { set; get; }
    }

    public class DeliveryAddresses
    {
        public List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> Items { get; set; }

        public DeliveryAddresses()
        {
            this.Items = new List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress>();
        }

        public DeliveryAddresses(List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> items)
        {
            this.Items = items;
        }
    }
}
