using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.InstanceProviders
{
    public class UnityServiceHost : System.ServiceModel.ServiceHost
    {
        public UnityServiceHost() : base() { }

        public UnityServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
        }

         protected override void OnOpening()
         {
             this.Description.Behaviors.Add(new UnityInstanceProviderServiceBehavior());

             base.OnOpening();
         }
     }
}
