using System;
using System.Collections.Generic;
using System.Linq;
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
        public CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.PartnerModule.SignInRequest request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "The request cannot be null!");

                CompanyGroup.Helpers.DesignByContract.Require((!String.IsNullOrEmpty(request.Password)), "The password cannot be null or empty!");

                CompanyGroup.Helpers.DesignByContract.Require((!String.IsNullOrEmpty(request.UserName)), "The username cannot be null or empty!");

                //bejelentkezés 
                List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDataList = customerRepository.SignIn(request.UserName, request.Password);

                CompanyGroup.Helpers.DesignByContract.Ensure(visitorDataList != null, "Visitor can not be null!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = new CompanyGroup.Domain.PartnerModule.Visitor(visitorDataList);

                //kérés IP címét menteni kell
                visitor.LoginIP = request.IPAddress;

                //aktív státusz beállítása a bejelentkezést követően (passzív, aktív, permanens)
                visitor.Status = LoginStatus.Active;

                //bejelentkezett állapot beállítása
                visitor.SetLoggedIn();

                //ha nem sikerült a bejelentkezés, vagy nem érvényes a bejelentkezés, akkor üres visitor objektum felhasználásával történik a visszatérés
                if (!visitor.LoggedIn)
                {
                    return new VisitorToVisitor().Map(visitor);
                }

                //telesales mint képviselő adatainak beállítása 
                visitor.Representative.SetDefault();

                //bejelentkezett visitor-t tárolni kell
                visitorRepository.Add(visitor);

                //visitor.Id nem lehet nulla
                CompanyGroup.Helpers.DesignByContract.Ensure(!visitor.IsTransient(), "Visitor can not be transient!");

                //le kell kérdezni a vevőhöz tartozó árbesorolás kivételeket
                visitor.CustomerPriceGroups = customerRepository.GetCustomerPriceGroups(visitor.CustomerId);

                //árcsoportok tárolása 
                visitor.CustomerPriceGroups.ToList().ForEach(x =>
                {
                    x.VisitorKey = visitor.Id;

                    visitorRepository.AddCustomerPriceGroup(x);
                });

                //visitor objektum cache-ben tárolása
                CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(ServiceCoreBase.CACHEKEY_VISITOR, visitor.VisitorId), visitor, DateTime.Now.AddHours(ServiceCoreBase.AuthCookieExpiredHours));

                //mappelés dto-ra
                return new VisitorToVisitor().Map(visitor);
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void SignOut(CompanyGroup.Dto.PartnerModule.SignOutRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                visitorRepository.DisableStatus(request.VisitorId);

                CompanyGroup.Helpers.CacheHelper.Clear(CompanyGroup.Helpers.ContextKeyManager.CreateKey(ServiceCoreBase.CACHEKEY_VISITOR, request.VisitorId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó információk kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor GetVisitorInfo(CompanyGroup.Dto.PartnerModule.VisitorInfoRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.DataAreaId), "DataAreaId cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                //mappelés dto-ra
                return new VisitorToVisitor().Map(visitor);
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor ChangeCurrency(CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Currency), "Currency cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                visitorRepository.ChangeCurrency(request.VisitorId, request.Currency);

                //látogató adatainak kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                //visitor objektum cache-ben tárolása
                CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(ServiceCoreBase.CACHEKEY_VISITOR, visitor.VisitorId), visitor, DateTime.Now.AddHours(ServiceCoreBase.AuthCookieExpiredHours));

                return new VisitorToVisitor().Map(visitor);
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void ChangeLanguage(CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Language), "Language cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                visitorRepository.ChangeLanguage(request.VisitorId, request.Language);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
