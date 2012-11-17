using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class RegistrationToRegistration
    {
        /// <summary>
        /// Domain registration -> Dto registration
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.Registration MapDomainToDto(CompanyGroup.Domain.RegistrationModule.Registration from)
        {
            return new CompanyGroup.Dto.RegistrationModule.Registration()
                       {
                           BankAccounts = from.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x)),
                           ContactPersons = from.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x)),
                           CompanyData = new CompanyDataToCompanyData().MapDomainToDto(from.CompanyData),
                           DataRecording = new DataRecordingToDataRecording().MapDomainToDto(from.DataRecording),
                           DeliveryAddresses = from.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x)),
                           InvoiceAddress = new InvoiceAddressToInvoiceAddress().MapDomainToDto(from.InvoiceAddress),
                           MailAddress = new MailAddressToMailAddress().MapDomainToDto(from.MailAddress),
                           WebAdministrator = new WebAdministratorToWebAdministrator().MapDomainToDto(from.WebAdministrator), 
                           RegistrationId = from.Id.ToString()
                       };
        }

        public CompanyGroup.Domain.RegistrationModule.Registration MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.Registration from)
        {
            return new CompanyGroup.Domain.RegistrationModule.Registration()
            {
                BankAccountList = from.BankAccounts.ConvertAll(x => new BankAccountToBankAccount().MapDtoToDomain(x)),
                ContactPersonList = from.ContactPersons.ConvertAll(x => new ContactPersonToContactPerson().MapDtoToDomain(x)),
                CompanyData = new CompanyDataToCompanyData().MapDtoToDomain(from.CompanyData),
                DataRecording = new DataRecordingToDataRecording().MapDtoToDomain(from.DataRecording),
                DeliveryAddressList = from.DeliveryAddresses.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDtoToDomain(x)),
                InvoiceAddress = new InvoiceAddressToInvoiceAddress().MapDtoToDomain(from.InvoiceAddress),
                MailAddress = new MailAddressToMailAddress().MapDtoToDomain(from.MailAddress),
                WebAdministrator = new WebAdministratorToWebAdministrator().MapDtoToDomain(from.WebAdministrator), 
                VisitorId = from.Visitor.Id
                //Id = 
                
            };
        }
    }
}
