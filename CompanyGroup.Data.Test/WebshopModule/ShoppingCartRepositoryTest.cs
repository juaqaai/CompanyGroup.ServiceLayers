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
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository();

            ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository(); 

            ShoppingCart shoppingCart = new ShoppingCart(1, "alma", "teszt rt", "test2 person", "cart55", "HUF", false); 

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem();

            Product product = productRepository.GetItem("JD990A", "hrp");

            shoppingCartItem.SetProduct(product);

            int cartId = shoppingCartRepository.Add(shoppingCart);

            shoppingCartItem.CartId = cartId;

            shoppingCartRepository.AddLine(shoppingCartItem);

            Assert.IsTrue(cartId > 0, "ShoppingCart cannot be empty!");
        }

        /// <summary>
        ///A test for AddItem
        ///</summary>
        [TestMethod()]
        public void AddItemTest()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository();

            ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository();

            Product product = productRepository.GetItem("AMVS238H", "hrp");

            ShoppingCartItem shoppingCartItem = new ShoppingCartItem();

            shoppingCartItem.SetProduct(product);

            shoppingCartItem.CartId = 1;

            shoppingCartRepository.AddLine(shoppingCartItem);
        }

        /// <summary>
        ///A test for GetItemByKey
        ///</summary>
        [TestMethod()]
        public void GetShoppingCartTest()
        {
            ShoppingCartRepository repository = new ShoppingCartRepository();

            ShoppingCart cart = repository.GetShoppingCart(20);

            Assert.IsNotNull(cart);
        }

        /// <summary>
        ///A test for GetItemsByVisitorId
        ///</summary>
        [TestMethod()]
        public void GetItemsByVisitorIdTest()
        {
            ShoppingCartRepository repository = new ShoppingCartRepository();

            string visitorId = "alma";

            List<ShoppingCart> carts = repository.GetCartCollection(visitorId);

            Assert.IsFalse(carts.Count == 0);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void GetShoppingCartLineTest()
        {
            ShoppingCartRepository repository = new ShoppingCartRepository();

            ShoppingCartItem shoppingCartItem = repository.GetShoppingCartLine(1);

            Assert.IsNotNull(shoppingCartItem);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            ShoppingCartRepository repository = new ShoppingCartRepository(); 

            repository.Remove(2);
        }

        /// <summary>
        ///A test for RemoveItem
        ///</summary>
        [TestMethod()]
        public void RemoveLineTest()
        {
            ShoppingCartRepository repository = new ShoppingCartRepository(); 

            repository.RemoveLine(2);
        }

        /// <summary>
        ///A test for UpdateItemQuantity
        ///</summary>
        [TestMethod()]
        public void UpdateItemQuantityTest()
        {

            ShoppingCartRepository repository = new ShoppingCartRepository();

            int quantity = 4;

            repository.UpdateLineQuantity(1, quantity);

            ShoppingCart shoppingCart = repository.GetShoppingCart(1);

            Assert.IsNotNull(shoppingCart);
        }
    }
}
