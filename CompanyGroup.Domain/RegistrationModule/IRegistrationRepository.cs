using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    public interface IRegistrationRepository
    {
        /// <summary>
        /// regisztráció kiolvasás
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyGroup.Domain.RegistrationModule.Registration GetByKey(string id);

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        void Add(CompanyGroup.Domain.RegistrationModule.Registration registration);

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        void Remove(string id);

        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        void Post(string id);

        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataRecording"></param>
        void UpdateDataRecording(string id, CompanyGroup.Domain.RegistrationModule.DataRecording dataRecording);

        /// <summary>
        /// regisztrációs adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyData"></param>
        /// <param name="invoiceAddress"></param>
        /// <param name="mailAddress"></param>
        void UpdateRegistrationData(string id, CompanyGroup.Domain.RegistrationModule.CompanyData companyData,
                                               CompanyGroup.Domain.RegistrationModule.InvoiceAddress invoiceAddress,
                                               CompanyGroup.Domain.RegistrationModule.MailAddress mailAddress);

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        void UpdateWebAdministrator(string id, CompanyGroup.Domain.RegistrationModule.WebAdministrator webAdministrator);

        /// <summary>
        /// szállítáci cím hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddress"></param>
        void AddDeliveryAddress(string id, CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress);

        /// <summary>
        /// szállítáci cím módosítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="deliveryAddress"></param>
        void UpdateDeliveryAddress(string registrationId, CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress);

        /// <summary>
        /// szállítáci cím törlése
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddressId"></param>
        void RemoveDeliveryAddress(string id, string deliveryAddressId);

        /// <summary>
        /// bankszámlaszám hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bankAccount"></param>
        void AddBankAccount(string id, CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount);

        /// <summary>
        /// bankszámlaszám módosítás
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="bankAccount"></param>
        void UpdateBankAccount(string registrationId, CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount);

        /// <summary>
        /// bankszámlaszám törlés
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="bankAccountId"></param>
        void RemoveBankAccount(string registrationId, string bankAccountId);

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactPerson"></param>
        void AddContactPerson(string id, CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson);

        /// <summary>
        /// kapcsolattartó adatok módosítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="contactPersonId"></param>
        /// <param name="personId"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="email"></param>
        /// <param name="cellularPhone"></param>
        /// <param name="phone"></param>
        /// <param name="allowOrder"></param>
        /// <param name="allowReceiptOfGoods"></param>
        /// <param name="smsArriveOfGoods"></param>
        /// <param name="smsOrderConfirm"></param>
        /// <param name="smsOfDelivery"></param>
        /// <param name="emailArriveOfGoods"></param>
        /// <param name="emailOfOrderConfirm"></param>
        /// <param name="emailOfDelivery"></param>
        /// <param name="webAdmin"></param>
        /// <param name="priceListDownload"></param>
        /// <param name="invoiceInfo"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="leftCompany"></param>
        /// <param name="newsletter"></param>
        /// <param name="recId"></param>
        /// <param name="refRecId"></param>
        void UpdateContactPerson(string registrationId, CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson);

        /// <summary>
        /// kapcsolattartó eltávolítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="contactPersonId"></param>
        void RemoveContactPerson(string registrationId, string contactPersonId);

    }
}
