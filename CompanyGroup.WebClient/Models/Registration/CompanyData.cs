using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class CompanyData : CompanyGroup.Dto.RegistrationModule.CompanyData
    {
        public CompanyData(CompanyGroup.Dto.RegistrationModule.CompanyData companyData)
        {
            this.CountryRegionId = companyData.CountryRegionId;
            this.CustomerId = companyData.CustomerId;
            this.CustomerName = companyData.CustomerName;
            this.EUVatNumber = companyData.EUVatNumber;
            this.MainEmail = companyData.MainEmail;
            this.NewsletterToMainEmail = companyData.NewsletterToMainEmail;
            this.RegistrationNumber = companyData.RegistrationNumber;
            this.SignatureEntityFile = companyData.SignatureEntityFile;
            this.VatNumber = companyData.VatNumber;
        }

        public CompanyData() { }
    }
}
