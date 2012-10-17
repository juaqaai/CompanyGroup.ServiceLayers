using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    /// <summary>
    /// összefogja a regisztrációs űrlapon módosítható értékeket 
    /// </summary>
    [System.Runtime.Serialization.DataContract(Name = "Registration", Namespace = "CompanyGroup.Dto.RegistrationModule")]
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

            this.VisitorId = String.Empty;

            this.RegistrationId = String.Empty;
        }

        [System.Runtime.Serialization.DataMember(Name = "DataRecording", Order = 1)]
        public CompanyGroup.Dto.RegistrationModule.DataRecording DataRecording { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "CompanyData", Order = 2)]
        public CompanyGroup.Dto.RegistrationModule.CompanyData CompanyData { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "InvoiceAddress", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.InvoiceAddress InvoiceAddress { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "MailAddress", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.MailAddress MailAddress { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "BankAccounts", Order = 4)]
        public List<CompanyGroup.Dto.RegistrationModule.BankAccount> BankAccounts { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DeliveryAddresses", Order = 5)]
        public List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> DeliveryAddresses { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "ContactPersons", Order = 6)]
        public List<CompanyGroup.Dto.RegistrationModule.ContactPerson> ContactPersons { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "WebAdministrator", Order = 7)]
        public CompanyGroup.Dto.RegistrationModule.WebAdministrator WebAdministrator { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 8)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 9)]
        public string RegistrationId { get; set; }
    }
}
