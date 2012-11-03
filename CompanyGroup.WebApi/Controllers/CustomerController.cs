using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerController : ApiController
    {
        private CompanyGroup.ApplicationServices.PartnerModule.ICustomerService service;

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
        public CompanyGroup.Dto.PartnerModule.AddressZipCodes GetAddressZipCodes(CompanyGroup.Dto.ServiceRequest.AddressZipCode request)
        {
            return service.GetAddressZipCodes(request);
        }

        /// <summary>
        /// vevő regisztrációs adatok kiolvasása vevőazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCustomerRegistration")]
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request)
        {
            return service.GetCustomerRegistration(request);
        }

        /// <summary>
        /// szállítási címek   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")]
        [HttpPost]
        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses request)
        {
            return service.GetDeliveryAddresses(request);
        }

        /// <summary>
        /// bejelentkezés           
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("SignIn")]
        [HttpPost]
        public CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.ServiceRequest.SignIn request)
        {
            return service.SignIn(request);
        }

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        [ActionName("SignOut")]
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.Empty SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request)
        {
            return service.SignOut(request);
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetInvoiceInfo")]
        [HttpPost]
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetInvoiceInfo(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request)
        {
            return service.GetInvoiceInfo(request);
        }
    }
}
