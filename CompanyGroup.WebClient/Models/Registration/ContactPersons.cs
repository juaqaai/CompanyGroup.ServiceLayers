using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// kapcsolattartók
    /// </summary>
    public class ContactPersons 
    {
        public ContactPersons(CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons, string selectedId)
        {
            this.Items = contactPersons.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.ContactPerson(x, selectedId));
        }

        public List<CompanyGroup.WebClient.Models.ContactPerson> Items { get; set; }

        ///// <summary>
        ///// módosításra kiválasztott kapcsolattartó azonosító
        ///// </summary>
        //private string selectedId = String.Empty;

        ///// <summary>
        ///// módosításra kiválasztott kapcsolattartó azonosító
        ///// </summary>
        //public string SelectedId
        //{
        //    get { return selectedId; }
        //    set
        //    {
        //        selectedId = value;

        //        if (!String.IsNullOrEmpty(value))
        //        {
        //            this.Items.ForEach(x => x.SelectedItem = x.Id.Equals(value));
        //        }
        //    }
        //}
    }
}
