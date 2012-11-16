using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class RegistrationApiController : ApiBaseController
    {
        /// <summary>
        /// regisztrációs adatok kiolvasása (betöltéskor, frissítéskor hívódik)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRegistrationData")]
        public CompanyGroup.WebClient.Models.RegistrationData GetRegistrationData()
        {
            CompanyGroup.Dto.RegistrationModule.Registration response = null;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            //CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

            //ha nem volt regisztrációs azonosítója, akkor adatok olvasása az ERP-ből     
            if (String.IsNullOrEmpty(visitorData.RegistrationId) && !String.IsNullOrEmpty(visitorData.ObjectId))
            {
                response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>("Customer", "GetCustomerRegistration", String.Format("{0}/{1}", visitorData.ObjectId, RegistrationApiController.DataAreaId));
            }
            else if (!String.IsNullOrEmpty(visitorData.RegistrationId))     //volt már regisztrációs azonosítója, ezért az ahhoz tartozó adatokat kell visszaolvasni a cacheDb-ből
            {
                response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "GetByKey", visitorData.RegistrationId);
            }
            else
            {
                response = new CompanyGroup.Dto.RegistrationModule.Registration();
            }

            CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts = new CompanyGroup.Dto.RegistrationModule.BankAccounts(response.BankAccounts);

            CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons = new CompanyGroup.Dto.RegistrationModule.ContactPersons(response.ContactPersons);

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses(response.DeliveryAddresses);

            CompanyGroup.WebClient.Models.RegistrationData model = new CompanyGroup.WebClient.Models.RegistrationData(bankAccounts,
                                                                                                                      response.CompanyData,
                                                                                                                      contactPersons,
                                                                                                                      response.DataRecording,
                                                                                                                      deliveryAddresses,
                                                                                                                      response.InvoiceAddress,
                                                                                                                      response.MailAddress,
                                                                                                                      response.RegistrationId,
                                                                                                                      response.VisitorId,
                                                                                                                      response.WebAdministrator);

            return model;        
        }

        /// <summary>
        /// új regisztráció hozzáadás (akkor hívódik, ha elfogadásra kerültek a szerződési feltételek)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddNew")]
        public HttpResponseMessage AddNew()
        {
            CompanyGroup.Dto.RegistrationModule.Registration response = null;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            //ha volt már regisztrációs azonosító, akkor a regisztráció kiolvasása történik a cacheDb-ből     
            if (!String.IsNullOrEmpty(visitorData.RegistrationId))
            {
                response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "GetByKey", visitorData.RegistrationId);
            }

            //ha nem volt korábban regisztráció, vagy volt, de nem érvényes a státusz flag, akkr új regisztráció hozzáadása történik
            if ((response == null) || (response.RegistrationId.Equals(RegistrationApiController.MongoDbEmptyObjectId)))
            {
                CompanyGroup.Dto.ServiceRequest.AddNewRegistration addNewRegistration = new CompanyGroup.Dto.ServiceRequest.AddNewRegistration()
                {
                    VisitorId = visitorData.ObjectId,
                    LanguageId = visitorData.Language
                };

                response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddNewRegistration, CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "AddNew", addNewRegistration);
            }

            //ha a válaszban nincs bankszámlaszám, akkor egy üres elemet hozzá kell adni
            //if (response.BankAccounts.Count.Equals(0))
            //{ 
            //    response.BankAccounts.Add(new CompanyGroup.Dto.RegistrationModule.BankAccount(){ Id = "", Part1 = "", Part2 = "", Part3 = "", RecId = 0 });
            //}

            //ha a válaszban nincs kapcsolattartó, akkor egy üres elemet hozzá kell adni
            //if (response.ContactPersons.Count.Equals(0))
            //{
            //    response.ContactPersons.Add(new CompanyGroup.Dto.RegistrationModule.ContactPerson() 
            //                                    { 
            //                                        AllowOrder = false, 
            //                                        AllowReceiptOfGoods = false, 
            //                                        ContactPersonId = "", 
            //                                        Email = "", 
            //                                        EmailArriveOfGoods = false, 
            //                                        EmailOfDelivery = false, 
            //                                        EmailOfOrderConfirm = false, 
            //                                        FirstName = "", 
            //                                        Id = "", 
            //                                        InvoiceInfo = false, 
            //                                        LastName = "", 
            //                                        LeftCompany = false, 
            //                                        Newsletter = false, 
            //                                        Password = "", 
            //                                        PriceListDownload = false, 
            //                                        SmsArriveOfGoods = false, 
            //                                        SmsOfDelivery = false, 
            //                                        SmsOrderConfirm = false, 
            //                                        Telephone = "", 
            //                                        UserName = "", 
            //                                        WebAdmin = false 
            //                                    });
            //}

            //ha a válaszban nincs szállítási cím, akkor egy üres elemet hozzá kell adni
            //if (response.DeliveryAddresses.Count.Equals(0))
            //{
            //    response.DeliveryAddresses.Add(new CompanyGroup.Dto.RegistrationModule.DeliveryAddress()
            //    {
            //        City = "",
            //        CountryRegionId = "",
            //        Id = "",
            //        RecId = 0,
            //        Street = "",
            //        ZipCode = ""
            //    });
            //}

            //létrehozott regisztrációs azonosító beírása sütibe
            visitorData.RegistrationId = response.RegistrationId;

            this.WriteCookie(visitorData);

            CompanyGroup.Dto.RegistrationModule.BankAccounts bankAccounts = new CompanyGroup.Dto.RegistrationModule.BankAccounts(response.BankAccounts);

            CompanyGroup.Dto.RegistrationModule.ContactPersons contactPersons = new CompanyGroup.Dto.RegistrationModule.ContactPersons(response.ContactPersons);

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses deliveryAddresses = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses(response.DeliveryAddresses);

            CompanyGroup.WebClient.Models.RegistrationData model = new CompanyGroup.WebClient.Models.RegistrationData(bankAccounts,
                                                                                                                      response.CompanyData,
                                                                                                                      contactPersons,
                                                                                                                      response.DataRecording,
                                                                                                                      deliveryAddresses,
                                                                                                                      response.InvoiceAddress,
                                                                                                                      response.MailAddress,
                                                                                                                      response.RegistrationId,
                                                                                                                      response.VisitorId,
                                                                                                                      response.WebAdministrator);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.RegistrationData>(HttpStatusCode.Created, model);

            string uri = Url.Route(null, new { id = model.RegistrationId });

            httpResponseMsg.Headers.Location = new Uri(Request.RequestUri, uri);

            return httpResponseMsg;
        }

        /// <summary>
        /// regisztráció elküldése az ERP -nek
        /// </summary>
        /// <returns>Successed, Message</returns>
        [HttpPost]
        [ActionName("Post")]
        public HttpResponseMessage Post()
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.PostRegistration request = new CompanyGroup.Dto.ServiceRequest.PostRegistration()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language
            };

            CompanyGroup.Dto.ServiceResponse.PostRegistration response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.PostRegistration, CompanyGroup.Dto.ServiceResponse.PostRegistration>("Registration", "Post", request);

            //ha a feladás sikeres, akkor a felhasználói sütiből a regisztrációs azonosító törlésre kerül
            if (response.Successed)
            {
                visitorData.RegistrationId = String.Empty;

                this.WriteCookie(visitorData);
            }

            CompanyGroup.WebClient.Models.PostRegistration model = new CompanyGroup.WebClient.Models.PostRegistration(response);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.PostRegistration>(HttpStatusCode.Created, model);

            string uri = Url.Route(null, new { id = visitorData.RegistrationId });

            httpResponseMsg.Headers.Location = new Uri(Request.RequestUri, uri);

            return httpResponseMsg;
        }

        /// <summary>
        /// adatrögzítő adatainak felvitele
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successed, Message</returns>
        [HttpPost]
        [ActionName("UpdateDataRecording")]
        public HttpResponseMessage UpdateDataRecording(CompanyGroup.WebClient.Models.DataRecording request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateDataRecording req = new CompanyGroup.Dto.ServiceRequest.UpdateDataRecording()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DataRecording = request
            };

            CompanyGroup.Dto.ServiceResponse.UpdateDataRecording response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateDataRecording, CompanyGroup.Dto.ServiceResponse.UpdateDataRecording>("Registration", "UpdateDataRecording", req);

            CompanyGroup.WebClient.Models.UpdateDataRecording model = new CompanyGroup.WebClient.Models.UpdateDataRecording(response);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.UpdateDataRecording>(HttpStatusCode.OK, model);

            string uri = Url.Route(null, new { id = visitorData.RegistrationId });

            httpResponseMsg.Headers.Location = new Uri(Request.RequestUri, uri);

            return httpResponseMsg;
        }

        /// <summary>
        /// céges regisztrációs adatok, számlák, levelezési címek felvitele
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successed, Message</returns>
        [HttpPost]
        [ActionName("UpdateRegistrationData")]
        public HttpResponseMessage UpdateRegistrationData(CompanyGroup.WebClient.Models.UpdateRegistrationDataRequest request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData req = new CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                CompanyData = request.CompanyData,
                InvoiceAddress = request.InvoiceAddress,
                MailAddress = request.MailAddress
            };

            CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData, CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData>("Registration", "UpdateRegistrationData", req);

            CompanyGroup.WebClient.Models.UpdateRegistrationData model = new CompanyGroup.WebClient.Models.UpdateRegistrationData(response);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.UpdateRegistrationData>(HttpStatusCode.OK, model);

            string uri = Url.Route(null, new { id = visitorData.RegistrationId });

            httpResponseMsg.Headers.Location = new Uri(Request.RequestUri, uri);

            return httpResponseMsg;
        }

        /// <summary>
        /// webadminisztrátor adatok felvitele
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Successed, Message</returns>
        [HttpPost]
        [ActionName("UpdateWebAdministrator")]
        public HttpResponseMessage UpdateWebAdministrator(CompanyGroup.WebClient.Models.WebAdministrator request)
        {
            request.ContactPersonId = request.ContactPersonId ?? String.Empty;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator req = new CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                WebAdministrator = request
            };

            CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator, CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator>("Registration", "UpdateWebAdministrator", req);

            CompanyGroup.WebClient.Models.WebAdministratorResponse model = new CompanyGroup.WebClient.Models.WebAdministratorResponse(response);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.WebAdministratorResponse>(HttpStatusCode.OK, model);

            string uri = Url.Route(null, new { id = visitorData.RegistrationId });

            httpResponseMsg.Headers.Location = new Uri(Request.RequestUri, uri);

            return httpResponseMsg;
        }

        #region "DeliveryAddress"

        /// <summary>
        /// kiszállítási cím felvitele
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// RecId, City, Street, ZipCode, CountryRegionId, Id
        /// </returns>
        [HttpPost]
        [ActionName("AddDeliveryAddress")]
        public CompanyGroup.WebClient.Models.DeliveryAddresses AddDeliveryAddress(CompanyGroup.WebClient.Models.DeliveryAddress request)
        {
            request.Id = String.Empty;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DeliveryAddress = request
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "AddDeliveryAddress", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses deliveryAddresses = new CompanyGroup.WebClient.Models.DeliveryAddresses(response) { SelectedId = request.Id };

            return deliveryAddresses;
        }

        [HttpPost]
        [ActionName("SelectForUpdateDeliveryAddress")]
        public CompanyGroup.WebClient.Models.DeliveryAddresses SelectForUpdateDeliveryAddress(CompanyGroup.WebClient.Models.SelectForUpdateDeliveryAddress request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "GetDeliveryAddresses", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses deliveryAddresses = new CompanyGroup.WebClient.Models.DeliveryAddresses(response) { SelectedId = request.SelectedId };

            return deliveryAddresses;
        }

        /// <summary>
        /// kiszállítási cím módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// RecId, City, Street, ZipCode, CountryRegionId, Id
        /// </returns>
        [HttpPost]
        [ActionName("UpdateDeliveryAddress")]
        public CompanyGroup.WebClient.Models.DeliveryAddresses UpdateDeliveryAddress(CompanyGroup.WebClient.Models.DeliveryAddress request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DeliveryAddress = request
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "UpdateDeliveryAddress", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses deliveryAddresses = new CompanyGroup.WebClient.Models.DeliveryAddresses(response) { SelectedId = request.Id };

            return deliveryAddresses;
        }

        /// <summary>
        /// kiszállítási cím törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// RecId, City, Street, ZipCode, CountryRegionId, Id
        /// </returns>
        [HttpPost]
        [ActionName("RemoveDeliveryAddress")]
        public CompanyGroup.WebClient.Models.DeliveryAddresses RemoveDeliveryAddress(CompanyGroup.WebClient.Models.RemoveDeliveryAddress request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DeliveryAddressId = request.Id
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "RemoveDeliveryAddress", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses deliveryAddresses = new CompanyGroup.WebClient.Models.DeliveryAddresses(response) { SelectedId = String.Empty };

            return deliveryAddresses;
        }

        #endregion

        #region "BankAccounts"

        /// <summary>
        /// bankszámlaszám hozzáadása
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Part1 Part2 Part3 RecId Id</returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.BankAccounts AddBankAccount(CompanyGroup.WebClient.Models.BankAccount request)
        {
            request.Id = String.Empty;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddBankAccount req = new CompanyGroup.Dto.ServiceRequest.AddBankAccount()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                BankAccount = request
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddBankAccount, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "AddBankAccount", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response) { SelectedId = String.Empty };

            return bankAccounts;
        }

        [HttpPost]
        public CompanyGroup.WebClient.Models.BankAccounts SelectForUpdateBankAccount(CompanyGroup.WebClient.Models.SelectForUpdateBankAccount request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetBankAccounts req = new CompanyGroup.Dto.ServiceRequest.GetBankAccounts()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetBankAccounts, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "GetBankAccounts", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response) { SelectedId = request.SelectedId };

            return bankAccounts;
        }

        /// <summary>
        /// bankszámlaszám módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Part1 Part2 Part3 RecId Id</returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.BankAccounts UpdateBankAccount(CompanyGroup.WebClient.Models.BankAccount request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateBankAccount req = new CompanyGroup.Dto.ServiceRequest.UpdateBankAccount()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                BankAccount = request
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateBankAccount, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "UpdateBankAccount", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response) { SelectedId = String.Empty };

            return bankAccounts;
        }

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request">Part1 Part2 Part3 RecId Id</param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.BankAccounts RemoveBankAccount(CompanyGroup.WebClient.Models.RemoveBankAccount request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveBankAccount req = new CompanyGroup.Dto.ServiceRequest.RemoveBankAccount()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                BankAccountId = request.Id
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveBankAccount, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "RemoveBankAccount", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response) { SelectedId = String.Empty };

            return bankAccounts;
        }

        #endregion

        #region "ContactPersons"

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// <summary>
        /// kapcsolattartó egyedi azonosító
        /// </summary>
        ///ContactPersonId,FirstName,LastName,AllowOrder ,AllowReceiptOfGoods ,SmsArriveOfGoods,SmsOrderConfirm,SmsOfDelivery ,EmailArriveOfGoods ,EmailOfOrderConfirm ,EmailOfDelivery ,WebAdmin ,PriceListDownload 
        ///InvoiceInfo,UserName,Password ,Newsletter,Telephone,Email ,LeftCompany,Id
        /// </returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ContactPersons AddContactPerson(CompanyGroup.WebClient.Models.ContactPerson request)
        {
            request.ContactPersonId = request.ContactPersonId ?? String.Empty;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddContactPerson req = new CompanyGroup.Dto.ServiceRequest.AddContactPerson()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                ContactPerson = request
            };

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddContactPerson, CompanyGroup.Dto.RegistrationModule.ContactPersons>("Registration", "AddContactPerson", req);

            CompanyGroup.WebClient.Models.ContactPersons contactPersons = new CompanyGroup.WebClient.Models.ContactPersons(response) { SelectedId = String.Empty };

            return contactPersons;
        }

        /// <summary>
        /// kapcsolattartók kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ContactPersons SelectForUpdateContactPerson(CompanyGroup.WebClient.Models.SelectForUpdateContactPerson request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetContactPerson req = new CompanyGroup.Dto.ServiceRequest.GetContactPerson()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language
            };

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetContactPerson, CompanyGroup.Dto.RegistrationModule.ContactPersons>("Registration", "GetContactPersons", req);

            CompanyGroup.WebClient.Models.ContactPersons contactPersons = new CompanyGroup.WebClient.Models.ContactPersons(response) { SelectedId = request.SelectedId };

            return contactPersons;
        }

        /// <summary>
        /// kapcsolattartó módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ContactPersons UpdateContactPerson(CompanyGroup.WebClient.Models.ContactPersonRequest request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateContactPerson req = new CompanyGroup.Dto.ServiceRequest.UpdateContactPerson()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                ContactPerson = request
            };

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateContactPerson, CompanyGroup.Dto.RegistrationModule.ContactPersons>("Registration", "UpdateContactPerson", req);

            CompanyGroup.WebClient.Models.ContactPersons contactPersons = new CompanyGroup.WebClient.Models.ContactPersons(response) { SelectedId = String.Empty };

            return contactPersons;
        }

        /// <summary>
        /// kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ContactPersons RemoveContactPerson(CompanyGroup.WebClient.Models.RemoveContactPerson request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveContactPerson req = new CompanyGroup.Dto.ServiceRequest.RemoveContactPerson()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                Id = request.Id
            };

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveContactPerson, CompanyGroup.Dto.RegistrationModule.ContactPersons>("Registration", "RemoveContactPerson", req);

            CompanyGroup.WebClient.Models.ContactPersons contactPersons = new CompanyGroup.WebClient.Models.ContactPersons(response) { SelectedId = String.Empty };

            return contactPersons;
        }

        #endregion

       
    }
}
