using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.ApplicationServices.Test
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

        private readonly static string ServiceBaseUrl = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ServiceBaseUrl", "http://1Juhasza/CompanyGroup.ServicesHost/{0}.svc/");

        protected readonly static string DataAreaId = CompanyGroup.Helpers.ConfigSettingsParser.GetString("DataAreaId", "hrp");

        private static string BaseUrl(string serviceName)
        {
            return String.Format(ServiceBaseUrl, serviceName);
        }

        /// <summary>
        /// post json data to service url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName">Catalogue</param>
        /// <param name="resource">GetStructures</param>
        /// <param name="requestBody">new { DataAreaId = dataAreaId, ActionFilter = false, BargainFilter = false, NewFilter = false, StockFilter = false, TextFilter = textFilter }</param>
        /// <returns></returns>
        protected T PostJSonData<T>(string serviceName, string resource, object requestBody) where T : new()
        {
            RestSharp.RestClient client = null;

            try
            {
                client = new RestSharp.RestClient(BaseUrl(serviceName));

                RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.POST);

                request.RequestFormat = RestSharp.DataFormat.Json;

                request.Resource = resource;

                request.AddBody(requestBody);

                RestSharp.RestResponse<T> response = client.Execute<T>(request);

                return response.Data;
            }
            catch { return default(T); }
        }

        protected System.IO.Stream GetData(string serviceName, RestSharp.RestRequest request)
        {
            RestSharp.RestClient client = null;

            try
            {
                client = new RestSharp.RestClient(BaseUrl(serviceName));

                RestSharp.RestResponse response = client.Execute(request);

                return new System.IO.MemoryStream(response.RawBytes);
            }
            catch { return new System.IO.MemoryStream(new byte[0]); }
        }

        #endregion

        #region  "Structure test case"

        [TestMethod]
        public void StructureGetAll()
        {
            CompanyGroup.Dto.ServiceRequest.GetAllStructureRequest request = new CompanyGroup.Dto.ServiceRequest.GetAllStructureRequest() { ActionFilter = false, BargainFilter = false, BscFilter = false, Category1IdList = new List<string>(), Category2IdList = new List<string>(), Category3IdList = new List<string>(), HrpFilter = false, IsInNewsletterFilter = false,  ManufacturerIdList = new List<string>(), NewFilter = false, StockFilter = false, TextFilter = "" };

            CompanyGroup.Data.WebshopModule.StructureRepository structureRepository = new CompanyGroup.Data.WebshopModule.StructureRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Dto.WebshopModule.Structures structures = new CompanyGroup.ApplicationServices.WebshopModule.StructureService(structureRepository).GetAll(request);

            Assert.IsNotNull(structures);

            Assert.IsNotNull(structures.Manufacturers);

            Assert.IsNotNull(structures.FirstLevelCategories);

            Assert.IsNotNull(structures.SecondLevelCategories);

            Assert.IsNotNull(structures.ThirdLevelCategories);
        }

        [TestMethod]
        public void StructureGetAllRest()
        {
            CompanyGroup.Dto.ServiceRequest.GetAllStructureRequest request = new CompanyGroup.Dto.ServiceRequest.GetAllStructureRequest() { ActionFilter = false, BargainFilter = false, BscFilter = false, Category1IdList = new List<string>(), Category2IdList = new List<string>(), Category3IdList = new List<string>(), HrpFilter = false, IsInNewsletterFilter = false, ManufacturerIdList = new List<string>(), NewFilter = false, StockFilter = false, TextFilter = "" };

            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.WebshopModule.Structures>("StructureService", "GetAll", request);

            Assert.IsNotNull(structures);

            Assert.IsNotNull(structures.Manufacturers);

            Assert.IsNotNull(structures.FirstLevelCategories);

            Assert.IsNotNull(structures.SecondLevelCategories);

            Assert.IsNotNull(structures.ThirdLevelCategories);
        }

        #endregion

        #region "Product test case"

        [TestMethod]
        public void ProductGetAll()
        {
            CompanyGroup.Dto.ServiceRequest.GetAllProductRequest request = new CompanyGroup.Dto.ServiceRequest.GetAllProductRequest()
                                                                        {
                                                                            ActionFilter = false,
                                                                            BargainFilter = false, 
                                                                            BscFilter = false, 
                                                                            Category1IdList = new List<string>() { "B011" },
                                                                            Category2IdList = new List<string>(),
                                                                            Category3IdList = new List<string>(),
                                                                            Currency = "HUF", 
                                                                            HrpFilter = false, 
                                                                            IsInNewsletterFilter = false, 
                                                                            ManufacturerIdList = new List<string>(),
                                                                            NewFilter = false,
                                                                            StockFilter = false,
                                                                            TextFilter = "",
                                                                            CurrentPageIndex = 0,
                                                                            ItemsOnPage = 20,
                                                                            Sequence = 0,
                                                                            VisitorId = ""
                                                                        };

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Dto.WebshopModule.Products products = new CompanyGroup.ApplicationServices.WebshopModule.ProductService(productRepository, shoppingCartRepository, financeRepository, visitorRepository).GetProducts(request);

            Assert.IsNotNull(products);

            Assert.IsTrue(products.Items.Count > 0);
        }

        [TestMethod]
        public void ProductGetAllRest()
        {
            //http://localhost/CompanyGroup.ServicesHost/ProductService.svc/GetAll             

            //CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetAll", new { ActionFilter = false, BargainFilter = false, Category1Id = "", Category2Id = "", Category3Id = "", DataAreaId = CatalogueController.DataAreaId, ManufacturerId = "", NewFilter = false, StockFilter = false, TextFilter = "" });

            //CompanyGroup.Dto.ServiceRequest.ProductFilter productFilter = new CompanyGroup.Dto.ServiceRequest.ProductFilter() { ActionFilter = false, BargainFilter = false, Category1Id = "", Category2Id = "", Category3Id = "", CurrentPageIndex = 0, DataAreaId = DataAreaId, ItemsOnPage = 20, ManufacturerId = "", NewFilter = false, Sequence = 2, StockFilter = false, TextFilter = "" };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.Products>("ProductService", "GetAll", new { ActionFilter = false, BargainFilter = false, Category1Id = "", Category2Id = "", Category3Id = "", CurrentPageIndex = 0, DataAreaId = UnitTest.DataAreaId, ItemsOnPage = 20, ManufacturerId = "", NewFilter = false, Sequence = 2, StockFilter = false, TextFilter = "" });

            Assert.IsNotNull(products);

            Assert.IsTrue(products.Items.Count > 0);
        }

        [TestMethod]
        public void GetPriceList()
        {
            CompanyGroup.Dto.ServiceRequest.GetPriceListRequest request = new CompanyGroup.Dto.ServiceRequest.GetPriceListRequest()
            {
                ActionFilter = false,
                BargainFilter = false,
                BscFilter = false,
                Category1IdList = new List<string>() { "B011" },
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = "HUF",
                HrpFilter = false,
                IsInNewsletterFilter = false,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                StockFilter = false,
                TextFilter = "",
                Sequence = 0,
                VisitorId = "502111910de52b2484b80269"
            };

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Dto.WebshopModule.PriceList priceList = new CompanyGroup.ApplicationServices.WebshopModule.ProductService(productRepository, shoppingCartRepository, financeRepository, visitorRepository).GetPriceList(request);

            Assert.IsNotNull(priceList);

            Assert.IsTrue(priceList.Items.Count > 0);
        }

        [TestMethod]
        public void GetProductByProductIdRest()
        {
            //http://localhost/CompanyGroup.ServicesHost/ProductService.svc/GetItemByProductId

            CompanyGroup.Dto.ServiceRequest.GetItemByProductIdRequest request = new CompanyGroup.Dto.ServiceRequest.GetItemByProductIdRequest() { ProductId = "PGI7BK", DataAreaId = UnitTest.DataAreaId };

            CompanyGroup.Dto.WebshopModule.ProductDetails productDetails = this.PostJSonData<CompanyGroup.Dto.WebshopModule.ProductDetails>("ProductService", "GetItemByProductId", request);

            Assert.IsNotNull(productDetails);
        }

        [TestMethod]
        public void GetItemByProductId()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ProductService productService = new CompanyGroup.ApplicationServices.WebshopModule.ProductService(productRepository, shoppingCartRepository, financeRepository, visitorRepository);
                
            CompanyGroup.Dto.WebshopModule.Product product = productService.GetItemByProductId(new CompanyGroup.Dto.ServiceRequest.GetItemByProductIdRequest() { ProductId = "B710dn", DataAreaId = "hrp", VisitorId = "" });

            Assert.IsNotNull(product);
        }

        //[TestMethod]
        //public void RefillProductCache()
        //{

        //    CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
        //                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
        //                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
        //                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

        //    CompanyGroup.Data.WebshopModule.ProductRepository productWebshopRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(settings);

        //    CompanyGroup.Data.MaintainModule.ProductRepository productMaintainRepository = new CompanyGroup.Data.MaintainModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

        //    CompanyGroup.ApplicationServices.MaintainModule.IProductService service = new CompanyGroup.ApplicationServices.MaintainModule.ProductService(productMaintainRepository, productWebshopRepository);

        //    service.FillProductCache(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);   //CompanyGroup.Domain.Core.Constants.DataAreaIdHrp
        //}

        //[TestMethod]
        //public void GetComplationList()
        //{
        //    CompanyGroup.Data.WebshopModule.ProductRepository productWebshopRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

        //    CompanyGroup.Domain.WebshopModule.CompletionList result = productWebshopRepository.GetComplationList("dell", "hrp", 20, Domain.WebshopModule.CompletionType.Full);

        //    Assert.IsNotNull(result);

        //    Assert.IsTrue(result.Count > 0);
        //}

        #endregion

        #region "Picture test cases"

        [TestMethod]
        public void PictureGetItem()
        {
            CompanyGroup.Dto.ServiceRequest.PictureFilterRequest request = new CompanyGroup.Dto.ServiceRequest.PictureFilterRequest()
            {
                DataAreaId = "ser",
                ProductId = "PFI702GY"
            };

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ProductList"));

            CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository = new CompanyGroup.Data.WebshopModule.PictureRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            System.IO.Stream stream = new CompanyGroup.ApplicationServices.WebshopModule.PictureService(pictureRepository).GetItem("5637193425", request.ProductId, "180", "180");

            Assert.IsNotNull(stream);

            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void PictureGetItemRest()
        {

            RestSharp.RestRequest request = new RestSharp.RestRequest(RestSharp.Method.GET);

            request.RequestFormat = RestSharp.DataFormat.Json;

            request.RootElement = "Item";

            request.Resource = "GetItem/{RecId}/{ProductId}";

            request.AddParameter("RecId", 5637193425, RestSharp.ParameterType.UrlSegment);

            request.AddParameter("ProductId", "PFI702GY", RestSharp.ParameterType.UrlSegment);

            //request.AddBody(requestBody);

            System.IO.Stream stream = this.GetData("PictureService", request);

            Assert.IsNotNull(stream);

            Assert.IsTrue(stream.Length > 0);
        }

        #endregion

        #region "SigIn test"

        [TestMethod]
        public void SignInTest()
        { 
            CompanyGroup.Dto.ServiceRequest.SignInRequest request = new Dto.ServiceRequest.SignInRequest("hrp", "elektroplaza", "58915891", "");

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Visitor"));

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.ApplicationServices.PartnerModule.CustomerService customerService = new CompanyGroup.ApplicationServices.PartnerModule.CustomerService(customerRepository, visitorRepository);

            CompanyGroup.Dto.PartnerModule.Visitor visitor = customerService.SignIn(request);

            Assert.IsNotNull(visitor);
        }

        [TestMethod]
        public void SignOutTest()
        {
            CompanyGroup.Dto.ServiceRequest.SignOut request = new CompanyGroup.Dto.ServiceRequest.SignOut() { DataAreaId = "hrp", ObjectId = "5039d7e66ee01213b0c8c201" };

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                               CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "Visitor"));

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.ApplicationServices.PartnerModule.CustomerService customerService = new CompanyGroup.ApplicationServices.PartnerModule.CustomerService(customerRepository, visitorRepository);

            CompanyGroup.Dto.ServiceResponse.Empty response = customerService.SignOut(request);

            Assert.IsNotNull(response);
        }

        #endregion

        #region "ShoppingCart test cases"

        [TestMethod()]
        public void AssociateCart()
        { 
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.AssociateCart request = new CompanyGroup.Dto.ServiceRequest.AssociateCart("503a0aad6ee01209a42e0c41", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AssociateCart(request);

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddCartTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.AddCart request = new CompanyGroup.Dto.ServiceRequest.AddCart("hu", "5039d7e66ee01213b0c8c201", "kosar neve");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AddCart(request);

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for AddItem
        ///</summary>
        [TestMethod()]
        public void AddItemTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.AddLineRequest request = new CompanyGroup.Dto.ServiceRequest.AddLineRequest();

            request.CartId = "5039d98b6ee01212f8abc3e5";
            request.DataAreaId = "hrp";
            request.Language = "hu";
            request.ProductId = "FTSP24W-6 LED";
            request.Quantity = 2;
            request.VisitorId = "5039d7e66ee01213b0c8c201";

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = service.AddLine(request);

            Assert.IsNotNull(shoppingCart);
        }

        /// <summary>
        ///A test for GetCartByKeyTests
        ///</summary>
        [TestMethod()]
        public void GetCartByKeyTests()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.GetCartByKeyRequest request = new CompanyGroup.Dto.ServiceRequest.GetCartByKeyRequest("hu", "5039d98b6ee01212f8abc3e5", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCart cart = service.GetCartByKey(request);

            Assert.IsNotNull(cart);
        }

        [TestMethod()]
        public void GetActiveCartTests()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);
            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.GetActiveCartRequest request = new CompanyGroup.Dto.ServiceRequest.GetActiveCartRequest("hu", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo cart = service.GetActiveCart(request);

            Assert.IsNotNull(cart);
        }

        /// <summary>
        ///A test for GetCartCollectionByVisitor
        ///</summary>
        [TestMethod()]
        public void GetCartCollectionByVisitorTest()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "ShoppingCart"));

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);
            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey("5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitorRequest request = new CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitorRequest("hu", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartCollection sc = service.GetCartCollectionByVisitor(request);

            Assert.IsFalse(sc.Carts.Count == 0);
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


            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(settings);

            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);
            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.ServiceRequest.UpdateLineQuantityRequest request = new CompanyGroup.Dto.ServiceRequest.UpdateLineQuantityRequest();

            request.CartId = "5039d98b6ee01212f8abc3e5";
            request.DataAreaId = "hrp";
            request.Language = "en";
            request.ProductId = "FTSP24W-6 LED";
            request.Quantity = 5;
            request.VisitorId = "5039d7e66ee01213b0c8c201";

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = service.UpdateLineQuantity(request);

            Assert.IsNotNull(shoppingCart);
        }

        #endregion

        [TestMethod]
        public void InsertInvoices()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                                      CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "InvoiceList"));

            CompanyGroup.Data.PartnerModule.InvoiceRepository repository = new CompanyGroup.Data.PartnerModule.InvoiceRepository(settings);

            CompanyGroup.Data.MaintainModule.InvoiceRepository maintainRepository = new CompanyGroup.Data.MaintainModule.InvoiceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.ApplicationServices.MaintainModule.IInvoiceService service = new CompanyGroup.ApplicationServices.MaintainModule.InvoiceService(maintainRepository, repository);

            bool result = service.FillCache(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateIndexes()
        {
            CompanyGroup.Data.NoSql.ISettings settings = new CompanyGroup.Data.NoSql.Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "srv1.hrp.hu"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                                                              CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollectionName", "InvoiceList"));

            CompanyGroup.Data.PartnerModule.InvoiceRepository repository = new CompanyGroup.Data.PartnerModule.InvoiceRepository(settings);

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(settings);

            CompanyGroup.ApplicationServices.PartnerModule.IInvoiceService service = new CompanyGroup.ApplicationServices.PartnerModule.InvoiceService(repository, financeRepository, visitorRepository);

            service.CreateIndexes();
        }

    }
}
