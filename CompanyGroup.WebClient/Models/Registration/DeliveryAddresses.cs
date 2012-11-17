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

            selectedId = String.Empty;
        }

        public DeliveryAddresses(CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses)
        {
            this.Items = deliveryAddresses.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.DeliveryAddress(x));

            selectedId = String.Empty;
        }

        public List<CompanyGroup.WebClient.Models.DeliveryAddress> Items { get; set; }

        private string selectedId = String.Empty;

        /// <summary>
        /// módosításra kiválasztott szállítási cím azonosító
        /// </summary>
        public string SelectedId
        {
            get { return selectedId; }
            set
            {
                selectedId = value;

                if (!String.IsNullOrEmpty(value))
                {
                    this.Items.ForEach(x => x.SelectedItem = x.Id.Equals(value));
                }
            }
        }
    }
}
