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
        public CompanyGroup.WebClient.Models.Visitor ChangeCurrency(CompanyGroup.WebClient.Models.ChangeCurrencyRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.Visitor visitor;

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Currency = String.IsNullOrEmpty(request.Currency) ? ApiBaseController.DefaultCurrency : request.Currency;

                //ha nincs bejelentkezve, akkor nincs szervizhívás sem
                if (!String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest req = new CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest(visitorData.VisitorId, visitorData.Currency);

                    CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "ChangeCurrency", req);

                    visitor = new CompanyGroup.WebClient.Models.Visitor(response);
                }
                else
                {
                    visitor = new CompanyGroup.WebClient.Models.Visitor() { Currency = request.Currency };
                }
                //változások mentése a sütibe
                this.WriteCookie(visitorData);

                return visitor;
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
        public CompanyGroup.WebClient.Models.Visitor ChangeLanguage(CompanyGroup.WebClient.Models.ChangeLanguageRequest request)
        {
            try
            {
                CompanyGroup.WebClient.Models.Visitor visitor;

                CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

                visitorData.Language = String.IsNullOrEmpty(request.Language) ? ApiBaseController.LanguageHungarian : request.Language;

                //ha nincs bejelentkezve, akkor nincs szervizhívás sem
                if (!String.IsNullOrEmpty(visitorData.VisitorId))
                {
                    CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest req = new CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest(visitorData.VisitorId, visitorData.Language);

                    CompanyGroup.Dto.PartnerModule.Visitor response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest, CompanyGroup.Dto.PartnerModule.Visitor>("Visitor", "ChangeLanguage", req);

                    visitor = new CompanyGroup.WebClient.Models.Visitor(response);
                }
                else
                {
                    visitor = new CompanyGroup.WebClient.Models.Visitor() { LanguageId = request.Language, InverseLanguageId = request.Language.ToUpper().Equals(ApiBaseController.LanguageHungarian) ? ApiBaseController.LanguageEnglish : ApiBaseController.LanguageHungarian };
                }

                //változások mentése a sütibe
                this.WriteCookie(visitorData);

                return visitor;
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

    }
}
