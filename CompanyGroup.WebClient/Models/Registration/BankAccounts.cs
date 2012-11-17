using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class BankAccounts
    {

        public BankAccounts(CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts)
        {
            this.Items = bankAccounts.Items.ConvertAll(x => new CompanyGroup.WebClient.Models.BankAccount(x));

            selectedId = String.Empty;
        }

        private string selectedId = String.Empty;

        /// <summary>
        /// módosításra kiválasztott bankszámla azonosító
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

        public List<CompanyGroup.WebClient.Models.BankAccount> Items { get; set; }
    }
}
