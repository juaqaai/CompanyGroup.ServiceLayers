using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class CustomerApiController : ApiBaseController
    {
        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetAddressZipCodes")]
        [HttpGet]
        public CompanyGroup.WebClient.Models.AddressZipCodes GetAddressZipCodes(string prefix)
        {
            CompanyGroup.Dto.ServiceRequest.AddressZipCode req = new Dto.ServiceRequest.AddressZipCode() { DataAreaId = CustomerApiController.DataAreaId, Prefix = prefix };

            CompanyGroup.Dto.PartnerModule.AddressZipCodes response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddressZipCode, CompanyGroup.Dto.PartnerModule.AddressZipCodes>("Customer", "GetAddressZipCodes", req);

            return new CompanyGroup.WebClient.Models.AddressZipCodes(response);
        }

        ///// <summary>
        ///// vevő regisztrációs adatok kiolvasása vevőazonosító és vállalatkód alapján
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[ActionName("GetCustomerRegistration")]
        //[HttpPost]
        //public CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request)
        //{
        //    return service.GetCustomerRegistration(request);
        //}

        /// <summary>
        /// szállítási címek   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")]
        [HttpGet]
        public CompanyGroup.WebClient.Models.DeliveryAddresses GetDeliveryAddresses()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses request = new Dto.ServiceRequest.GetDeliveryAddresses() { DataAreaId = CustomerApiController.DataAreaId, VisitorId = visitorData.ObjectId };

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", request);

            return new CompanyGroup.WebClient.Models.DeliveryAddresses(response);
        }

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SignIn")]
        public HttpResponseMessage SignIn(CompanyGroup.WebClient.Models.SignInRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "SignIn request can not be null!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Password), "A jelszó megadása kötelező!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.UserName), "A belépési név megadása kötelező!");

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                //előző belépés azonosítójának mentése
                string permanentObjectId = visitorData.PermanentId;

                CompanyGroup.Dto.ServiceRequest.SignIn req = new CompanyGroup.Dto.ServiceRequest.SignIn(ApiBaseController.DataAreaId,
                                                                                                        request.UserName,
                                                                                                        request.Password,
                                                                                                        System.Web.HttpContext.Current.Request.UserHostAddress);

                CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignIn, CompanyGroup.Dto.PartnerModule.Visitor>("Customer", "SignIn", req);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor(response);

                HttpStatusCode httpStatusCode = HttpStatusCode.OK;

                //check status
                if (!visitor.LoggedIn)
                {
                    visitor.ErrorMessage = "A bejelentkezés nem sikerült!";
                }
                else    //SignIn process, set http cookie, etc...
                {
                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.Id), "A bejelentkezés nem sikerült! (üres azonosító)");

                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.CompanyId), "A bejelentkezés nem sikerült! (üres cégazonosító)");

                    //kosár társítása
                    CompanyGroup.Dto.ServiceRequest.AssociateCart associateRequest = new CompanyGroup.Dto.ServiceRequest.AssociateCart(visitor.Id, permanentObjectId) { Language = visitorData.Language };

                    CompanyGroup.Dto.WebshopModule.ShoppingCartInfo associateCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AssociateCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AssociateCart", associateRequest);

                    //visitor adatok http sütibe írása     
                    this.WriteCookie(new CompanyGroup.WebClient.Models.VisitorData(visitor.Id, visitor.LanguageId, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, visitor.Currency, visitor.Id, associateCart.ActiveCart.Id, visitorData.RegistrationId));

                    visitor.ErrorMessage = String.Empty;

                    httpStatusCode = HttpStatusCode.Created;
                }

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.Visitor>(httpStatusCode, visitor);

                return httpResponseMessage;
            }
            catch (CompanyGroup.Helpers.DesignByContractException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                //ex.Message
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SignOut")]
        public HttpResponseMessage SignOut()
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                CompanyGroup.Dto.ServiceRequest.SignOut req = new CompanyGroup.Dto.ServiceRequest.SignOut() { DataAreaId = ApiBaseController.DataAreaId, ObjectId = visitorData.ObjectId };

                CompanyGroup.Dto.ServiceResponse.Empty empty = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SignOut, CompanyGroup.Dto.ServiceResponse.Empty>("Customer", "SignOut", req);

                visitorData.ObjectId = String.Empty;

                this.WriteCookie(visitorData);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor();

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.Visitor>(HttpStatusCode.OK, visitor);

                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        /// <summary>
        /// beállítja a http süti Currency mező értékét.
        /// /// </summary>
        /// <returns></returns>
        [ActionName("ChangeCurrency")]
        [HttpPut]
        public void ChangeCurrency(string currency)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Currency = String.IsNullOrEmpty(currency) ? ApiBaseController.DefaultCurrency : currency;

                this.WriteCookie(visitorData);
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /// <summary>
        /// beállítja a http süti Language mező értékét.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [ActionName("ChangeLanguage")]
        [HttpPut]
        public void ChangeLanguage(string language)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Language = String.IsNullOrEmpty(language) ? ApiBaseController.LanguageHungarian : language;

                this.WriteCookie(visitorData);
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

    }
}
