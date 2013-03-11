using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// szállítási címek 
    /// </summary>
    public class DeliveryAddresses
    {
        public DeliveryAddresses(CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses, string selectedId)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.DeliveryAddress(x, selectedId));
        }

        public DeliveryAddresses(CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.DeliveryAddress(x));
        }

        public List<CompanyGroup.WebClient.Models.DeliveryAddress> Items { get; set; }
    }
}
