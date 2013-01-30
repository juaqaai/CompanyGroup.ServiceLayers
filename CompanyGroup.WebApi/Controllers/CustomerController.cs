using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// vevőhöz kapcsolódó műveletek
    /// </summary>
    public class CustomerController : ApiController
    {
        /// <summary>
        /// privát szerviz interfész referencia
        /// </summary>
        private CompanyGroup.ApplicationServices.PartnerModule.ICustomerService service;

        /// <summary>
        /// konstruktor customer service interfésszel
        /// </summary>
        /// <param name="service"></param>
        public CustomerController(CompanyGroup.ApplicationServices.PartnerModule.ICustomerService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("CustomerService");
            }

            this.service = service;        
        }

        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetAddressZipCodes")]
        [HttpGet]
        public CompanyGroup.Dto.PartnerModule.AddressZipCodes GetAddressZipCodes(CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request)
        {
            return service.GetAddressZipCodes(request);
        }

        /// <summary>
        /// vevő regisztrációs adatok kiolvasása vevőazonosító és vállalatkód alapján TODO: CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCustomerRegistration")]
        [HttpGet]
        public CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(string visitorId, string dataAreaId)
        {
            if(String.IsNullOrEmpty(visitorId))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            CompanyGroup.Dto.PartnerModule.GetCustomerRegistrationRequest request = new CompanyGroup.Dto.PartnerModule.GetCustomerRegistrationRequest(visitorId, dataAreaId);

            return service.GetCustomerRegistration(request);
        }

        /// <summary>
        /// szállítási címek   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")]
        [HttpPost]
        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest request)
        {
            return service.GetDeliveryAddresses(request);
        }

    }
}
