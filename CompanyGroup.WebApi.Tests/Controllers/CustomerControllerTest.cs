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

            CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Visitor"));

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);



            return new CompanyGroup.ApplicationServices.PartnerModule.CustomerService(customerRepository, visitorRepository);
        }

        [TestMethod]
        public void SignIn()
        {
            CompanyGroup.ApplicationServices.PartnerModule.ICustomerService service = CreateService();

            CustomerController controller = new CustomerController(service);

            Dto.ServiceRequest.SignInRequest request = new Dto.ServiceRequest.SignInRequest("bsc", "elektroplaza", "58915891", "127.0.0.1");
            
            // Act
            CompanyGroup.Dto.PartnerModule.Visitor result = controller.SignIn(request);

            // Assert
            Assert.IsNotNull(result);
        }

    }
}
