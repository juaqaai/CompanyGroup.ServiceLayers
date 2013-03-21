using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class VisitorApiController : ApiBaseController
    {
        /// <summary>
        /// látogató adatainak kiolvasása
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetVisitorInfo")]
        public CompanyGroup.WebClient.Models.Visitor GetVisitorInfo()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            if (visitorData == null)
            {
                return new CompanyGroup.WebClient.Models.Visitor();
            }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return visitor;
        }

        /// <summary>
        /// beállítja a http süti Currency mező értékét.
        /// /// </summary>
        /// <returns></returns>
        [ActionName("ChangeCurrency")]
        [HttpPost]
        public HttpResponseMessage ChangeCurrency(CompanyGroup.WebClient.Models.ChangeCurrencyRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Currency = String.IsNullOrEmpty(request.Currency) ? ApiBaseController.DefaultCurrency : request.Currency;

                //ha nincs bejelentkezve, akkor nincs szervizhívás sem
                if (String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                    
                CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest req = new CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest(visitorData.VisitorId, visitorData.Currency);

                CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "ChangeCurrency", req);

                //változások mentése a sütibe
                this.WriteCookie(visitorData);

                CompanyGroup.WebClient.Models.Visitor visitor = new Models.Visitor(response);

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.Visitor>(HttpStatusCode.OK, visitor);

                return httpResponseMessage;
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /// <summary>
        /// beállítja a http süti Language mező értékét.
        /// Ha EN-re kattint, akkor a request.Language értéke EN, 
        /// ha HU-ra kettint, akkor a request.Language értéke HU
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ChangeLanguage")]
        public HttpResponseMessage ChangeLanguage(CompanyGroup.WebClient.Models.ChangeLanguageRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Language = String.IsNullOrEmpty(request.Language) ? ApiBaseController.LanguageHungarian : request.Language;

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                //ha nincs bejelentkezve, akkor nincs szervizhívás sem
                if (!String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest req = new CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest(visitorData.VisitorId, visitorData.Language);

                    response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest>("Visitor", "ChangeLanguage", req);
                }
                //else
                //{
                //    visitor = new CompanyGroup.WebClient.Models.Visitor() { LanguageId = request.Language, InverseLanguageId = request.Language.ToUpper().Equals(ApiBaseController.LanguageHungarian) ? ApiBaseController.LanguageEnglish : ApiBaseController.LanguageHungarian };
                //}

                //változások mentése a sütibe
                this.WriteCookie(visitorData);

                return response;
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
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

                CompanyGroup.Dto.PartnerModule.SignInRequest req = new CompanyGroup.Dto.PartnerModule.SignInRequest(ApiBaseController.DataAreaId,
                                                                                                        request.UserName,
                                                                                                        request.Password,
                                                                                                        System.Web.HttpContext.Current.Request.UserHostAddress);

                HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.SignInRequest>("Visitor", "SignIn", req);

                CompanyGroup.Dto.PartnerModule.Visitor visitor = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.PartnerModule.Visitor>().Result : new CompanyGroup.Dto.PartnerModule.Visitor();

                CompanyGroup.WebClient.Models.Visitor viewModel = new CompanyGroup.WebClient.Models.Visitor(visitor);

                HttpStatusCode httpStatusCode = HttpStatusCode.OK;

                //check status
                if (!viewModel.LoggedIn)
                {
                    viewModel.ErrorMessage = "A bejelentkezés nem sikerült!";
                }
                else    //SignIn process, set http cookie, etc...
                {
                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.Id), "A bejelentkezés nem sikerült! (üres azonosító)");

                    CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(visitor.CompanyId), "A bejelentkezés nem sikerült! (üres cégazonosító)");

                    //kosár társítása
                    CompanyGroup.Dto.WebshopModule.AssociateCartRequest associateRequest = new CompanyGroup.Dto.WebshopModule.AssociateCartRequest(visitor.Id, permanentObjectId, visitorData.Language, visitorData.Currency);

                    CompanyGroup.Dto.WebshopModule.ShoppingCartInfo associateCart = this.PostJSonData<CompanyGroup.Dto.WebshopModule.AssociateCartRequest, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AssociateCart", associateRequest);

                    //visitor adatok http sütibe írása     
                    this.WriteCookie(new CompanyGroup.WebClient.Models.VisitorData(viewModel.Id, viewModel.LanguageId, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, viewModel.Currency, viewModel.Id, associateCart.ActiveCart.Id, visitorData.RegistrationId));

                    viewModel.ErrorMessage = String.Empty;

                    httpStatusCode = HttpStatusCode.Created;
                }

                return Request.CreateResponse<CompanyGroup.WebClient.Models.Visitor>(httpStatusCode, viewModel);
            }
            catch(CompanyGroup.Helpers.DesignByContractException ex)
            {
                return ThrowHttpError(ex);
            }
            catch(Exception ex)
            {
                return ThrowHttpError(ex);
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

                CompanyGroup.Dto.PartnerModule.SignOutRequest req = new CompanyGroup.Dto.PartnerModule.SignOutRequest(ApiBaseController.DataAreaId, visitorData.VisitorId);

                HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.SignOutRequest>("Visitor", "SignOut", req);

                visitorData.VisitorId = String.Empty;

                this.WriteCookie(visitorData);

                CompanyGroup.WebClient.Models.Visitor visitor = new CompanyGroup.WebClient.Models.Visitor();

                return Request.CreateResponse<CompanyGroup.WebClient.Models.Visitor>(HttpStatusCode.OK, visitor);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
