using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace CompanyGroup.WebApi.Ioc
{
    /// <summary>
    /// IOC container
    /// </summary>
    class Container : ContainerBase, System.Web.Http.Dependencies.IDependencyResolver
    {
        /// <summary>
        /// konstruktor unity container interfésszel
        /// </summary>
        /// <param name="container"></param>
        public Container(Microsoft.Practices.Unity.IUnityContainer container) : base(container)
        {
        }

        /// <summary>
        /// System.Web.Http.Dependencies.IDependencyResolver kötelezően megvalósítandó metódus
        /// </summary>
        /// <returns></returns>
        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            Microsoft.Practices.Unity.IUnityContainer child = container.CreateChildContainer();

            return new ContainerBase(child);
        }
    }
}