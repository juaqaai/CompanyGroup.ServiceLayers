using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.Unity;

namespace CompanyGroup.WebApi.Ioc
{
    /// <summary>
    /// IOC container ős
    /// </summary>
    class ContainerBase : System.Web.Http.Dependencies.IDependencyScope
    {
        /// <summary>
        /// unity conatiner példány
        /// </summary>
        protected Microsoft.Practices.Unity.IUnityContainer container;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="container"></param>
        public ContainerBase(Microsoft.Practices.Unity.IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        /// <summary>
        /// System.Web.Http.Dependencies.IDependencyScope kötelezően előírt metódus
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            if (container.IsRegistered(serviceType))
            {
                return container.Resolve(serviceType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// System.Web.Http.Dependencies.IDependencyScope kötelezően előírt metódus
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (container.IsRegistered(serviceType))
            {
                return container.ResolveAll(serviceType);
            }
            else
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// System.Web.Http.Dependencies.IDependencyScope kötelezően előírt metódus
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }
    }
}