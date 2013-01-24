using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanyGroup.WebApi;
using CompanyGroup.WebApi.Controllers;
using System.Net.Http.Headers;

namespace CompanyGroup.WebApi.Tests.Controllers
{
    [TestClass]
    public class NewsletterControllerTest
    {
        private CompanyGroup.ApplicationServices.WebshopModule.INewsletterService CreateService()
        {

            CompanyGroup.Domain.WebshopModule.INewsletterRepository newsletterRepository = new Data.WebshopModule.NewsletterRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Visitor"));

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);



            return new CompanyGroup.ApplicationServices.WebshopModule.NewsletterService(newsletterRepository, visitorRepository);
        }

        private static string BaseAddress = "http://localhost/CompanyGroup.WebApi/api/";

        private static bool IsHttpClientTest = true;

        [TestMethod]
        public void GetNewsletterCollection()
        {
            Dto.ServiceRequest.GetNewsletterCollectionRequest request = new Dto.ServiceRequest.GetNewsletterCollectionRequest("hu", "5092cfe46ee0121e98101481", "");

            if (!IsHttpClientTest)
            {
                CompanyGroup.ApplicationServices.WebshopModule.INewsletterService service = CreateService();

                NewsletterController controller = new NewsletterController(service);

                CompanyGroup.Dto.WebshopModule.NewsletterCollection result = controller.GetCollection(request);

                // Assert
                Assert.IsNotNull(result);
            }
            else
            {
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(BaseAddress);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri requestUri = null;

                HttpResponseMessage response = client.PostAsJsonAsync("Newsletter/GetCollection", request).Result;

                if (response.IsSuccessStatusCode)
                {
                    requestUri = response.Headers.Location;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                CompanyGroup.Dto.WebshopModule.NewsletterCollection collection = response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.NewsletterCollection>().Result;

                // Assert
                Assert.IsNotNull(collection);
            }

        }

    }
}
