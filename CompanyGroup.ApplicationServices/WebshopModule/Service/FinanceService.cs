using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    [ServiceBehavior(UseSynchronizationContext = false,
                    InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true),
                    System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    [CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()]
    public class FinanceService : IFinanceService
    {
        private CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository;

        public FinanceService(CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository)
        {
            if (financeRepository == null)
            {
                throw new ArgumentNullException("FinanceRepository");
            }

            this.financeRepository = financeRepository;
        }



    }
}
