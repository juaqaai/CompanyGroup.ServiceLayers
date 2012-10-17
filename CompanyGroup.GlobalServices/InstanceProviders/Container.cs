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

namespace CompanyGroup.GlobalServices.InstanceProviders
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
            /*
             * Add here the code configuration or the call to configure the container 
             * using the application configuration file
             */

            _currentContainer = new UnityContainer();

            //-> Application services
            _currentContainer.RegisterType<CompanyGroup.GlobalServices.IProductService, CompanyGroup.GlobalServices.ProductService>();
            //_currentContainer.RegisterType<CompanyGroup.GlobalServices.IPictureService, CompanyGroup.GlobalServices.PictureService>();
            //_currentContainer.RegisterType<CompanyGroup.GlobalServices.IStructureService, CompanyGroup.GlobalServices.StructureService>();
        }

        //static void ConfigureFactories()
        //{
        //    LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        //    EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        //}

        #endregion
    }
}