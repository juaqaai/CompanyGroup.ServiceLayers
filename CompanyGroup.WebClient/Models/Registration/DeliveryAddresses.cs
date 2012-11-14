using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// szállítási címek 
    /// </summary>
    public class DeliveryAddresses
    {
        public DeliveryAddresses(CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.DeliveryAddress(x));

            this.SelectedId = String.Empty;
        }

        public DeliveryAddresses(CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.DeliveryAddress(x));

            this.SelectedId = String.Empty;
        }

        public List<CompanyGroup.WebClient.Models.DeliveryAddress> Items { get; set; }

        /// <summary>
        /// módosításra kiválasztott szállítási cím azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
