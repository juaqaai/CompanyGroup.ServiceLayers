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
            CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request = new CompanyGroup.Dto.WebshopModule.GetAllStructureRequest() { DiscountFilter = false, SecondhandFilter = false, BscFilter = false, Category1IdList = new List<string>(), Category2IdList = new List<string>(), Category3IdList = new List<string>(), HrpFilter = false, IsInNewsletterFilter = false, ManufacturerIdList = new List<string>(), NewFilter = false, StockFilter = false, TextFilter = "" };

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
            CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request = new CompanyGroup.Dto.WebshopModule.GetAllStructureRequest() { DiscountFilter = false, SecondhandFilter = false, BscFilter = false, Category1IdList = new List<string>(), Category2IdList = new List<string>(), Category3IdList = new List<string>(), HrpFilter = false, IsInNewsletterFilter = false, ManufacturerIdList = new List<string>(), NewFilter = false, StockFilter = false, TextFilter = "" };

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
            CompanyGroup.Dto.WebshopModule.GetAllProductRequest request = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest()
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

            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

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
            CompanyGroup.Dto.WebshopModule.GetPriceListRequest request = new CompanyGroup.Dto.WebshopModule.GetPriceListRequest()
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

            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Dto.WebshopModule.PriceList priceList = new CompanyGroup.ApplicationServices.WebshopModule.ProductService(productRepository, shoppingCartRepository, financeRepository, visitorRepository).GetPriceList(request);

            Assert.IsNotNull(priceList);

            Assert.IsTrue(priceList.Items.Count > 0);
        }

        [TestMethod]
        public void GetProductByProductIdRest()
        {
            //http://localhost/CompanyGroup.ServicesHost/ProductService.svc/GetItemByProductId

            CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request = new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest() { ProductId = "PGI7BK", DataAreaId = UnitTest.DataAreaId };

            CompanyGroup.Dto.WebshopModule.ProductDetails productDetails = this.PostJSonData<CompanyGroup.Dto.WebshopModule.ProductDetails>("ProductService", "GetItemByProductId", request);

            Assert.IsNotNull(productDetails);
        }

        [TestMethod]
        public void GetItemByProductId()
        {
            CompanyGroup.Data.WebshopModule.ProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ProductService productService = new CompanyGroup.ApplicationServices.WebshopModule.ProductService(productRepository, shoppingCartRepository, financeRepository, visitorRepository);

            CompanyGroup.Dto.WebshopModule.Product product = productService.GetItemByProductId(new CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest() { ProductId = "B710dn", DataAreaId = "hrp", VisitorId = "" });

            Assert.IsNotNull(product);
        }

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
            CompanyGroup.Dto.PartnerModel.PictureFilterRequest request = new CompanyGroup.Dto.PartnerModel.PictureFilterRequest("hrp", "PFI702GY");

            CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository = new CompanyGroup.Data.WebshopModule.PictureRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

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
            CompanyGroup.Dto.PartnerModule.SignInRequest request = new Dto.PartnerModule.SignInRequest("hrp", "elektroplaza", "58915891", "");

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.ApplicationServices.PartnerModule.VisitorService service = new CompanyGroup.ApplicationServices.PartnerModule.VisitorService(visitorRepository, customerRepository);

            CompanyGroup.Dto.PartnerModule.Visitor visitor = service.SignIn(request);

            Assert.IsNotNull(visitor);
        }

        [TestMethod]
        public void SignOutTest()
        {
            CompanyGroup.Dto.PartnerModule.SignOutRequest request = new CompanyGroup.Dto.PartnerModule.SignOutRequest("hrp", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.ApplicationServices.PartnerModule.IVisitorService service = new CompanyGroup.ApplicationServices.PartnerModule.VisitorService(visitorRepository, customerRepository);

            service.SignOut(request);
        }

        #endregion

        #region "ShoppingCart test cases"

        [TestMethod()]
        public void AssociateCart()
        { 
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.AssociateCartRequest request = new CompanyGroup.Dto.WebshopModule.AssociateCartRequest("503a0aad6ee01209a42e0c41", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AssociateCart(request);

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddCartTest()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.AddCartRequest request = new CompanyGroup.Dto.WebshopModule.AddCartRequest("hu", "5039d7e66ee01213b0c8c201", "kosar neve");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AddCart(request);

            Assert.IsNotNull(response);
        }

        /// <summary>
        ///A test for AddItem
        ///</summary>
        [TestMethod()]
        public void AddItemTest()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.AddLineRequest request = new CompanyGroup.Dto.WebshopModule.AddLineRequest();

            request.CartId = 1;
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
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest request = new CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest("hu", 1, "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCart cart = service.GetCartByKey(request);

            Assert.IsNotNull(cart);
        }

        [TestMethod()]
        public void GetActiveCartTests()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.GetActiveCartRequest request = new CompanyGroup.Dto.WebshopModule.GetActiveCartRequest("hu", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo cart = service.GetActiveCart(request);

            Assert.IsNotNull(cart);
        }

        /// <summary>
        ///A test for GetCartCollectionByVisitor
        ///</summary>
        [TestMethod()]
        public void GetCartCollectionByVisitorTest()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById("5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.GetCartCollectionByVisitorRequest request = new CompanyGroup.Dto.WebshopModule.GetCartCollectionByVisitorRequest("hu", "5039d7e66ee01213b0c8c201");

            CompanyGroup.Dto.WebshopModule.ShoppingCartCollection sc = service.GetCartCollectionByVisitor(request);

            Assert.IsFalse(sc.Carts.Count == 0);
        }

        /// <summary>
        ///A test for UpdateItemQuantity
        ///</summary>
        [TestMethod()]
        public void UpdateItemQuantityTest()
        {
            CompanyGroup.Domain.WebshopModule.IProductRepository productRepository = new CompanyGroup.Data.WebshopModule.ProductRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.WebshopModule.ShoppingCartRepository shoppingCartRepository = new CompanyGroup.Data.WebshopModule.ShoppingCartRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.VisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.Data.PartnerModule.CustomerRepository customerRepository = new Data.PartnerModule.CustomerRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.PartnerModule.SalesOrderRepository salesOrderRepository = new Data.PartnerModule.SalesOrderRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Data.WebshopModule.FinanceRepository financeRepository = new CompanyGroup.Data.WebshopModule.FinanceRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService service = new CompanyGroup.ApplicationServices.WebshopModule.ShoppingCartService(shoppingCartRepository, productRepository, visitorRepository, customerRepository, salesOrderRepository, financeRepository);

            CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest request = new CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest();

            request.CartId = 1;
            request.Language = "en";
            request.LineId = 2;
            request.Quantity = 5;
            request.VisitorId = "5039d7e66ee01213b0c8c201";

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = service.UpdateLineQuantity(request);

            Assert.IsNotNull(shoppingCart);
        }

        #endregion

        [TestMethod]
        public void GetNewsletterCollection()
        {
            CompanyGroup.Dto.PartnerModel.GetNewsletterCollectionRequest request = new CompanyGroup.Dto.PartnerModel.GetNewsletterCollectionRequest("hu", "5092cfe46ee0121e98101481", "");

            CompanyGroup.Domain.WebshopModule.INewsletterRepository newsletterRepository = new Data.WebshopModule.NewsletterRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession());

            CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository = new CompanyGroup.Data.PartnerModule.VisitorRepository(CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession());

            CompanyGroup.ApplicationServices.WebshopModule.INewsletterService service = new CompanyGroup.ApplicationServices.WebshopModule.NewsletterService(newsletterRepository, visitorRepository);

            CompanyGroup.Dto.WebshopModule.NewsletterCollection collection = service.GetNewsletterCollection(request);

            Assert.IsNotNull(collection);
        }

    }
}
