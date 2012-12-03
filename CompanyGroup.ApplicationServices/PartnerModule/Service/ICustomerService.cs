using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.PartnerModule/", Name = "CustomerService")]
    public interface ICustomerService
    {
        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetAddressZipCodes")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetAddressZipCodes",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebGet(UriTemplate = "/Client/{id}", BodyStyle = WebMessageBodyStyle.Bare)]  string prefix, string dataAreaId
        CompanyGroup.Dto.PartnerModule.AddressZipCodes GetAddressZipCodes(CompanyGroup.Dto.ServiceRequest.AddressZipCode request);

        /// <summary>
        /// vevő regisztrációs adatok kiolvasása vevőazonosító és vállalatkód alapján
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetCustomerRegistration")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetCustomerLetter",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request);

        /// <summary>
        /// szállítási címek   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetDeliveryAddresses")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetDeliveryAddresses",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses request);

        /// <summary>
        /// bejelentkezés           
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "SignIn")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/SignIn",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.ServiceRequest.SignIn request);

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        //[OperationContract(Action = "SignOut")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/SignOut",
        //    RequestFormat = WebMessageFormat.Json, 
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.ServiceResponse.Empty SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request);

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[OperationContract(Action = "GetInvoiceInfo")]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "/GetInvoiceInfo",
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetInvoiceInfo(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request);
    }
}
