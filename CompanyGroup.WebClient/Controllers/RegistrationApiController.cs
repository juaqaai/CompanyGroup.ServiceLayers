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
        /// országlista
        /// </summary>
        /// <returns></returns>
        private CompanyGroup.WebClient.Models.Countries GetCountries()
        {
            CompanyGroup.WebClient.Models.Countries countries = new CompanyGroup.WebClient.Models.Countries();
            countries.Add(new Models.Country("HU", "Magyarország"));
            countries.Add(new Models.Country("AE", "Egyesült Arab Emirátus"));
            countries.Add(new Models.Country("AL", "Albánia"));
            countries.Add(new Models.Country("AT", "AUSZTRIA"));
            countries.Add(new Models.Country("AU", "AUSZTRÁLIA"));
            countries.Add(new Models.Country("AZ", "Azerbajdzsán"));
            countries.Add(new Models.Country("BE", "BELGIUM"));
            countries.Add(new Models.Country("BG", "BULGÁRIA"));
            countries.Add(new Models.Country("BR", "BRAZILIA"));
            countries.Add(new Models.Country("CA", "KANADA"));
            countries.Add(new Models.Country("CH", "SVÁJC"));
            countries.Add(new Models.Country("CN", "KINA"));
            countries.Add(new Models.Country("CY", "CIPRUS"));
            countries.Add(new Models.Country("CZ", "CSEH-KÖZTÁRSASÁG"));
            countries.Add(new Models.Country("DE", "NÉMETORSZÁG"));
            countries.Add(new Models.Country("DK", "DÁNIA"));
            countries.Add(new Models.Country("EE", "ÉSZTORSZÁG"));
            countries.Add(new Models.Country("ES", "SPANYOLORSZÁG"));
            countries.Add(new Models.Country("EU", "EURÓPAI UNIÓ"));
            countries.Add(new Models.Country("FI", "FINNORSZÁG"));
            countries.Add(new Models.Country("FR", "FRANCIAORSZÁG"));
            countries.Add(new Models.Country("GB", "NAGY-BRITANNIA"));
            countries.Add(new Models.Country("GR", "GÖRÖGORSZÁG"));
            countries.Add(new Models.Country("HK", "HONG KONG"));
            countries.Add(new Models.Country("HR", "HORVÁTORSZÁG"));
            countries.Add(new Models.Country("ID", "INDONÉZIA"));
            countries.Add(new Models.Country("IE", "IRORSZÁG"));
            countries.Add(new Models.Country("IL", "IZRAEL"));
            countries.Add(new Models.Country("IN", "INDIA"));
            countries.Add(new Models.Country("IS", "IZLAND"));
            countries.Add(new Models.Country("IT", "OLASZORSZÁG"));
            countries.Add(new Models.Country("JP", "JAPÁN"));
            countries.Add(new Models.Country("KR", "KOREAI KÖZTÁRSASÁG"));
            countries.Add(new Models.Country("LI", "LICHTENSTEIN"));
            countries.Add(new Models.Country("LT", "LITVÁNIA"));
            countries.Add(new Models.Country("LU", "LUXEMBURG"));
            countries.Add(new Models.Country("LV", "LETTORSZÁG"));
            countries.Add(new Models.Country("MD", "MOLDOVA"));
            countries.Add(new Models.Country("MN", "MONGOLIA"));
            countries.Add(new Models.Country("MT", "MÁLTA"));
            countries.Add(new Models.Country("MX", "MEXIKO"));
            countries.Add(new Models.Country("MY", "MALAYSIA"));
            countries.Add(new Models.Country("MZ", "MOZAMBIK"));
            countries.Add(new Models.Country("NL", "HOLLANDIA"));
            countries.Add(new Models.Country("NO", "NORVÉGIA"));
            countries.Add(new Models.Country("NZ", "Új-Zéland"));
            countries.Add(new Models.Country("PH", "FÜLÖPSZIGETEK"));
            countries.Add(new Models.Country("PL", "LENGYELORSZÁG"));
            countries.Add(new Models.Country("PT", "PORTUGÁLIA"));
            countries.Add(new Models.Country("RO", "ROMÁNIA"));
            countries.Add(new Models.Country("RS", "Szerbia"));
            countries.Add(new Models.Country("RU", "Oroszország"));
            countries.Add(new Models.Country("SE", "SVÉDORSZÁG"));
            countries.Add(new Models.Country("SG", "SZINGAPUR"));
            countries.Add(new Models.Country("SI", "SZLOVÉNIA"));
            countries.Add(new Models.Country("SK", "SZLOVÁK KÖZTÁRSASÁG"));
            countries.Add(new Models.Country("TH", "THAIFÖLD"));
            countries.Add(new Models.Country("TN", "TUNÉZIA"));
            countries.Add(new Models.Country("TR", "TÖRÖKORSZÁG"));
            countries.Add(new Models.Country("TW", "TAIWAN"));
            countries.Add(new Models.Country("UA", "UKRAJNA"));
            countries.Add(new Models.Country("US", "AMERIKAI EGYESÜLT ÁLLAMOK"));
            countries.Add(new Models.Country("VN", "VIETNAM"));
            countries.Add(new Models.Country("XS", "SZERBIA"));
            return countries;
        }

        /// <summary>
        /// regisztrációs adatok kiolvasása (betöltéskor, frissítéskor hívódik)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRegistrationData")]
        public CompanyGroup.WebClient.Models.RegistrationData GetRegistrationData()
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = null;

                //regisztrációs azonosító kiolvasása sütiből
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                //CompanyGroup.WebClient.Models.Visitor visitor = (visitorData == null) ? new CompanyGroup.WebClient.Models.Visitor() : this.GetVisitor(visitorData);

                //ha nem volt regisztrációs azonosítója, akkor adatok olvasása az ERP-ből     
                if (String.IsNullOrEmpty(visitorData.RegistrationId) && !String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>("Customer", "GetCustomerRegistration", String.Format("{0}/{1}", visitorData.VisitorId, RegistrationApiController.DataAreaId)); ;
                }
                //volt már regisztrációs azonosítója, ezért az ahhoz tartozó adatokat kell visszaolvasni a cacheDb-ből
                else if (!String.IsNullOrEmpty(visitorData.RegistrationId))     
                {
                    CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request = new CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey(visitorData.RegistrationId, visitorData.VisitorId);

                    response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey, CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "GetByKey", request);
                }
                //ha nincs belépve, és nincs megkezdett regisztrációja sem, akkor üreset kell visszaadni
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
                                                                                                                          response.Visitor,
                                                                                                                          response.WebAdministrator,
                                                                                                                          this.GetCountries());

                return model;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// új regisztráció hozzáadás (akkor hívódik, ha elfogadásra kerültek a szerződési feltételek)
        /// süti tartalma alapján, ha volt már regisztrációs azonosító, akkor a regisztráció kiolvasása történik a cache-ből.
        /// ha nem volt korábban regisztráció, vagy volt, de nem érvényes a státusz flag, akkr új regisztráció hozzáadása történik.
        /// regisztrációs azonosító beírása sütibe.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddNew")]
        public HttpResponseMessage AddNew()
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = null;

                //regisztrációs azonosító kiolvasása sütiből
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                //ha volt már regisztrációs azonosító, akkor a regisztráció kiolvasása történik a cacheDb-ből     
                //if (!String.IsNullOrEmpty(visitorData.RegistrationId))
                //{
                //    response = this.GetJSonData<CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "GetRegistrationByKey", String.Format("{0}/{1}", visitorData.RegistrationId, visitorData.VisitorId));

                //    //ha nem volt korábban regisztráció, vagy volt, de nem érvényes a státusz flag, akkr új regisztráció hozzáadása történik
                //    if ((response == null) || String.IsNullOrEmpty(response.RegistrationId))
                //    {

                //    }
                //}

                CompanyGroup.Dto.ServiceRequest.AddNewRegistration request = new CompanyGroup.Dto.ServiceRequest.AddNewRegistration(visitorData.VisitorId, visitorData.Language, visitorData.RegistrationId);

                response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddNewRegistration, CompanyGroup.Dto.RegistrationModule.Registration>("Registration", "AddNew", request);

                //létrehozott regisztrációs azonosító sütibe írása
                visitorData.RegistrationId = response.RegistrationId;

                this.WriteCookie(visitorData);

                //válaszüzenet előállítása
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
                                                                                                                          response.Visitor,
                                                                                                                          response.WebAdministrator, 
                                                                                                                          this.GetCountries());

                HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.RegistrationData>(HttpStatusCode.OK, model);

                return httpResponseMsg;
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
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
                WebAdministrator = new Dto.RegistrationModule.WebAdministrator(request.AllowOrder, request.AllowReceiptOfGoods, request.ContactPersonId, request.Email, request.EmailArriveOfGoods, 
                                                                               request.EmailOfDelivery, request.EmailOfOrderConfirm, request.FirstName, request.InvoiceInfo, request.LastName, 
                                                                               request.LeftCompany, request.Newsletter, request.Password, request.PriceListDownload, request.RecId, 
                                                                               request.RefRecId, request.SmsArriveOfGoods, request.SmsOfDelivery, request.SmsOrderConfirm, request.Telephone, 
                                                                               request.UserName)
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
        public HttpResponseMessage AddDeliveryAddress(CompanyGroup.WebClient.Models.DeliveryAddress request)
        {
            request.Id = String.Empty;

            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DeliveryAddress = new Dto.RegistrationModule.DeliveryAddress(request.RecId, request.City, request.Street, request.ZipCode, request.CountryRegionId, request.Id)
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "AddDeliveryAddress", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses model = new CompanyGroup.WebClient.Models.DeliveryAddresses(response, request.Id);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.DeliveryAddresses>(HttpStatusCode.OK, model);

            return httpResponseMsg;
        }

        [HttpPost]
        [ActionName("SelectForUpdateDeliveryAddress")]
        public HttpResponseMessage SelectForUpdateDeliveryAddress(CompanyGroup.WebClient.Models.SelectForUpdateDeliveryAddress request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "GetDeliveryAddresses", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses model = new CompanyGroup.WebClient.Models.DeliveryAddresses(response, request.SelectedId);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.DeliveryAddresses>(HttpStatusCode.OK, model);

            return httpResponseMsg;
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
        public HttpResponseMessage UpdateDeliveryAddress(CompanyGroup.WebClient.Models.DeliveryAddress request)
        {
            //regisztrációs azonosító kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress req = new CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress()
            {
                RegistrationId = visitorData.RegistrationId,
                LanguageId = visitorData.Language,
                DeliveryAddress = new Dto.RegistrationModule.DeliveryAddress(request.RecId, request.City, request.Street, request.ZipCode, request.CountryRegionId, request.Id)
            };

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress, CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>("Registration", "UpdateDeliveryAddress", req);

            CompanyGroup.WebClient.Models.DeliveryAddresses model = new CompanyGroup.WebClient.Models.DeliveryAddresses(response, request.Id);

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.DeliveryAddresses>(HttpStatusCode.OK, model);

            return httpResponseMsg;
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
        public HttpResponseMessage RemoveDeliveryAddress(CompanyGroup.WebClient.Models.RemoveDeliveryAddress request)
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

            CompanyGroup.WebClient.Models.DeliveryAddresses model = new CompanyGroup.WebClient.Models.DeliveryAddresses(response, "");

            HttpResponseMessage httpResponseMsg = Request.CreateResponse<CompanyGroup.WebClient.Models.DeliveryAddresses>(HttpStatusCode.OK, model);

            return httpResponseMsg;
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
                BankAccount = new Dto.RegistrationModule.BankAccount(request.Part1, request.Part2, request.Part3, request.RecId, request.Id)
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddBankAccount, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "AddBankAccount", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response, "");

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

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response, request.SelectedId);

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
                BankAccount = new Dto.RegistrationModule.BankAccount(request.Part1, request.Part2, request.Part3, request.RecId, request.Id)
            };

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateBankAccount, CompanyGroup.Dto.RegistrationModule.BankAccounts>("Registration", "UpdateBankAccount", req);

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response, "");

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

            CompanyGroup.WebClient.Models.BankAccounts bankAccounts = new CompanyGroup.WebClient.Models.BankAccounts(response, "");

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
