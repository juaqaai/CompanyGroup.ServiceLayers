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
    public class RegistrationController : ApiController
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
        [ActionName("GetByKey")]
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.Registration GetByKey(CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request)
        {
            return service.GetByKey(request);
        }

        /// <summary>
        /// új regisztráció hozzáadása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("AddNew")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.Registration AddNew(CompanyGroup.Dto.ServiceRequest.AddNewRegistration request)
        {
            return service.AddNew(request);
        }

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        [ActionName("Remove")] 
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.Empty Remove(string id)
        {
            return service.Remove(id);
        }

        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        [ActionName("Post")] 
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.PostRegistration Post(CompanyGroup.Dto.ServiceRequest.PostRegistration request)
        {
            return service.Post(request);
        }

        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="dataRecording"></param>
        [ActionName("UpdateDataRecording")] 
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.UpdateDataRecording UpdateDataRecording(CompanyGroup.Dto.ServiceRequest.UpdateDataRecording request)
        {
            return service.UpdateDataRecording(request);
        }

        /// <summary>
        /// regisztrációs adatok módosítása 
        /// </summary>
        /// <param name="request"></param>
        [ActionName("UpdateRegistrationData")] 
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData UpdateRegistrationData(CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData request)
        {
            return service.UpdateRegistrationData(request);
        }

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        [ActionName("UpdateWebAdministrator")] 
        [HttpPost]
        public CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator UpdateWebAdministrator(CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator request)
        {
            return service.UpdateWebAdministrator(request);
        }

        /// <summary>
        /// szállítási címek kiolvasása GetDeliveryAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetDeliveryAddresses")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress request)
        {
            return service.GetDeliveryAddresses(request);
        }

        /// <summary>
        /// szállítási cím hozzáadása
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddDeliveryAddress")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses AddDeliveryAddress(CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress request)
        {
            return service.AddDeliveryAddress(request);
        }

        /// <summary>
        /// szállítási cím adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateDeliveryAddress")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses UpdateDeliveryAddress(CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress request)
        {
            return service.UpdateDeliveryAddress(request);
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
        public CompanyGroup.Dto.RegistrationModule.BankAccounts GetBankAccounts(CompanyGroup.Dto.ServiceRequest.GetBankAccounts request)
        {
            return service.GetBankAccounts(request);
        }

        /// <summary>
        /// bankszámlaszám felvitele
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddBankAccount")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.BankAccounts AddBankAccount(CompanyGroup.Dto.ServiceRequest.AddBankAccount request)
        {
            return service.AddBankAccount(request);
        }

        /// <summary>
        /// bankszámlaszám adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateBankAccount")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.BankAccounts UpdateBankAccount(CompanyGroup.Dto.ServiceRequest.UpdateBankAccount request)
        {
            return service.UpdateBankAccount(request);
        }

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("RemoveBankAccount")] 
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.BankAccounts RemoveBankAccount(CompanyGroup.Dto.ServiceRequest.RemoveBankAccount request)
        {
            return service.RemoveBankAccount(request);
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
        public CompanyGroup.Dto.RegistrationModule.ContactPersons GetContactPersons(CompanyGroup.Dto.ServiceRequest.GetContactPerson request)
        {
            return service.GetContactPersons(request);
        }

        /// <summary>
        /// kapcsolattartó adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("UpdateContactPerson")]
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.ContactPersons UpdateContactPerson(CompanyGroup.Dto.ServiceRequest.UpdateContactPerson request)
        {
            return service.UpdateContactPerson(request);
        }

        /// <summary>
        /// Kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("RemoveContactPerson")]
        [HttpPost]
        public CompanyGroup.Dto.RegistrationModule.ContactPersons RemoveContactPerson(CompanyGroup.Dto.ServiceRequest.RemoveContactPerson request)
        {
            return service.RemoveContactPerson(request);
        }
    }
}
