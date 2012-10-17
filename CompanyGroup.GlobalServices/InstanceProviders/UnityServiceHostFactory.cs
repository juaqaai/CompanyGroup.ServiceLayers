using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace CompanyGroup.GlobalServices.InstanceProviders
{

    public class UnityServiceHostFactory : ServiceHostFactory
    {
         protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
         {
             return new UnityServiceHost(serviceType, baseAddresses);
         }
    }
}
