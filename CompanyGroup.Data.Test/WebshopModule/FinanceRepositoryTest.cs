using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test.WebshopModule
{
    [TestClass]
    public class FinanceRepositoryTest
    {
        [TestMethod]
        public void GetCurrentRates()
        {
            CompanyGroup.Domain.WebshopModule.IFinanceRepository repository = new CompanyGroup.Data.WebshopModule.FinanceRepository();

            List<CompanyGroup.Domain.WebshopModule.ExchangeRate> rates = repository.GetCurrentRates();

            Assert.IsNotNull(rates);

            Assert.IsTrue(rates.Count > 0);
        }

        [TestMethod]
        public void GetMinMaxLeasingValues()
        {
            CompanyGroup.Domain.WebshopModule.IFinanceRepository repository = new CompanyGroup.Data.WebshopModule.FinanceRepository();

            CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue values = repository.GetMinMaxLeasingValues();

            Assert.IsNotNull(values);
        }

        [TestMethod]
        public void GetLeasingByFinancedAmount()
        {
            CompanyGroup.Domain.WebshopModule.IFinanceRepository repository = new CompanyGroup.Data.WebshopModule.FinanceRepository();

            List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptions = repository.GetLeasingByFinancedAmount(3000000);

            Assert.IsNotNull(leasingOptions);

            Assert.IsTrue(leasingOptions.Count > 0);
        }
    }
}
