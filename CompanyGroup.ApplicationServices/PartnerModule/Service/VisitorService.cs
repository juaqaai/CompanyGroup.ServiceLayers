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

    }
}
