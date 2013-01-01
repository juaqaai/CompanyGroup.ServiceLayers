using CompanyGroup.Data.WebshopModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CompanyGroup.Data.NoSql;
using CompanyGroup.Domain.WebshopModule;
using System.Collections.Generic;

namespace CompanyGroup.Data.Test
{
    
    
    /// <summary>
    ///This is a test class for ShoppingCartRepositoryTest and is intended
    ///to contain all ShoppingCartRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShoppingCartRepositoryTest
    {

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            ShoppingCartRepository target = new ShoppingCartRepository(settings); 

            ShoppingCart shoppingCart = new ShoppingCart("4f666a456ee01212480d7eb6", "", "", "", false); 

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem();

            Product product = productRepository.GetItem("AWLESSOPTMOUSEB", "hrp");

            shoppingCartItem.SetProduct(product);

            shoppingCart.Items.Add(shoppingCartItem);

            target.Add(shoppingCart);   

            Assert.IsTrue(!shoppingCart.IsEmpty, "ShoppingCart cannot be empty!");
        }

        /// <summary>
        ///A test for AddItem
        ///</summary>
        [TestMethod()]
        public void AddItemTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            ShoppingCartRepository target = new ShoppingCartRepository(settings);

            Product product = productRepository.GetItem("AMVS238H", "hrp");

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem();

            shoppingCartItem.SetProduct(product);

            MongoDB.Bson.ObjectId objectId;

            MongoDB.Bson.ObjectId.TryParse("4f7629136ee01212fca04f2c", out objectId);

            shoppingCartItem.Id = objectId;

            target.AddLine(shoppingCartItem);
        }

        /// <summary>
        ///A test for GetItemByKey
        ///</summary>
        [TestMethod()]
        public void GetItemByKeyTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            ShoppingCartRepository target = new ShoppingCartRepository(settings);
            string id = "4f7629136ee01212fca04f2c"; 

            ShoppingCart cart = target.GetCartByKey(id);

            Assert.IsNotNull(cart);
        }

        /// <summary>
        ///A test for GetItemsByVisitorId
        ///</summary>
        [TestMethod()]
        public void GetItemsByVisitorIdTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            ShoppingCartRepository target = new ShoppingCartRepository(settings);

            string visitorId = "4f666a456ee01212480d7eb6";

            List<ShoppingCart> carts = target.GetCartCollectionByVisitor(visitorId);

            Assert.IsFalse(carts.Count == 0);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            ISettings settings = null; // TODO: Initialize to an appropriate value
            ShoppingCartRepository target = new ShoppingCartRepository(settings); // TODO: Initialize to an appropriate value
            MongoDB.Bson.ObjectId id = MongoDB.Bson.ObjectId.Empty; // TODO: Initialize to an appropriate value
            target.Remove(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveItem
        ///</summary>
        [TestMethod()]
        public void RemoveLineTest()
        {
            ISettings settings = null; // TODO: Initialize to an appropriate value
            ShoppingCartRepository target = new ShoppingCartRepository(settings); // TODO: Initialize to an appropriate value
            MongoDB.Bson.ObjectId cartId = MongoDB.Bson.ObjectId.Empty; // TODO: Initialize to an appropriate value
            string productId = string.Empty; // TODO: Initialize to an appropriate value
            target.RemoveLine(cartId, productId);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateItemQuantity
        ///</summary>
        [TestMethod()]
        public void UpdateItemQuantityTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            ShoppingCartRepository target = new ShoppingCartRepository(settings);

            string cartId = "4f7629136ee01212fca04f2c";

            string productId = "AMVS238H"; 

            int quantity = 4; 

            target.UpdateLineQuantity(cartId, productId, quantity);
        }
    }
}
