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
			
namespace CompanyGroup.ApplicationServices.InstanceProviders
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
            
            //-> Unit of Work and repositories
            //_currentContainer.RegisterType<IMainBCUnitOfWork, MainBCUnitOfWork>(new PerResolveLifetimeManager());

            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.ICustomerRepository, CompanyGroup.Data.PartnerModule.CustomerRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.ISalesOrderRepository, CompanyGroup.Data.PartnerModule.SalesOrderRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IVisitorRepository, CompanyGroup.Data.PartnerModule.VisitorRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IInvoiceRepository, CompanyGroup.Data.PartnerModule.InvoiceRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IContactPersonRepository, CompanyGroup.Data.PartnerModule.ContactPersonRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IChangePasswordRepository, CompanyGroup.Data.PartnerModule.ChangePasswordRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IForgetPasswordRepository, CompanyGroup.Data.PartnerModule.ForgetPasswordRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IStructureRepository,CompanyGroup.Data.WebshopModule.StructureRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IProductRepository, CompanyGroup.Data.WebshopModule.ProductRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IPictureRepository, CompanyGroup.Data.WebshopModule.PictureRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IShoppingCartRepository, CompanyGroup.Data.WebshopModule.ShoppingCartRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IFinanceRepository, CompanyGroup.Data.WebshopModule.FinanceRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.WebshopModule.INewsletterRepository, CompanyGroup.Data.WebshopModule.NewsletterRepository>();
            _currentContainer.RegisterType<CompanyGroup.Domain.RegistrationModule.IRegistrationRepository, CompanyGroup.Data.RegistrationModule.RegistrationRepository>();


            //-> Adapters
            //_currentContainer.RegisterType<ITypeAdapter, TypeAdapter>();
            //_currentContainer.RegisterType<RegisterTypesMap, ERPModuleRegisterTypesMap>("erpmodule");
            //_currentContainer.RegisterType<RegisterTypesMap, BankingModuleRegisterTypesMap>("bankingmodule");

            //-> Domain Services
            //_currentContainer.RegisterType<IBankTransferService, BankTransferService>();

            //-> Application services
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IProductService, CompanyGroup.ApplicationServices.WebshopModule.ProductService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IPictureService, CompanyGroup.ApplicationServices.WebshopModule.PictureService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IStructureService, CompanyGroup.ApplicationServices.WebshopModule.StructureService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.IVisitorService, CompanyGroup.ApplicationServices.PartnerModule.VisitorService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.ICustomerService, CompanyGroup.ApplicationServices.PartnerModule.CustomerService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.ISalesOrderService, CompanyGroup.ApplicationServices.PartnerModule.SalesOrderService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.IContactPersonService, CompanyGroup.ApplicationServices.PartnerModule.ContactPersonService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IShoppingCartService, CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IFinanceService, CompanyGroup.ApplicationServices.WebshopModule.FinanceService>();
            _currentContainer.RegisterType<CompanyGroup.ApplicationServices.RegistrationModule.IRegistrationService, CompanyGroup.ApplicationServices.RegistrationModule.RegistrationService>();

            _currentContainer.RegisterInstance<CompanyGroup.Data.NoSql.ISettings>(CompanyGroup.Data.NoSql.SettingsFactory.Create(), new ContainerControlledLifetimeManager());

            _currentContainer.RegisterInstance<NHibernate.ISession>(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession(), new ContainerControlledLifetimeManager());

            //-> Distributed Services
            //_currentContainer.RegisterType<IBankingModuleService, BankingModuleService>();
            //_currentContainer.RegisterType<IERPModuleService, ERPModuleService>();
        }


        //static void ConfigureFactories()
        //{
        //    LoggerFactory.SetCurrent(new TraceSourceLogFactory());
        //    EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        //}

        #endregion
    }
}