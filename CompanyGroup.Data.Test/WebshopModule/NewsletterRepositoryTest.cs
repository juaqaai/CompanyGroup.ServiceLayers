using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test.WebshopModule
{
    [TestClass]
    public class NewsletterRepositoryTest
    {

        [TestMethod]
        public void GetNewsletterList()
        {
            CompanyGroup.Domain.WebshopModule.INewsletterRepository repository = new CompanyGroup.Data.WebshopModule.NewsletterRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.WebshopModule.Newsletter> newsletters = repository.GetNewsletterList(10, "hrp", "", "");

            Assert.IsNotNull(newsletters);

            Assert.IsTrue(newsletters.Count > 0);
        }
    }
}
