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
        public UpdateRegistrationDataRequest(CompanyData companyData, InvoiceAddress invoiceAddress, MailAddress mailAddress)
        {
            this.CompanyData = companyData;

            this.InvoiceAddress = invoiceAddress;

            this.MailAddress = mailAddress;
        }

        public CompanyGroup.WebClient.Models.CompanyData CompanyData { get; set; }

        public CompanyGroup.WebClient.Models.InvoiceAddress InvoiceAddress { get; set; }

        public CompanyGroup.WebClient.Models.MailAddress MailAddress { get; set; }
    }
}