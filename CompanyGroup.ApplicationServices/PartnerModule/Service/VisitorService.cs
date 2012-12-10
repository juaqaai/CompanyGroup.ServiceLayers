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
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="visitorRepository"></param>
        /// <param name="financeRepository"></param>
        public VisitorService(CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
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

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó jogosultságok listázása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<string> GetRoles(CompanyGroup.Dto.ServiceRequest.VisitorInfo request)
        {
            CompanyGroup.Dto.PartnerModule.Visitor visitor = GetVisitorInfo(request);

            return visitor.Roles;
        }

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor ChangeCurrency(CompanyGroup.Dto.ServiceRequest.ChangeCurrency request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Currency), "Currency cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.ChangeCurrency(request.VisitorId, request.Currency);

            return new VisitorToVisitor().Map(visitor);
        }

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor ChangeLanguage(CompanyGroup.Dto.ServiceRequest.ChangeLanguage request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Language), "Language cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.ChangeLanguage(request.VisitorId, request.Language);

            return new VisitorToVisitor().Map(visitor);
        }
    }
}
