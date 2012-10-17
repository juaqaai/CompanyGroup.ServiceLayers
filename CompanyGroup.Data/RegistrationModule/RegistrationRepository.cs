using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.RegistrationModule
{
    /// <summary>
    /// regisztrációs adatkezelés műveletei
    /// </summary>
    /// <remarks>
    /// csak a szerződési feltételeket követően kerül létrehozásra az új regisztráció, vagy a regisztrációs adatok módosítása objektum
    /// ha a felhasználó bejelentkezett státuszú, akkor az elfogadást követően az ERP rendszerben nyilvántartott adatokkal kitöltésre kerül a regisztrációs űrlap minden mezője
    /// kitöltés és ellenörzés után a regisztrációs adatokat a rendszer feldolgozza és válaszüzenetet küld.
    /// Ha a rögzítés sikeres, akkor a következő lépés a nyomtatási képernyő. A nyomtatott szerződésen feltüntetésre kerül az ERP rendszer által generált regisztrációs azonosító és a kitöltés során megadott adatok.
    /// </remarks>
    public class RegistrationRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.RegistrationModule.Registration>, CompanyGroup.Domain.RegistrationModule.IRegistrationRepository
    {

        public RegistrationRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("RegistrationCollectionName", "Registration");

        /// <summary>
        /// regisztráció kiolvasás
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.Registration GetByKey(string id)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The Id parameter cannot be null or empty!");

                MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                CompanyGroup.Domain.RegistrationModule.Registration registration = collection.FindOne(query);

                return registration;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        public void Add(CompanyGroup.Domain.RegistrationModule.Registration registration)
        {
            CompanyGroup.Domain.Utils.Check.Require(registration != null, "The registration parameter cannot be null!");
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                collection.Insert(registration);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The id parameter cannot be null!");
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));


                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Deleted);

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        public void Post(string id)
        {
            CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(id), "The id parameter cannot be null!");
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));


                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Posted);

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataRecording"></param>
        public void UpdateDataRecording(string id, CompanyGroup.Domain.RegistrationModule.DataRecording dataRecording)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("DataRecording.Email", MongoDB.Bson.BsonString.Create(dataRecording.Email))
                                                                    .Set("DataRecording.Name", MongoDB.Bson.BsonString.Create(dataRecording.Name))
                                                                    .Set("DataRecording.Phone", MongoDB.Bson.BsonString.Create(dataRecording.Phone));

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// regisztrációs adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyData"></param>
        /// <param name="invoiceAddress"></param>
        /// <param name="mailAddress"></param>
        public void UpdateRegistrationData(string id, CompanyGroup.Domain.RegistrationModule.CompanyData companyData, 
                                                      CompanyGroup.Domain.RegistrationModule.InvoiceAddress invoiceAddress, 
                                                      CompanyGroup.Domain.RegistrationModule.MailAddress mailAddress)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("CompanyData.CountryRegionId", MongoDB.Bson.BsonString.Create(companyData.CountryRegionId))
                                                                    .Set("CompanyData.CustomerId", MongoDB.Bson.BsonString.Create(companyData.CustomerId))
                                                                    .Set("CompanyData.CustomerName", MongoDB.Bson.BsonString.Create(companyData.CustomerName))
                                                                    .Set("CompanyData.EUVatNumber", MongoDB.Bson.BsonString.Create(companyData.EUVatNumber))
                                                                    .Set("CompanyData.MainEmail", MongoDB.Bson.BsonString.Create(companyData.MainEmail))
                                                                    .Set("CompanyData.NewsletterToMainEmail", MongoDB.Bson.BsonBoolean.Create(companyData.NewsletterToMainEmail))
                                                                    .Set("CompanyData.RegistrationNumber", MongoDB.Bson.BsonString.Create(companyData.RegistrationNumber))
                                                                    .Set("CompanyData.SignatureEntityFile", MongoDB.Bson.BsonString.Create(companyData.SignatureEntityFile))
                                                                    .Set("CompanyData.VatNumber", MongoDB.Bson.BsonString.Create(companyData.VatNumber))
                                                                    .Set("InvoiceAddress.Country", MongoDB.Bson.BsonString.Create(invoiceAddress.Country))
                                                                    .Set("InvoiceAddress.City", MongoDB.Bson.BsonString.Create(invoiceAddress.City))
                                                                    .Set("InvoiceAddress.Phone", MongoDB.Bson.BsonString.Create(invoiceAddress.Phone))
                                                                    .Set("InvoiceAddress.Street", MongoDB.Bson.BsonString.Create(invoiceAddress.Street))
                                                                    .Set("InvoiceAddress.ZipCode", MongoDB.Bson.BsonString.Create(invoiceAddress.ZipCode))
                                                                    .Set("MailAddress.Country", MongoDB.Bson.BsonString.Create(mailAddress.Country))
                                                                    .Set("MailAddress.City", MongoDB.Bson.BsonString.Create(mailAddress.City))
                                                                    .Set("MailAddress.Street", MongoDB.Bson.BsonString.Create(mailAddress.Street))
                                                                    .Set("MailAddress.ZipCode", MongoDB.Bson.BsonString.Create(mailAddress.ZipCode));

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        #region "Szállítási cím műveletek"

        /// <summary>
        /// szállítáci cím hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddress"></param>
        public void AddDeliveryAddress(string id, CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                collection.Update(query, MongoDB.Driver.Builders.Update.PushWrapped("DeliveryAddressList", deliveryAddress));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// szállítási cím módosítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="deliveryAddressId"></param>
        /// <param name="recId"></param>
        /// <param name="city"></param>
        /// <param name="countryRegionId"></param>
        /// <param name="zipCode"></param>
        /// <param name="street"></param>
        public void UpdateDeliveryAddress(string registrationId, CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(registrationId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))),
                                                                      MongoDB.Driver.Builders.Query.EQ("DeliveryAddressList._id", deliveryAddress.Id));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("DeliveryAddressList.$.City", MongoDB.Bson.BsonString.Create(deliveryAddress.City))
                                                                    .Set("DeliveryAddressList.$.CountryRegionId", MongoDB.Bson.BsonString.Create(deliveryAddress.CountryRegionId))
                                                                    .Set("DeliveryAddressList.$.ZipCode", MongoDB.Bson.BsonString.Create(deliveryAddress.ZipCode))
                                                                    .Set("DeliveryAddressList.$.Street", MongoDB.Bson.BsonString.Create(deliveryAddress.Street))
                                                                    .Set("DeliveryAddressList.$.RecId", MongoDB.Bson.BsonInt64.Create(deliveryAddress.RecId));
                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// szállítási cím törlése
        ///     _posts.Collection.Update(Query.EQ("_id", postId),
        /// Update.Pull("Comments", Query.EQ("_id", commentId))
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryAddressId"></param>
        public void RemoveDeliveryAddress(string id, string deliveryAddressId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Pull("DeliveryAddressList", MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(deliveryAddressId)));

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        #endregion

        #region "Bankszámlaszám műveletek"

        /// <summary>
        /// bankszámlaszám hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bankAccount"></param>
        public void AddBankAccount(string id, CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                collection.Update(query, MongoDB.Driver.Builders.Update.PushWrapped("BankAccountList", bankAccount));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// bankszámlaszám adatok módosítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="bankAccountId"></param>
        /// <param name="number"></param>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <param name="part3"></param>
        /// <param name="recId"></param>
        public void UpdateBankAccount(string registrationId, CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(registrationId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))),
                                                                      MongoDB.Driver.Builders.Query.EQ("BankAccountList._id", bankAccount.Id));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("BankAccountList.$.Number", MongoDB.Bson.BsonString.Create(bankAccount.Number))
                                                                    .Set("BankAccountList.$.Part1", MongoDB.Bson.BsonString.Create(bankAccount.Part1))
                                                                    .Set("BankAccountList.$.Part2", MongoDB.Bson.BsonString.Create(bankAccount.Part2))
                                                                    .Set("BankAccountList.$.Part3", MongoDB.Bson.BsonString.Create(bankAccount.Part3))
                                                                    .Set("BankAccountList.$.RecId", MongoDB.Bson.BsonInt64.Create(bankAccount.RecId));
                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }        
        }

        /// <summary>
        /// bankszámlaszám adatok törlése
        /// _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="bankAccountId"></param>
        public void RemoveBankAccount(string registrationId, string bankAccountId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(registrationId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Pull("BankAccountList", MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(bankAccountId)));

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        #endregion

        #region "Kapcsolattartó műveletek"

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contactPerson"></param>
        public void AddContactPerson(string id, CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                collection.Update(query, MongoDB.Driver.Builders.Update.PushWrapped("ContactPersonList", contactPerson));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

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
        public void UpdateContactPerson(string registrationId, CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(registrationId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))),
                                                                      MongoDB.Driver.Builders.Query.EQ("ContactPersonList._id", contactPerson.Id));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("ContactPersonList.$.ContactPersonId", MongoDB.Bson.BsonString.Create(contactPerson.ContactPersonId))
                                                                    .Set("ContactPersonList.$.LastName", MongoDB.Bson.BsonString.Create(contactPerson.LastName))
                                                                    .Set("ContactPersonList.$.FirstName", MongoDB.Bson.BsonString.Create(contactPerson.FirstName))
                                                                    .Set("ContactPersonList.$.Email", MongoDB.Bson.BsonString.Create(contactPerson.Email))
                                                                    .Set("ContactPersonList.$.Telephone", MongoDB.Bson.BsonString.Create(contactPerson.Telephone))
                                                                    .Set("ContactPersonList.$.AllowOrder", MongoDB.Bson.BsonBoolean.Create(contactPerson.AllowOrder))
                                                                    .Set("ContactPersonList.$.AllowReceiptOfGoods", MongoDB.Bson.BsonBoolean.Create(contactPerson.AllowReceiptOfGoods))
                                                                    .Set("ContactPersonList.$.SmsArriveOfGoods", MongoDB.Bson.BsonBoolean.Create(contactPerson.SmsArriveOfGoods))
                                                                    .Set("ContactPersonList.$.SmsOrderConfirm", MongoDB.Bson.BsonBoolean.Create(contactPerson.SmsOrderConfirm))
                                                                    .Set("ContactPersonList.$.SmsOfDelivery", MongoDB.Bson.BsonBoolean.Create(contactPerson.SmsOfDelivery))
                                                                    .Set("ContactPersonList.$.EmailArriveOfGoods", MongoDB.Bson.BsonBoolean.Create(contactPerson.EmailArriveOfGoods))
                                                                    .Set("ContactPersonList.$.EmailOfOrderConfirm", MongoDB.Bson.BsonBoolean.Create(contactPerson.EmailOfOrderConfirm))
                                                                    .Set("ContactPersonList.$.EmailOfDelivery", MongoDB.Bson.BsonBoolean.Create(contactPerson.EmailOfDelivery))
                                                                    .Set("ContactPersonList.$.WebAdmin", MongoDB.Bson.BsonBoolean.Create(contactPerson.WebAdmin))
                                                                    .Set("ContactPersonList.$.PriceListDownload", MongoDB.Bson.BsonBoolean.Create(contactPerson.PriceListDownload))
                                                                    .Set("ContactPersonList.$.InvoiceInfo", MongoDB.Bson.BsonBoolean.Create(contactPerson.InvoiceInfo))
                                                                    .Set("ContactPersonList.$.UserName", MongoDB.Bson.BsonString.Create(contactPerson.UserName))
                                                                    .Set("ContactPersonList.$.Password", MongoDB.Bson.BsonString.Create(contactPerson.Password))
                                                                    .Set("ContactPersonList.$.LeftCompany", MongoDB.Bson.BsonBoolean.Create(contactPerson.LeftCompany))
                                                                    .Set("ContactPersonList.$.Newsletter", MongoDB.Bson.BsonBoolean.Create(contactPerson.Newsletter))
                                                                    .Set("ContactPersonList.$.RecId", MongoDB.Bson.BsonInt64.Create(contactPerson.RecId))
                                                                    .Set("ContactPersonList.$.RefRecId", MongoDB.Bson.BsonInt64.Create(contactPerson.RefRecId));

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// kapcsolattartó eltávolítása
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="contactPersonId"></param>
        public void RemoveContactPerson(string registrationId, string contactPersonId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(registrationId)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Pull("ContactPersonList", MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(contactPersonId)));

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        #endregion

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        public void UpdateWebAdministrator(string id, CompanyGroup.Domain.RegistrationModule.WebAdministrator webAdministrator)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.RegistrationModule.Registration> collection = this.GetCollection(RegistrationRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", ConvertStringToBsonObjectId(id)),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CompanyGroup.Domain.RegistrationModule.RegistrationStatus.Created))));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("WebAdministrator.AllowOrder", MongoDB.Bson.BsonBoolean.Create(webAdministrator.AllowOrder))
                                                                    .Set("WebAdministrator.AllowReceiptOfGoods", MongoDB.Bson.BsonBoolean.Create(webAdministrator.AllowReceiptOfGoods))
                                                                    .Set("WebAdministrator.ContactPersonId", MongoDB.Bson.BsonString.Create(webAdministrator.ContactPersonId))
                                                                    .Set("WebAdministrator.Email", MongoDB.Bson.BsonString.Create(webAdministrator.Email))
                                                                    .Set("WebAdministrator.EmailArriveOfGoods", MongoDB.Bson.BsonBoolean.Create(webAdministrator.EmailArriveOfGoods))
                                                                    .Set("WebAdministrator.EmailOfDelivery", MongoDB.Bson.BsonBoolean.Create(webAdministrator.EmailOfDelivery))
                                                                    .Set("WebAdministrator.EmailOfOrderConfirm", MongoDB.Bson.BsonBoolean.Create(webAdministrator.EmailOfOrderConfirm))
                                                                    .Set("WebAdministrator.FirstName", MongoDB.Bson.BsonString.Create(webAdministrator.FirstName))
                                                                    .Set("WebAdministrator.InvoiceInfo", MongoDB.Bson.BsonBoolean.Create(webAdministrator.InvoiceInfo))
                                                                    .Set("WebAdministrator.LastName", MongoDB.Bson.BsonString.Create(webAdministrator.LastName))
                                                                    .Set("WebAdministrator.LeftCompany", MongoDB.Bson.BsonBoolean.Create(webAdministrator.LeftCompany))
                                                                    .Set("WebAdministrator.Newsletter", MongoDB.Bson.BsonBoolean.Create(webAdministrator.Newsletter))
                                                                    .Set("WebAdministrator.Password", MongoDB.Bson.BsonString.Create(webAdministrator.Password))
                                                                    .Set("WebAdministrator.PriceListDownload", MongoDB.Bson.BsonBoolean.Create(webAdministrator.PriceListDownload))
                                                                    .Set("WebAdministrator.RecId", MongoDB.Bson.BsonInt64.Create(webAdministrator.RecId))
                                                                    .Set("WebAdministrator.RefRecId", MongoDB.Bson.BsonInt64.Create(webAdministrator.RefRecId))
                                                                    .Set("WebAdministrator.SmsArriveOfGoods", MongoDB.Bson.BsonBoolean.Create(webAdministrator.SmsArriveOfGoods))
                                                                    .Set("WebAdministrator.SmsOfDelivery", MongoDB.Bson.BsonBoolean.Create(webAdministrator.SmsOfDelivery))
                                                                    .Set("WebAdministrator.SmsOrderConfirm", MongoDB.Bson.BsonBoolean.Create(webAdministrator.SmsOrderConfirm))
                                                                    .Set("WebAdministrator.Telephone", MongoDB.Bson.BsonString.Create(webAdministrator.Telephone))
                                                                    .Set("WebAdministrator.UserName", MongoDB.Bson.BsonString.Create(webAdministrator.UserName))
                                                                    .Set("WebAdministrator.WebAdmin", MongoDB.Bson.BsonBoolean.Create(webAdministrator.WebAdmin));

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

    }
}
