//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
			
namespace Newsletter.ApplicationService.InstanceProviders
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// DI container accesor
    /// </summary>
    public static class Container
    {
        static  IUnityContainer _currentContainer;

        /// <summary>
        /// Get the current configured container
        /// </summary>
        /// <returns>Configured container</returns>
        public static IUnityContainer Current
        {
            get { return _currentContainer; }
        }

        /// <summary>
        /// konstruktor
        /// </summary>
        static Container()
        {
            ConfigureContainer();

            //ConfigureFactories();
        }

        #region Methods

        static void ConfigureContainer()
        {
            _currentContainer = new UnityContainer();
            
            _currentContainer.RegisterType<Newsletter.Repository.IRecipient, Newsletter.Repository.Recipient>();

            _currentContainer.RegisterType<Newsletter.Repository.ISendOut, Newsletter.Repository.SendOut>();

            _currentContainer.RegisterType<IService, Service>();

            _currentContainer.RegisterInstance<NHibernate.ISession>(Newsletter.Repository.NHibernateSessionManager.Instance.GetSession(), new ContainerControlledLifetimeManager());

        }

        //static void ConfigureFactories()
        //{
        //    LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        //    EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        //}

        #endregion
    }
}