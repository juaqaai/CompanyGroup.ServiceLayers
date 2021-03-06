﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Newsletter.ApplicationService.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        public UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Send()
        {
            string id = "1176";

            Newsletter.Repository.ISendOut sendoutRepository = new Newsletter.Repository.SendOut(Newsletter.Repository.NHibernateSessionManager.Instance.GetSession());

            Newsletter.Repository.IRecipient recipientRepository = new Newsletter.Repository.Recipient(Newsletter.Repository.NHibernateSessionManager.Instance.GetSession());

            Newsletter.ApplicationService.IService service = new Newsletter.ApplicationService.Service(sendoutRepository, recipientRepository);

            service.Send(id);
        }

        [TestMethod]
        public void GetExtraAddressListTest()
        {
            Newsletter.Repository.IRecipient recipientRepository = new Newsletter.Repository.Recipient(Newsletter.Repository.NHibernateSessionManager.Instance.GetSession());

            List<Dto.Address> extraAddressList = recipientRepository.GetExtraAddressList("hrp");

            Assert.IsTrue(extraAddressList.Count > 0);
        }
    }
}
