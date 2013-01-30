using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.WebApi;
using CompanyGroup.WebApi.Controllers;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        private CompanyGroup.ApplicationServices.PartnerModule.ICustomerService CreateService()
        {
            CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            return new CompanyGroup.ApplicationServices.PartnerModule.CustomerService(customerRepository, visitorRepository);
        }

        [TestMethod]
        public void SignIn()
        {
            CompanyGroup.ApplicationServices.PartnerModule.ICustomerService service = CreateService();

            CustomerController controller = new CustomerController(service);

            CompanyGroup.Dto.PartnerModule.SignInRequest request = new CompanyGroup.Dto.PartnerModule.SignInRequest("bsc", "elektroplaza", "58915891", "127.0.0.1");
            
            //CompanyGroup.Dto.PartnerModule.Visitor result = controller.SignIn(request);

            //Assert.IsNotNull(result);
        }

    }
}
