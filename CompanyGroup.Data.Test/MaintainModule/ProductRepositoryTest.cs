using CompanyGroup.Data.MaintainModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using NHibernate;
using CompanyGroup.Domain.MaintainModule;
using System.Collections.Generic;

namespace CompanyGroup.Data.Test.MaintainModule
{
    
    /// <summary>
    ///This is a test class for ItemRepositoryTest and is intended
    ///to contain all ItemRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProductRepositoryTest
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
        ///A test for GetFirstLevelCategoryList
        ///</summary>
        [TestMethod()]
        public void GetFirstLevelCategoryList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> categories = repository.GetFirstLevelCategoryList("hrp");

            Assert.IsNotNull(categories);

            Assert.IsTrue(categories.Count > 0);  
        }

        /// <summary>
        ///A test for GetSecondLevelCategoryList
        ///</summary>
        [TestMethod()]
        public void GetSecondLevelCategoryList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> categories = repository.GetSecondLevelCategoryList("hrp");

            Assert.IsNotNull(categories);

            Assert.IsTrue(categories.Count > 0);
        }

        /// <summary>
        ///A test for GetThirdLevelCategoryList
        ///</summary>
        [TestMethod()]
        public void GetThirdLevelCategoryList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> categories = repository.GetThirdLevelCategoryList("hrp");

            Assert.IsNotNull(categories);

            Assert.IsTrue(categories.Count > 0);
        }

        /// <summary>
        ///A test for GetManufacturerList
        ///</summary>
        [TestMethod()]
        public void GetManufacturerList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.Manufacturer> manufacturers = repository.GetManufacturerList("hrp");

            Assert.IsNotNull(manufacturers);

            Assert.IsTrue(manufacturers.Count > 0);
        }

        /// <summary>
        ///A test for GetProductDescriptionList
        ///</summary>
        [TestMethod()]
        public void GetProductDescriptionList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.ProductDescription> productDescriptions = repository.GetProductDescriptionList("hrp");

            Assert.IsNotNull(productDescriptions);

            Assert.IsTrue(productDescriptions.Count > 0);
        }

        /// <summary>
        ///A test for GetProductManagerList
        ///</summary>
        //[TestMethod()]
        //public void GetProductManagerList()
        //{
        //    CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

        //    List<CompanyGroup.Domain.MaintainModule.ProductManager> productManagers = repository.GetProductManagerList("hrp");

        //    Assert.IsNotNull(productManagers);

        //    Assert.IsTrue(productManagers.Count > 0);
        //}

        /// <summary>
        ///A test for ProductList
        ///</summary>
        [TestMethod()]
        public void GetProductList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.Product> products = repository.GetProductList("hrp");

            Assert.IsNotNull(products);

            Assert.IsTrue(products.Count > 0);
        }

        /// <summary>
        ///test for GetPictureList
        ///</summary>
        [TestMethod()]
        public void GetPictureList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.Picture> pictures = repository.GetPictureList("hrp");

            Assert.IsNotNull(pictures);

            Assert.IsTrue(pictures.Count > 0);
        }

        /// <summary>
        ///test for GetStockList
        ///</summary>
        [TestMethod()]
        public void GetStockList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.Stock> stocks = repository.GetStockList("hrp");

            Assert.IsNotNull(stocks);

            Assert.IsTrue(stocks.Count > 0);
        }

        [TestMethod()]
        public void GetInventNameEnglishList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.InventName> inventNames = repository.GetInventNameEnglishList("hrp");

            Assert.IsNotNull(inventNames);

            Assert.IsTrue(inventNames.Count > 0);
        }

        [TestMethod()]
        public void GetPurchaseOrderLineList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine> purchaseOrderLines = repository.GetPurchaseOrderLineList("hrp");

            Assert.IsNotNull(purchaseOrderLines);

            Assert.IsTrue(purchaseOrderLines.Count > 0);
        }

        /// <summary>
        ///BuildProductList test 
        ///</summary>
        [TestMethod()]
        public void BuildProductList()
        {
            string dataAreaId = Domain.Core.Constants.DataAreaIdHrp;

            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> firstLevelCategories = repository.GetFirstLevelCategoryList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> secondLevelCategories = repository.GetSecondLevelCategoryList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> thirdLevelCategories = repository.GetThirdLevelCategoryList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.Manufacturer> manufacturers = repository.GetManufacturerList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.ProductDescription> productDescriptions = repository.GetProductDescriptionList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.Picture> pictures = repository.GetPictureList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.Stock> stocks = repository.GetStockList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.SecondHand> secondHandList = repository.GetSecondHandList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.InventName> inventNames = repository.GetInventNameEnglishList(dataAreaId);

            List<CompanyGroup.Domain.MaintainModule.Product> products = repository.GetProductList(dataAreaId);  //.Take(500).ToList();

            products.ForEach(delegate(Product item) {

                string langId = item.DataAreaId.ToLower() == Domain.Core.Constants.DataAreaIdHrp || item.DataAreaId.ToLower() == Domain.Core.Constants.DataAreaIdBsc ? Domain.Core.Constants.DataAreaIdHun : Domain.Core.Constants.DataAreaIdSerbia;

                FirstLevelCategory firstLevelCategory = GetFirstLevelCategory(firstLevelCategories, item.Category1Id);

                item.Category1Name = firstLevelCategory.Category1Name;
                item.Category1NameEnglish = firstLevelCategory.Category1NameEnglish;

                SecondLevelCategory secondLevelCategory = GetSecondLevelCategory(secondLevelCategories, item.Category2Id);

                item.Category2Name = secondLevelCategory.Category2Name;
                item.Category2NameEnglish = secondLevelCategory.Category2Name;

                ThirdLevelCategory thirdLevelCategory = GetThirdLevelCategory(thirdLevelCategories, item.Category3Id);

                item.Category3Name = thirdLevelCategory.Category3Name;
                item.Category3NameEnglish = thirdLevelCategory.Category3Name;

                Manufacturer manufacturer = GetManufacturer(manufacturers, item.ManufacturerId);
                item.ManufacturerName = manufacturer.ManufacturerName;
                item.ManufacturerNameEnglish = manufacturer.ManufacturerNameEnglish;

                item.ItemNameEnglish = GetInventNameEnglish(inventNames, item.ProductId);

                item.ShippingDate = DateTime.MinValue;

                ProductDescription descHun = GetProductDescription(productDescriptions, item.ProductId, "hun");

                ProductDescription descEng = GetProductDescription(productDescriptions, item.ProductId, "eng");

                item.Description = descHun.Description;
                item.DescriptionEnglish = descEng.Description;

                item.Pictures.AddRange(GetPictures(pictures, item.ProductId));

                //inventLocationId: hrp: BELSO, KULSO, HASZNALT|bsc: 7000, 1000, 2100|ser: SER

                item.InnerStock = GetStock(dataAreaId, dataAreaId == Domain.Core.Constants.DataAreaIdHrp ? Domain.Core.Constants.InnerStockHrp : Domain.Core.Constants.InnerStockBsc, item.ProductId, stocks);
                item.OuterStock = GetStock(dataAreaId, dataAreaId == Domain.Core.Constants.DataAreaIdHrp ? Domain.Core.Constants.OuterStockHrp : Domain.Core.Constants.OuterStockBsc, item.ProductId, stocks);

                item.SecondHandList = GetSecondHandList(secondHandList, item.ProductId);
                
            });

            Assert.IsNotNull(products);

            Assert.IsTrue(products.Count > 0);

            InsertProducts(products);
        }

        [TestMethod()]
        public void RemoveProductList()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

            productRepository.RemoveAllItemsFromCollection();

        }

        /// <summary>
        ///test for GetStockList
        ///</summary>
        [TestMethod()]
        public void GetCompatibilityItemList()
        {
            CompanyGroup.Domain.MaintainModule.IProductRepository repository = new CompanyGroup.Data.MaintainModule.ProductRepository(NHibernateSessionManager.Instance.GetSession());

            List<CompanyGroup.Domain.MaintainModule.CompatibilityItem> items = repository.GetCompatibilityItemList("B710dn", "hrp", false);

            Assert.IsNotNull(items);

            Assert.IsTrue(items.Count > -1);
        }

        #region BuildProductList private methods

        private string GetInventNameEnglish(List<CompanyGroup.Domain.MaintainModule.InventName> inventNames, string key)
        {
            CompanyGroup.Domain.MaintainModule.InventName inventName = inventNames.FirstOrDefault(x => x.ItemId == key);

            return (inventName != null) ? inventName.ItemName : String.Empty;
        }

        private FirstLevelCategory GetFirstLevelCategory(List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> categories, string key)
        {
            FirstLevelCategory firstLevelCategory = categories.FirstOrDefault(category => category.Category1Id.ToLower() == key.ToLower());

            return firstLevelCategory != null ? firstLevelCategory : new FirstLevelCategory("", "", "", "");
        }

        private SecondLevelCategory GetSecondLevelCategory(List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> categories, string key)
        {
            SecondLevelCategory secondLevelCategory = categories.FirstOrDefault(category => category.Category2Id.ToLower() == key.ToLower());

            return secondLevelCategory != null ? secondLevelCategory : new SecondLevelCategory("", "", "", "");
        }

        private ThirdLevelCategory GetThirdLevelCategory(List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> categories, string key)
        {
            ThirdLevelCategory thirdLevelCategory = categories.FirstOrDefault(category => category.Category3Id.ToLower() == key.ToLower());

            return thirdLevelCategory != null ? thirdLevelCategory : new ThirdLevelCategory("", "", "", "");
        }

        private Manufacturer GetManufacturer(List<CompanyGroup.Domain.MaintainModule.Manufacturer> manufacturers, string key)
        {
            Manufacturer manufacturer = manufacturers.FirstOrDefault(manu => manu.ManufacturerId.ToLower() == key.ToLower());

            return manufacturer != null ? manufacturer : new Manufacturer("", "", "", "");
        }

        private ProductDescription GetProductDescription(List<CompanyGroup.Domain.MaintainModule.ProductDescription> descriptions, string key, string langId)
        {
            ProductDescription description = descriptions.FirstOrDefault(d => d.ProductId.ToLower() == key.ToLower() && d.LangId == langId);

            return description != null ? description : new ProductDescription("", "", "");
        }

        //private ProductManager GetProductManager(List<CompanyGroup.Domain.MaintainModule.ProductManager> managers, string key)
        //{
        //    ProductManager manager = managers.FirstOrDefault(m => m.EmployeeId.ToLower() == key.ToLower());

        //    return manager != null ? manager : new ProductManager("", "", "", "", "");
        //}

        private List<Picture> GetPictures(List<CompanyGroup.Domain.MaintainModule.Picture> pictures, string key)
        {
            List<Picture> pictureList = pictures.FindAll(m => m.ItemId.ToLower() == key.ToLower());

            return pictureList != null ? pictureList : new List<Picture>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAreaId">hrp, bsc, ser</param>
        /// <param name="inventLocation">hrp: BELSO, KULSO, HASZNALT|bsc: 7000, 1000, 2100|ser: SER</param>
        /// <param name="productId"></param>
        /// <param name="stocks"></param>
        /// <returns></returns>
        private int GetStock(string dataAreaId, string inventLocation, string productId, List<CompanyGroup.Domain.MaintainModule.Stock> stocks)
        {
            CompanyGroup.Domain.MaintainModule.Stock stock = stocks.SingleOrDefault(x => x.DataAreaId.ToLower().Equals(dataAreaId.ToLower()) && x.InventLocationId.ToUpper().Equals(inventLocation.ToUpper()) && x.ProductId.ToLower().Equals(productId.ToLower()));

            return (stock != null) ? stock.Quantity : 0;
        }

        private List<SecondHand> GetSecondHandList(List<CompanyGroup.Domain.MaintainModule.SecondHand> list, string key)
        {
            List<SecondHand> results = list.FindAll(m => m.ProductId.ToLower() == key.ToLower());

            return results != null ? results : new List<SecondHand>();
        }

        /// <summary>
        /// insert product entity to nosql documentum collection
        /// </summary>
        /// <param name="items"></param>
        private void InsertProducts(List<CompanyGroup.Domain.MaintainModule.Product> items)
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                  CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

            CompanyGroup.Domain.WebshopModule.Pager pager = new CompanyGroup.Domain.WebshopModule.Pager(0, 0, 0);

            CompanyGroup.Domain.WebshopModule.Products products = new CompanyGroup.Domain.WebshopModule.Products(pager);
            
            items.ForEach( x => products.Add( CompanyGroup.Domain.WebshopModule.Factory.CreateProduct(x) ) );

            productRepository.InsertList(products);
        }

        #endregion
    }
}
