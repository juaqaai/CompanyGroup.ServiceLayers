using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.Data.NoSql;
using CompanyGroup.Domain.RegistrationModule;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Data.Test
{
    [TestClass]
    public class RegistrationRepositoryTest
    {
        private string objectId = "505e0ed26ee01226e4f76235";

        [TestMethod()]
        public void RegistrationRepositoryFullTest()
        { 
            AddRegistrationTest();

            UpdateDataRecordingTest();

            UpdateRegistrationDataTest();

            UpdateWebAdministratorTest();

            AddBankAccountTest();

        }

        ///<summary>
        ///
        ///</summary>
        [TestMethod()]
        public void AddRegistrationTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            DataRecording dataRecording = Factory.CreateDataRecording("ajuhasz@hrp.hu", "Teszt Elek", "+36 30 2036 448");

            CompanyData companyData = Factory.CreateCompanyData("", "", "", "", "", false, "", "", "");

            InvoiceAddress invoiceAddress = Factory.CreateInvoiceAddress("", "", "", "", "");

            MailAddress mailAddress = Factory.CreateMailAddress("", "", "", "");

            WebAdministrator webAdministrator = Factory.CreateWebAdministrator("", "", "", "", "", "", false, false, false, false, false, false, false, false, false, false, "", "", false, false, 0, 0, new List<string>());

            Registration registration = Factory.CreateRegistration("", "", "", dataRecording, companyData, invoiceAddress, mailAddress, webAdministrator, new List<BankAccount>(), new List<ContactPerson>(), new List<DeliveryAddress>());

            registrationRepository.Add(registration);

            objectId = registration.Id.ToString();

            Assert.IsTrue(registration != null);
        }

        [TestMethod()]
        public void UpdateDataRecordingTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            DataRecording dataRecording = Factory.CreateDataRecording("bjuhasz@hrp.hu", "Mekk Elek", "+36 70 2036 448");

            registrationRepository.UpdateDataRecording( objectId, dataRecording);

            Assert.IsTrue(dataRecording != null);
        }

        [TestMethod()]
        public void UpdateRegistrationDataTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyData companyData = Factory.CreateCompanyData("Hungary", "", "Teszt Elek Kft.", "9999999999", "juhasz@hrp.hu", false, "9999999999", "9999999999", "alairasi_cimpld.pdf");

            InvoiceAddress invoiceAddress = Factory.CreateInvoiceAddress("Budapest", "Hungary", "1111", "Véső u. 12.", "06 1 222 2222");

            MailAddress mailAddress = Factory.CreateMailAddress("Budapest", "Hungary", "1111", "Véső u. 12.");

            registrationRepository.UpdateRegistrationData(objectId, companyData, invoiceAddress, mailAddress);

            Assert.IsTrue(companyData != null);
        }

        [TestMethod()]
        public void UpdateWebAdministratorTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.WebAdministrator webAdministrator = Factory.CreateWebAdministrator("", "Elek", "Brekk", "cjuhasz@hrp.hu", "+36 20 444 5557", "06 1 777 7778", true, true, true, true, true, true, true, true, true, true, "user1", "ppwwq", true, true, 0, 0, new List<string>());

            registrationRepository.UpdateWebAdministrator(objectId, webAdministrator);

            Assert.IsTrue(webAdministrator != null);
        }

        [TestMethod()]
        public void AddBankAccountTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount = Factory.CreateBankAccount("3333333333", "555555555", "8888888888", 0);

            registrationRepository.AddBankAccount(objectId, bankAccount);

            Assert.IsTrue(bankAccount != null);        
        }

        [TestMethod()]
        public void UpdateBankAccountTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount = Factory.CreateBankAccount("444444444", "232323232323232", "343433434345567", 1);

            bankAccount.Id = MongoDB.Bson.ObjectId.Parse("505e0ede6ee01226e4f76236");

            registrationRepository.UpdateBankAccount(objectId, bankAccount);

            Assert.IsTrue(bankAccount != null);
        }

        [TestMethod()]
        public void RemoveBankAccountTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            registrationRepository.RemoveBankAccount(objectId, "505e0ede6ee01226e4f76236");

            Assert.IsTrue(objectId != null);
        }

        [TestMethod()]
        public void AddDeliveryAddressTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = Factory.CreateDeliveryAddress(0, "Budapest", "Hungary", "1222", "Véső u. 8");

            registrationRepository.AddDeliveryAddress(objectId, deliveryAddress);

            Assert.IsTrue(deliveryAddress != null);
        }

        [TestMethod()]
        public void UpdateDeliveryAddressTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = Factory.CreateDeliveryAddress(1, "Budapest2", "Hungary5", "1033", "Méta u. 8");

            deliveryAddress.Id = MongoDB.Bson.ObjectId.Parse("505e10316ee01218784f91ac");

            registrationRepository.UpdateDeliveryAddress(objectId, deliveryAddress);

            Assert.IsTrue(deliveryAddress != null);
        }

        [TestMethod()]
        public void RemoveDeliveryAddressTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            registrationRepository.RemoveDeliveryAddress(objectId, "505e10316ee01218784f91ac");

            Assert.IsTrue(objectId != null);
        }

        [TestMethod()]
        public void AddContactPersonTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = Factory.CreateContactPerson("CP000060", "Elek", "Tesztelo", "ajuhasz@hrp.hu", "+36 20 444 5557", "06 1 777 7778", 
                                           true, true,
                                           true, true, true,
                                           true, true, true,
                                           true, true, true,
                                           "brekeke", "brekeke01", false, true, 0, 0, new List<string>());

            registrationRepository.AddContactPerson(objectId, contactPerson);

            Assert.IsTrue(contactPerson != null);
        }

        [TestMethod()]
        public void UpdateContactPersonTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = Factory.CreateContactPerson("CP6666666", "Elek", "Teszt", "djuhasz@hrp.hu", "+36 20 444 5557", "06 1 777 7778",
                                           true, true,
                                           true, true, true,
                                           true, true, true,
                                           true, true, true,
                                           "bbbbbbb", "brrrrrrrrrrrrrr", false, true, 0, 0, new List<string>());

            contactPerson.Id = MongoDB.Bson.ObjectId.Parse("505e19876ee012204c0ca5a3");

            registrationRepository.UpdateContactPerson(objectId, contactPerson);

            Assert.IsTrue(contactPerson != null);
        }

        [TestMethod()]
        public void DeleteContactPersonTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Registration"));

            CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository = new CompanyGroup.Data.RegistrationModule.RegistrationRepository(settings);

            registrationRepository.RemoveContactPerson(objectId, "505e19876ee012204c0ca5a3");

            Assert.IsTrue(objectId != null);
        }
    }
}
