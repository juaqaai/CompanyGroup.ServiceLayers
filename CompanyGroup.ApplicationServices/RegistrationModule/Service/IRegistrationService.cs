using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    [ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.RegistrationModule/", Name = "RegistrationService")]
    public interface IRegistrationService
    {
        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó regisztrációs információk kiolvasása            
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetByKey")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/GetByKey",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.Registration GetByKey(CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request);

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="request"></param>
        //[OperationContract(Action = "Add")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/Add",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //string Add(CompanyGroup.Dto.RegistrationModule.Registration request);

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "AddNew")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/AddNew",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.Registration AddNew(CompanyGroup.Dto.ServiceRequest.AddNewRegistration request);

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(Action = "Remove")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/Remove",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.Empty Remove(string id);

        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        [OperationContract(Action = "Post")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/Post",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.PostRegistration Post(CompanyGroup.Dto.ServiceRequest.PostRegistration request);

        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="dataRecording"></param>
        [OperationContract(Action = "UpdateDataRecording")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateDataRecording",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.UpdateDataRecording UpdateDataRecording(CompanyGroup.Dto.ServiceRequest.UpdateDataRecording request);

        /// <summary>
        /// regisztrációs adatok módosítása 
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "UpdateRegistrationData")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateRegistrationData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData UpdateRegistrationData(CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData request);

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        [OperationContract(Action = "UpdateWebAdministrator")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateWebAdministrator",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator UpdateWebAdministrator(CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator request);

        /// <summary>
        /// szállítási cím adatok kiolvasása
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "GetDeliveryAddresses")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/GetDeliveryAddresses",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress request);

        /// <summary>
        /// szállítási cím hozzáadása
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "AddDeliveryAddress")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/AddDeliveryAddress",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.DeliveryAddresses AddDeliveryAddress(CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress request);

        /// <summary>
        /// szállítási cím adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "UpdateDeliveryAddress")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateDeliveryAddress",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.DeliveryAddresses UpdateDeliveryAddress(CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress request);

        /// <summary>
        /// szállítási cím törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "RemoveDeliveryAddress")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/RemoveDeliveryAddress",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.DeliveryAddresses RemoveDeliveryAddress(CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress request);

        /// <summary>
        /// bankszámlaszám lista
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "GetBankAccounts")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/GetBankAccounts",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.BankAccounts GetBankAccounts(CompanyGroup.Dto.ServiceRequest.GetBankAccounts request);

        /// <summary>
        /// bankszámlaszám felvitele
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "AddBankAccount")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/AddBankAccount",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.BankAccounts AddBankAccount(CompanyGroup.Dto.ServiceRequest.AddBankAccount request);

        /// <summary>
        /// bankszámlaszám adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "UpdateBankAccount")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateBankAccount",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.BankAccounts UpdateBankAccount(CompanyGroup.Dto.ServiceRequest.UpdateBankAccount request);

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "RemoveBankAccount")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/RemoveBankAccount",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.BankAccounts RemoveBankAccount(CompanyGroup.Dto.ServiceRequest.RemoveBankAccount request);

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "AddContactPerson")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/AddContactPerson",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.ContactPersons AddContactPerson(CompanyGroup.Dto.ServiceRequest.AddContactPerson request);

        /// <summary>
        /// kapcsolattartók kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetContactPersons")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/GetContactPersons",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.ContactPersons GetContactPersons(CompanyGroup.Dto.ServiceRequest.GetContactPerson request);

        /// <summary>
        /// kapcsolattartó adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "UpdateContactPerson")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/UpdateContactPerson",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.ContactPersons UpdateContactPerson(CompanyGroup.Dto.ServiceRequest.UpdateContactPerson request);

        /// <summary>
        /// Kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "RemoveContactPerson")]
        [WebInvoke(Method = "POST",
            UriTemplate = "/RemoveContactPerson",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.ContactPersons RemoveContactPerson(CompanyGroup.Dto.ServiceRequest.RemoveContactPerson request);
    }
}
