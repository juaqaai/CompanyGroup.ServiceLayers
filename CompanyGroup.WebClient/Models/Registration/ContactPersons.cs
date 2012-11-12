using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// kapcsolattartók
    /// </summary>
    public class ContactPersons 
    {
        public ContactPersons(CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons)
        {
            SelectedId = String.Empty;

            this.Items = contactPersons.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.ContactPerson(x));
        }

        public List<CompanyGroup.WebClient.Models.ContactPerson> Items { get; set; }

        /// <summary>
        /// módosításra kiválasztott kapcsolattartó azonosító
        /// </summary>
        public string SelectedId { get; set; }
    }
}
