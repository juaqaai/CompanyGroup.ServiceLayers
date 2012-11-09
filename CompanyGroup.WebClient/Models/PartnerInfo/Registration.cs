﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// regisztráció adatokat összefogó osztály
    /// </summary>
    public class Registration //: CompanyGroup.Dto.RegistrationModule.Registration
    {
        public Registration(List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccounts, 
                            CompanyGroup.Dto.RegistrationModule.CompanyData companyData, 
                            List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersons, 
                            CompanyGroup.Dto.RegistrationModule.DataRecording dataRecording,
                            List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddresses,
                            CompanyGroup.Dto.RegistrationModule.InvoiceAddress invoiceAddress,
                            CompanyGroup.Dto.RegistrationModule.MailAddress mailAddress,
                            string registrationId, string visitorId, 
                            CompanyGroup.Dto.RegistrationModule.WebAdministrator webAdministrator)
        {
            this.BankAccounts = new CompanyGroup.WebClient.Models.BankAccounts() { Items = bankAccounts, SelectedId = "" };

            this.CompanyData = new CompanyData(companyData);

            this.ContactPersons = new ContactPersons() { Items = contactPersons, SelectedId = "" };

            this.DataRecording = new DataRecording(dataRecording);

            //this.DeliveryAddresses = new DeliveryAddresses(new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses() { Items = deliveryAddresses });

            this.InvoiceAddress = new InvoiceAddress(invoiceAddress);

            this.MailAddress = new MailAddress(mailAddress);

            this.RegistrationId = registrationId;

            this.VisitorId = visitorId;

            this.WebAdministrator = new WebAdministrator(webAdministrator);
        }

        public CompanyGroup.WebClient.Models.BankAccounts BankAccounts { get; set; }

        public CompanyData CompanyData { get; set; }

        public CompanyGroup.WebClient.Models.ContactPersons ContactPersons { get; set; }

        public DataRecording DataRecording { get; set; }

        public CompanyGroup.WebClient.Models.DeliveryAddresses DeliveryAddresses { get; set; }

        public InvoiceAddress InvoiceAddress { get; set; }

        public MailAddress MailAddress { get; set; }

        public string RegistrationId { get; set; }

        public string VisitorId { get; set; }

        public WebAdministrator WebAdministrator { get; set; }
    }
}