using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// regisztráció adatokat összefogó osztály
    /// </summary>
    public class RegistrationData //: CompanyGroup.Dto.RegistrationModule.Registration
    {
        public RegistrationData(CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts, 
                            CompanyGroup.Dto.RegistrationModule.CompanyData companyData, 
                            CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons, 
                            CompanyGroup.Dto.RegistrationModule.DataRecording dataRecording,
                            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses,
                            CompanyGroup.Dto.RegistrationModule.InvoiceAddress invoiceAddress,
                            CompanyGroup.Dto.RegistrationModule.MailAddress mailAddress,
                            string registrationId,
                            Visitor visitor, 
                            CompanyGroup.Dto.RegistrationModule.WebAdministrator webAdministrator, 
                            Countries countries)
        {
            this.BankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(bankAccounts, String.Empty);

            this.CompanyData = new CompanyGroup.WebClient.Models.CompanyData(companyData);

            this.ContactPersons = new CompanyGroup.WebClient.Models.ContactPersons(contactPersons, String.Empty);

            this.DataRecording = new CompanyGroup.WebClient.Models.DataRecording(dataRecording);

            this.DeliveryAddresses = new CompanyGroup.WebClient.Models.DeliveryAddresses(deliveryAddresses, String.Empty);

            this.InvoiceAddress = new CompanyGroup.WebClient.Models.InvoiceAddress(invoiceAddress);

            this.MailAddress = new CompanyGroup.WebClient.Models.MailAddress(mailAddress);

            this.RegistrationId = registrationId;

            this.WebAdministrator = new CompanyGroup.WebClient.Models.WebAdministrator(webAdministrator);

            this.Countries = countries;

            this.Visitor = visitor; 
        }

        public CompanyGroup.WebClient.Models.BankAccounts BankAccounts { get; set; }

        public CompanyGroup.WebClient.Models.CompanyData CompanyData { get; set; }

        public CompanyGroup.WebClient.Models.ContactPersons ContactPersons { get; set; }

        public CompanyGroup.WebClient.Models.DataRecording DataRecording { get; set; }

        public CompanyGroup.WebClient.Models.DeliveryAddresses DeliveryAddresses { get; set; }

        public CompanyGroup.WebClient.Models.InvoiceAddress InvoiceAddress { get; set; }

        public CompanyGroup.WebClient.Models.MailAddress MailAddress { get; set; }

        public string RegistrationId { get; set; }

        /// <summary>
        /// bejelentkezett látogató adatai
        /// </summary>
        public Visitor Visitor { get; set; }

        public CompanyGroup.WebClient.Models.WebAdministrator WebAdministrator { get; set; }

        public Countries Countries { get; set; }

    }
}
