using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// regisztrációs adatok módosítása
    /// </summary>
    public class UpdateRegistrationDataRequest
    {
        public UpdateRegistrationDataRequest(CompanyData companyData, InvoiceAddress invoiceAddress, MailAddress mailAddress, bool modifyMailAddress, bool modifyDeliveryAddress, DeliveryAddress deliveryAddress, BankAccount bankAccount)
        {
            this.CompanyData = companyData;

            this.InvoiceAddress = invoiceAddress;

            this.MailAddress = mailAddress;

            this.ModifyMailAddress = modifyMailAddress;

            this.ModifyDeliveryAddress = modifyDeliveryAddress;

            this.DeliveryAddress = deliveryAddress;

            this.BankAccount = bankAccount;
        }

        public CompanyGroup.WebClient.Models.CompanyData CompanyData { get; set; }

        public CompanyGroup.WebClient.Models.InvoiceAddress InvoiceAddress { get; set; }

        public CompanyGroup.WebClient.Models.MailAddress MailAddress { get; set; }

        /// <summary>
        /// beállította-e a nem egyezik a levelezési cím kapcsolót
        /// </summary>
        public bool ModifyMailAddress { get; set; }

        /// <summary>
        /// beállította-e a nem egyezik a szállítási cím kapcsolót
        /// </summary>
        public bool ModifyDeliveryAddress { get; set; }

        /// <summary>
        /// szállítási cím
        /// </summary>
        public CompanyGroup.WebClient.Models.DeliveryAddress DeliveryAddress { get; set; }

        /// <summary>
        /// bankszámlaszám
        /// </summary>
        public CompanyGroup.WebClient.Models.BankAccount BankAccount { get; set; }
    }
}