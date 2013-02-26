using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace CompanyGroup.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Configure(GlobalConfiguration.Configuration);
        }

        void Configure(HttpConfiguration httpConfiguration)
        {
            Microsoft.Practices.Unity.UnityContainer unityContainer = new Microsoft.Practices.Unity.UnityContainer();
            //unity.RegisterType<BooksController>();
            //unity.RegisterType<IBookRepository, BookRepository>(new Microsoft.Practices.Unity.HierarchicalLifetimeManager());

            //-> WebApi controllers
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.ContactPersonController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.CustomerController>();
            //unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.FinanceController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.InvoiceController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.MaintainController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.NewsletterController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.PictureController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.ProductController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.RegistrationController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.SalesOrderController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.ShoppingCartController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.StructureController>();
            unityContainer.RegisterType<CompanyGroup.WebApi.Controllers.VisitorController>();

            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.ICustomerRepository, CompanyGroup.Data.PartnerModule.CustomerRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.ISalesOrderRepository, CompanyGroup.Data.PartnerModule.SalesOrderRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IVisitorRepository, CompanyGroup.Data.PartnerModule.VisitorRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IContactPersonRepository, CompanyGroup.Data.PartnerModule.ContactPersonRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IChangePasswordRepository, CompanyGroup.Data.PartnerModule.ChangePasswordRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IForgetPasswordRepository, CompanyGroup.Data.PartnerModule.ForgetPasswordRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IStructureRepository, CompanyGroup.Data.WebshopModule.StructureRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IProductRepository, CompanyGroup.Data.WebshopModule.ProductRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IPictureRepository, CompanyGroup.Data.WebshopModule.PictureRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IShoppingCartRepository, CompanyGroup.Data.WebshopModule.ShoppingCartRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.IFinanceRepository, CompanyGroup.Data.WebshopModule.FinanceRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.WebshopModule.INewsletterRepository, CompanyGroup.Data.WebshopModule.NewsletterRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.RegistrationModule.IRegistrationRepository, CompanyGroup.Data.RegistrationModule.RegistrationRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.MaintainModule.ISyncRepository, CompanyGroup.Data.MaintainModule.SyncRepository>();
            unityContainer.RegisterType<CompanyGroup.Domain.PartnerModule.IInvoiceRepository, CompanyGroup.Data.PartnerModule.InvoiceRepository>();

            //unityContainer.Configure<InjectedMembers>().

            //-> Application services
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IProductService, CompanyGroup.ApplicationServices.WebshopModule.ProductService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IPictureService, CompanyGroup.ApplicationServices.WebshopModule.PictureService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IStructureService, CompanyGroup.ApplicationServices.WebshopModule.StructureService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.IVisitorService, CompanyGroup.ApplicationServices.PartnerModule.VisitorService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.ICustomerService, CompanyGroup.ApplicationServices.PartnerModule.CustomerService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.ISalesOrderService, CompanyGroup.ApplicationServices.PartnerModule.SalesOrderService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.IContactPersonService, CompanyGroup.ApplicationServices.PartnerModule.ContactPersonService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IShoppingCartService, CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.IFinanceService, CompanyGroup.ApplicationServices.WebshopModule.FinanceService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.MaintainModule.ISyncService, CompanyGroup.ApplicationServices.MaintainModule.SyncService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.WebshopModule.INewsletterService, CompanyGroup.ApplicationServices.WebshopModule.NewsletterService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.RegistrationModule.IRegistrationService, CompanyGroup.ApplicationServices.RegistrationModule.RegistrationService>();
            unityContainer.RegisterType<CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService, CompanyGroup.ApplicationServices.PartnerModule.InvoiceService>();

            //unityContainer.RegisterInstance<CompanyGroup.Data.NoSql.ISettings>(CompanyGroup.Data.NoSql.SettingsFactory.Create(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<CompanyGroup.Data.NoSql.ISettings>(new InjectionFactory(c => CompanyGroup.Data.NoSql.SettingsFactory.Create()));

            //unityContainer.RegisterType<NHibernate.ISession, >();
            //unityContainer.RegisterType<NHibernate.ISession>(new InjectionFactory(c => CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession()));

            httpConfiguration.DependencyResolver = new CompanyGroup.WebApi.Ioc.Container(unityContainer);
        }
    }
}