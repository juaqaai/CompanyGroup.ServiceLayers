using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// regisztrációval kapcsolatos műveletek
    /// </summary>
    public class RegistrationController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.RegistrationModule.IRegistrationService service;

        /// <summary>
        /// konstruktor regisztrációval kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public RegistrationController(CompanyGroup.ApplicationServices.RegistrationModule.IRegistrationService service)
        {
            this.service = service;
        }


        /// <summary>
        /// kulcs alapján a megkezdett regisztrációs adatok kiolvasása
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetRegistrationByKey")]
        [HttpGet]
        public HttpResponseMessage GetRegistrationByKey(string id, string visitorId)
        {
            return this.GetByKey(new CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey(id, visitorId));
        }

        /// <summary>
        /// kulcs alapján a megkezdett regisztrációs adatok kiolvasása
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionName("GetByKey")]
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage GetByKey(CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request)
        {
            if (String.IsNullOrEmpty(request.Id))
            {
                ThrowSafeException("Id can not be null or empty!", HttpStatusCode.BadRequest);
            }

            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = service.GetByKey(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.Registration>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// új regisztráció hozzáadása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("AddNew")] 
        [HttpPost]
        public HttpResponseMessage AddNew(CompanyGroup.Dto.ServiceRequest.AddNewRegistration request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.Registration response = service.AddNew(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.Registration>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        [ActionName("Remove")] 
        [HttpPost]
        public HttpResponseMessage Remove(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                ThrowSafeException("Id can not be null or empty!", HttpStatusCode.BadRequest);
            }

            try
            {
                service.Remove(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        [ActionName("Post")] 
        [HttpPost]
        public HttpResponseMessage Post(CompanyGroup.Dto.ServiceRequest.PostRegistration request)
        {
            try
            {
                CompanyGroup.Dto.ServiceResponse.PostRegistration response = service.Post(request);

                return Request.CreateResponse<CompanyGroup.Dto.ServiceResponse.PostRegistration>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="dataRecording"></param>
        [ActionName("UpdateDataRecording")] 
        [HttpPost]
        public HttpResponseMessage UpdateDataRecording(CompanyGroup.Dto.ServiceRequest.UpdateDataRecording request)
        {
            try
            {
                CompanyGroup.Dto.ServiceResponse.UpdateDataRecording response = service.UpdateDataRecording(request);

                return Request.CreateResponse<CompanyGroup.Dto.ServiceResponse.UpdateDataRecording>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// regisztrációs adatok módosítása 
        /// </summary>
        /// <param name="request"></param>
        [ActionName("UpdateRegistrationData")] 
        [HttpPost]
        public HttpResponseMessage UpdateRegistrationData(CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData request)
        {
            try
            {
                CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData response = service.UpdateRegistrationData(request);

                return Request.CreateResponse<CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        [ActionName("UpdateWebAdministrator")] 
        [HttpPost]
        public HttpResponseMessage UpdateWebAdministrator(CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator request)
        {
            try
            {
                CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator response = service.UpdateWebAdministrator(request);
                
                return Request.CreateResponse<CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// szállítási címek kiolvasása GetDeliveryAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")] 
        [HttpPost]
        public HttpResponseMessage GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = service.GetDeliveryAddresses(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// szállítási cím hozzáadása
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddDeliveryAddress")] 
        [HttpPost]
        public HttpResponseMessage AddDeliveryAddress(CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = service.AddDeliveryAddress(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// szállítási cím adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateDeliveryAddress")] 
        [HttpPost]
        public HttpResponseMessage UpdateDeliveryAddress(CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response =  service.UpdateDeliveryAddress(request);
                
                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.DeliveryAddresses>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// szállítási cím törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("RemoveDeliveryAddress")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses RemoveDeliveryAddress(CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress request)
        {
            return service.RemoveDeliveryAddress(request);
        }

        /// <summary>
        /// bankszámlaszám lista
        /// </summary>
        /// <param name="request"></param>
        [ActionName("GetBankAccounts")] 
        [HttpPost]
        public HttpResponseMessage GetBankAccounts(CompanyGroup.Dto.ServiceRequest.GetBankAccounts request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.BankAccounts response = service.GetBankAccounts(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.BankAccounts>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// bankszámlaszám felvitele
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddBankAccount")] 
        [HttpPost]
        public HttpResponseMessage AddBankAccount(CompanyGroup.Dto.ServiceRequest.AddBankAccount request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.BankAccounts response = service.AddBankAccount(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.BankAccounts>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// bankszámlaszám adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateBankAccount")] 
        [HttpPost]
        public HttpResponseMessage UpdateBankAccount(CompanyGroup.Dto.ServiceRequest.UpdateBankAccount request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.BankAccounts response = service.UpdateBankAccount(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.BankAccounts>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("RemoveBankAccount")] 
        [HttpPost]
        public HttpResponseMessage RemoveBankAccount(CompanyGroup.Dto.ServiceRequest.RemoveBankAccount request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.BankAccounts response = service.RemoveBankAccount(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.BankAccounts>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddContactPerson")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.ContactPersons AddContactPerson(CompanyGroup.Dto.ServiceRequest.AddContactPerson request)
        {
            return service.AddContactPerson(request);
        }

        /// <summary>
        /// kapcsolattartó kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetContactPersons")]
        [HttpPost]
        public HttpResponseMessage GetContactPersons(CompanyGroup.Dto.ServiceRequest.GetContactPerson request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.ContactPersons response = service.GetContactPersons(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.ContactPersons>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kapcsolattartó adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateContactPerson")]
        [HttpPost]
        public HttpResponseMessage UpdateContactPerson(CompanyGroup.Dto.ServiceRequest.UpdateContactPerson request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.ContactPersons response = service.UpdateContactPerson(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.ContactPersons>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// Kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("RemoveContactPerson")]
        [HttpPost]
        public HttpResponseMessage RemoveContactPerson(CompanyGroup.Dto.ServiceRequest.RemoveContactPerson request)
        {
            try
            {
                CompanyGroup.Dto.RegistrationModule.ContactPersons response = service.RemoveContactPerson(request);

                return Request.CreateResponse<CompanyGroup.Dto.RegistrationModule.ContactPersons>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
