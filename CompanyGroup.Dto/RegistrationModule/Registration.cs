using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    /// <summary>
    /// összefogja a regisztrációs űrlapon módosítható értékeket 
    /// </summary>
    public class Registration
    {
        public Registration()
        {
            this.DataRecording = new CompanyGroup.Dto.RegistrationModule.DataRecording() { Email = "", Name = "", Phone = "" };

            this.InvoiceAddress = new CompanyGroup.Dto.RegistrationModule.InvoiceAddress() { City = "", CountryRegionId = "", Phone = "", Street = "", ZipCode = "" };

            this.MailAddress = new CompanyGroup.Dto.RegistrationModule.MailAddress() { City = "", CountryRegionId = "", Street = "", ZipCode = "" };

            this.CompanyData = new CompanyGroup.Dto.RegistrationModule.CompanyData() { CountryRegionId = "", CustomerId = "", RegistrationNumber = "", CustomerName = "", MainEmail = "", EUVatNumber = "", NewsletterToMainEmail = false, SignatureEntityFile = "", VatNumber = "" };

            this.BankAccounts = new List<CompanyGroup.Dto.RegistrationModule.BankAccount>();

            this.DeliveryAddresses = new List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress>();

            this.ContactPersons = new List<CompanyGroup.Dto.RegistrationModule.ContactPerson>();

            this.WebAdministrator = new CompanyGroup.Dto.RegistrationModule.WebAdministrator() 
                                        { 
                                            AllowOrder = false, 
                                            AllowReceiptOfGoods = false, 
                                            ContactPersonId = "", 
                                            Email = "", 
                                            EmailArriveOfGoods = false, 
                                            EmailOfDelivery = false, 
                                            EmailOfOrderConfirm = false, 
                                            FirstName = "", 
                                            InvoiceInfo = false, 
                                            LastName = "", 
                                            LeftCompany = false, 
                                            Newsletter = false, 
                                            Password = "", 
                                            PriceListDownload = false, 
                                            RecId = 0, 
                                            RefRecId = 0, 
                                            SmsArriveOfGoods = false, 
                                            SmsOfDelivery = false, 
                                            SmsOrderConfirm = false, 
                                            Telephone = "", 
                                            UserName = "" 
                                        };

            this.Visitor = new CompanyGroup.Dto.PartnerModule.Visitor();

            this.RegistrationId = String.Empty;

            this.LoadData = false;
        }

        public CompanyGroup.Dto.RegistrationModule.DataRecording DataRecording { get; set; }

        public CompanyGroup.Dto.RegistrationModule.CompanyData CompanyData { get; set; }

        public CompanyGroup.Dto.RegistrationModule.InvoiceAddress InvoiceAddress { get; set; }

        public CompanyGroup.Dto.RegistrationModule.MailAddress MailAddress { get; set; }

        public List<CompanyGroup.Dto.RegistrationModule.BankAccount> BankAccounts { get; set; }

        public List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> DeliveryAddresses { get; set; }

        public List<CompanyGroup.Dto.RegistrationModule.ContactPerson> ContactPersons { get; set; }

        public CompanyGroup.Dto.RegistrationModule.WebAdministrator WebAdministrator { get; set; }

        public CompanyGroup.Dto.PartnerModule.Visitor Visitor { get; set; }

        public string RegistrationId { get; set; }

        public bool LoadData { get; set; }
    }
}
