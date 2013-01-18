using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] 
    public class VisitorService : ServiceCoreBase, IVisitorService
    {
        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="visitorRepository"></param>
        /// <param name="customerRepository"></param>
        public VisitorService(CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository, CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository) : base(visitorRepository)
        {
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.ServiceRequest.SignIn request)
        {
            //tárolt eljárás hívással történik a bejelentkezés 
            CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.SignIn(request.UserName, request.Password, request.DataAreaId);

            //kérés IP címét menteni kell
            visitor.LoginIP = request.IPAddress;

            //vállalat kódja, ahov a bejelentkezés történik
            visitor.DataAreaId = request.DataAreaId;

            //aktív státusz beállítása a bejelentkezést követően
            visitor.Status = LoginStatus.Active;

            //bejelentkezett állapot beállítása
            visitor.SetLoggedIn();

            //ha nem sikerült a bejelentkezés, vagy nem érvényes a bejelentkezés, akkor üres visitor objektum felhasználásával történik a visszatérés
            if (!visitor.LoggedIn)
            {
                return new VisitorToVisitor().Map(visitor);
            }

            //ha sikeres a bejelentkezés, akkor le kell kérdezni a vevőhöz tartozó árbesorolás kivételeket
            visitor.CustomerPriceGroups = customerRepository.GetCustomerPriceGroups(request.DataAreaId, visitor.CustomerId);

            //alapértelmezett képviselő adatainak beállítása 
            visitor.Representative.SetDefault();

            //bejelentkezett visitor-t tárolni kell
            visitorRepository.Add(visitor);

            //mappelés dto-ra
            return new VisitorToVisitor().Map(visitor);
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request)
        {
            visitorRepository.DisableStatus(request.ObjectId);
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó információk kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor GetVisitorInfo(CompanyGroup.Dto.ServiceRequest.VisitorInfo request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.DataAreaId), "DataAreaId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.ObjectId), "ObjectId cannot be null, or empty!");

            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.ObjectId);

            //mappelés dto-ra
            return new VisitorToVisitor().Map(visitor);
        }

        ///// <summary>
        ///// bejelentkezett látogatóhoz kapcsolódó jogosultságok listázása
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public List<string> GetRoles(CompanyGroup.Dto.ServiceRequest.VisitorInfo request)
        //{
        //    CompanyGroup.Dto.PartnerModule.Visitor visitor = GetVisitorInfo(request);

        //    return visitor.Roles;
        //}

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void ChangeCurrency(CompanyGroup.Dto.ServiceRequest.ChangeCurrency request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Currency), "Currency cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            visitorRepository.ChangeCurrency(request.VisitorId, request.Currency);
        }

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void ChangeLanguage(CompanyGroup.Dto.ServiceRequest.ChangeLanguage request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Language), "Language cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            visitorRepository.ChangeLanguage(request.VisitorId, request.Language);
        }
    }
}
