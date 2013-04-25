using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    public class Factory
    {
        /// <summary>
        /// adatrögzítő személyes adatai
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static DataRecording CreateDataRecording(string email, string name, string phone)
        {
            return new DataRecording(email, name, phone);
        }

        /// <summary>
        /// cégadatok
        /// </summary>
        /// <param name="countryRegionId"></param>
        /// <param name="customerId"></param>
        /// <param name="customerName"></param>
        /// <param name="euVatNumber"></param>
        /// <param name="mainEmail"></param>
        /// <param name="newsletterToMainEmail"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="vatNumber"></param>
        /// <param name="signatureEntityFile"></param>
        /// <returns></returns>
        public static CompanyData CreateCompanyData(string countryRegionId, string customerId, string customerName, string euVatNumber, string mainEmail, bool newsletterToMainEmail, string registrationNumber, string vatNumber, string signatureEntityFile)
        {
            return new CompanyData(countryRegionId, customerId, customerName, euVatNumber, mainEmail, newsletterToMainEmail, registrationNumber, signatureEntityFile, vatNumber);
        }

        /// <summary>
        /// számlázási cím adatok
        /// </summary>
        /// <param name="city"></param>
        /// <param name="countryRegionId"></param>
        /// <param name="zipCode"></param>
        /// <param name="street"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static InvoiceAddress CreateInvoiceAddress(string city, string countryRegionId, string zipCode, string street, string phone)
        {
            return new InvoiceAddress(city, countryRegionId, zipCode, street, phone);
        }

        /// <summary>
        /// levelezési cím adatok
        /// </summary>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="zipCode"></param>
        /// <param name="street"></param>
        /// <returns></returns>
        public static MailAddress CreateMailAddress(string city, string country, string zipCode, string street)
        {
            return new MailAddress(city, country, zipCode, street);
        }

        /// <summary>
        /// kapcsolattartó adatok
        /// </summary>
        /// <param name="contactPersonId"></param>
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
        /// <returns></returns>
        public static ContactPerson CreateContactPerson(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone,
                                           bool allowOrder, bool allowReceiptOfGoods,
                                           bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery,
                                           bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery,
                                           bool webAdmin, bool priceListDownload, bool invoiceInfo,
                                           string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId, List<string> positions)
        { 
            return new ContactPerson(contactPersonId, lastName, firstName, email, cellularPhone, phone,
                                     allowOrder, allowReceiptOfGoods,
                                     smsArriveOfGoods, smsOrderConfirm, smsOfDelivery,
                                     emailArriveOfGoods, emailOfOrderConfirm, emailOfDelivery,
                                     webAdmin, priceListDownload, invoiceInfo,
                                     userName, password, leftCompany, newsletter, recId, refRecId, positions);
        }

        /// <summary>
        /// web adminisztrátor létrehozása
        /// </summary>
        /// <param name="contactPersonId"></param>
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
        /// <param name="priceListDownload"></param>
        /// <param name="invoiceInfo"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="leftCompany"></param>
        /// <param name="newsletter"></param>
        /// <param name="recId"></param>
        /// <param name="refRecId"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static WebAdministrator CreateWebAdministrator(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone,
                                           bool allowOrder, bool allowReceiptOfGoods,
                                           bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery,
                                           bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery,
                                           bool priceListDownload, bool invoiceInfo,
                                           string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId, List<string> positions)
        {
            return new WebAdministrator(contactPersonId, lastName, firstName, email, cellularPhone, phone,
                                        allowOrder, allowReceiptOfGoods,
                                        smsArriveOfGoods, smsOrderConfirm, smsOfDelivery,
                                        emailArriveOfGoods, emailOfOrderConfirm, emailOfDelivery,
                                        priceListDownload, invoiceInfo,
                                        userName, password, leftCompany, newsletter, recId, refRecId, positions);
        }

        /// <summary>
        /// bankszámla létrehozása
        /// </summary>
        /// <param name="number"></param>
        /// <param name="recId"></param>
        /// <returns></returns>
        public static BankAccount CreateBankAccount(string number, long recId)
        {
            return new BankAccount(number, recId);
        }

        /// <summary>
        /// bankszámla létrehozása
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <param name="part3"></param>
        /// <param name="recId"></param>
        /// <returns></returns>
        public static BankAccount CreateBankAccount(string part1, string part2, string part3, Int64 recId)
        { 
            return new BankAccount(part1, part2, part3, recId);
        }

        /// <summary>
        /// szállítási cím létrehozása
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="zipCode"></param>
        /// <param name="street"></param>
        /// <returns></returns>
        public static DeliveryAddress CreateDeliveryAddress(long recId, string city, string country, string zipCode, string street)
        {
            return new DeliveryAddress(recId, city, country, zipCode, street);
        }

        /// <summary>
        /// regisztráció létrehozása
        /// </summary>
        /// <returns></returns>
        public static Registration CreateRegistration()
        {
            return new Registration()
            {
                Id = MongoDB.Bson.ObjectId.Empty,
                CompanyId = "",
                PersonId = "",
                VisitorId = "",
                DataRecording = CreateDataRecording("", "", ""),
                DeliveryAddressList = new List<DeliveryAddress>(),
                ContactPersonList = new List<ContactPerson>(),
                InvoiceAddress = CreateInvoiceAddress("", "", "", "", ""),
                MailAddress = CreateMailAddress("", "", "", ""),
                WebAdministrator = CreateWebAdministrator("", "", "", "", "", "", false, false, false, false, false, false, false, false, false, false,"", "", false,false, 0, 0, new List<string>()),   
                BankAccountList = new List<BankAccount>(),
                CompanyData = CreateCompanyData("", "", "", "", "", false, "", "", ""),
                Status = RegistrationStatus.Created
            }; 
        }

        /// <summary>
        /// regisztráció létrehozása
        /// </summary>
        /// <param name="mainEmail"></param>
        /// <param name="newsletterToMainEmail"></param>
        /// <param name="companyId"></param>
        /// <param name="personId"></param>
        /// <param name="visitorId"></param>
        /// <param name="dataRecording"></param>
        /// <param name="companyData"></param>
        /// <param name="invoiceAddress"></param>
        /// <param name="mailAddress"></param>
        /// <param name="webAdministrator"></param>
        /// <param name="bankAccountList"></param>
        /// <param name="contactPersonList"></param>
        /// <param name="deliveryAddressList"></param>
        /// <returns></returns>
        public static Registration CreateRegistration(string companyId, 
                                                      string personId, 
                                                      string visitorId,
                                                      DataRecording dataRecording, 
                                                      CompanyData companyData, 
                                                      InvoiceAddress invoiceAddress, 
                                                      MailAddress mailAddress,
                                                      WebAdministrator webAdministrator, 
                                                      List<BankAccount> bankAccountList,
                                                      List<ContactPerson> contactPersonList,
                                                      List<DeliveryAddress> deliveryAddressList)
        {
            return new Registration()
                       {
                           Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                           CompanyId = companyId,
                           PersonId = personId,
                           VisitorId = visitorId,
                           DataRecording = dataRecording,
                           DeliveryAddressList = deliveryAddressList,
                           ContactPersonList = contactPersonList,
                           InvoiceAddress = invoiceAddress,
                           MailAddress = mailAddress,
                           WebAdministrator = webAdministrator,
                           BankAccountList = bankAccountList,
                           CompanyData = companyData,
                           Status = RegistrationStatus.Created
                       };
        }

    }
}
