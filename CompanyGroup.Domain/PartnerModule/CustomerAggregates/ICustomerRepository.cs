using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő repository
    /// </summary>
    public interface ICustomerRepository
    {
        Customer GetCustomer(string customerId, string dataAreaId);

        /// <summary>
        /// vevő levelezési cím kiolvasása vevőazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        MailAddress GetMailAddress(string customerId, string dataAreaId);

        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<AddressZipCode> GetAddressZipCode(string prefix, string dataAreaId);

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        //CompanyGroup.Domain.PartnerModule.LoginInfo SignIn(string userName, string password, string dataAreaId);

        /// <summary>
        /// vevőhöz tartozó szállítási címek
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> GetDeliveryAddress(string customerId, string dataAreaId);

        /// <summary>
        /// vevőhöz tartozó bankszámlaszámok 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.BankAccount> GetBankAccounts(string customerId, string dataAreaId);

        /// <summary>
        /// kapcsolattartók lista
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.ContactPerson> GetContactPersons(string customerId, string dataAreaId);

        /// <summary>
        /// vevők árcsoportjainak lekérdezése 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> GetCustomerPriceGroups(string customerId);

        /// <summary>
        /// vevői bejelentkezés
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.Visitor SignIn(string userName, string password, string dataAreaId);

        /// <summary>
        /// vevő létrehozása (regisztráció)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Domain.RegistrationModule.CustomerCreateResult Create(CompanyGroup.Domain.RegistrationModule.CustomerCreate request);

        /// <summary>
        /// szállítási cím létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        long CreateDeliveryAddress(CompanyGroup.Domain.RegistrationModule.DeliveryAddressCreate request);

        /// <summary>
        /// kapcsolattartó létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        long CreateContactPerson(CompanyGroup.Domain.RegistrationModule.ContactPersonCreate request);

        /// <summary>
        /// regisztráció - bankszámlaszám létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        long CreateBankAccount(CompanyGroup.Domain.RegistrationModule.BankAccountCreate request);
    }
}
