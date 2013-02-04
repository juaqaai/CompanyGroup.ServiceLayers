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
    public class CustomerController : ApiBaseController
    {
        /// <summary>
        /// customer szerviz interfész referencia
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
        public HttpResponseMessage GetAddressZipCodes(string dataAreaId, string prefix) //CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request = new Dto.PartnerModule.AddressZipCodeRequest() { DataAreaId = dataAreaId, Prefix = prefix };

                CompanyGroup.Dto.PartnerModule.AddressZipCodes response = service.GetAddressZipCodes(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.AddressZipCodes>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// vevő regisztrációs adatok kiolvasása vevőazonosító és vállalatkód alapján TODO: CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCustomerRegistration")]
        [HttpGet, HttpPost]
        public HttpResponseMessage GetCustomerRegistration(CompanyGroup.Dto.PartnerModule.GetCustomerRegistrationRequest request)
        {
            if(String.IsNullOrEmpty(request.VisitorId))
            {
                ThrowSafeException("The visitor id cannot be null!", HttpStatusCode.NotFound);
            }            

            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = service.GetCustomerRegistration(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.Registration>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// szállítási címek   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")]
        [HttpPost]
        public HttpResponseMessage GetDeliveryAddresses(CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.DeliveryAddresses response = service.GetDeliveryAddresses(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.DeliveryAddresses>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
