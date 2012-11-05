using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ContactPersons : CompanyGroup.Dto.RegistrationModule.ContactPersons
    {
        public ContactPersons() : base()
        {
            SelectedId = String.Empty;
        }

        /// <summary>
        /// módosításra kiválasztott kapcsolattartó azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
