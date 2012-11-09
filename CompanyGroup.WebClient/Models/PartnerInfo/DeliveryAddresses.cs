using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class DeliveryAddresses
    {
        public List<DeliveryAddress> Items { get; set; }

        public DeliveryAddresses(CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new DeliveryAddress(x));

            this.SelectedId = String.Empty;
        }

        /// <summary>
        /// módosításra kiválasztott szállítási cím azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
