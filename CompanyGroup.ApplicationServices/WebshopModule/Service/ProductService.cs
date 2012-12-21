using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using CompanyGroup.Helpers;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] 
    public class ProductService : ServiceBase, IProductService
    {
        private const string CACHEKEY_PRODUCT = "product";

        private const double CACHE_EXPIRATION_PRODUCTS = 1d;

        private static readonly bool CatalogueCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("CatalogueCacheEnabled", false);

        private static readonly int ProductComplationLimit = Helpers.ConfigSettingsParser.GetInt("ProductComplationLimit", 15);

        private const string CACHEKEY_STRUCTURE = "structure";

        private const double CACHE_EXPIRATION_STRUCTURE = 24d;

        private static readonly bool StructureCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("StructureCacheEnabled", true);

        private CompanyGroup.Domain.WebshopModule.IProductRepository productRepository;

        private CompanyGroup.Domain.MaintainModule.IProductRepository maintainProductRepository;

        private CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository;

        //private CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository;

        /// <summary>
        /// konstruktor repository interfész paraméterrel
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="maintainProductRepository"></param>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="financeRepository"></param>
        /// <param name="visitorRepository"></param>
        public ProductService(CompanyGroup.Domain.WebshopModule.IProductRepository productRepository,
                              CompanyGroup.Domain.MaintainModule.IProductRepository maintainProductRepository,
                              CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository,
                              CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                              CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentNullException("ProductRepository");
            }

            if (shoppingCartRepository == null)
            {
                throw new ArgumentNullException("ShoppingCartRepository");
            }

            if (maintainProductRepository == null)
            {
                throw new ArgumentNullException("MaintainProductRepository");
            }

            this.productRepository = productRepository;

            this.maintainProductRepository = maintainProductRepository;

            this.shoppingCartRepository = shoppingCartRepository;
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Catalogue GetCatalogue(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            long count = 0;

            request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
            string dataAreaId = ConstructDataAreaId(request);

            string dataAreaIdCacheKey = dataAreaId;

            if (String.IsNullOrEmpty(dataAreaId))
            { 
                dataAreaIdCacheKey = "all";
            }

            int priceFilterRelation = 0;

            int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

            CompanyGroup.Domain.WebshopModule.ProductList productList = null;

            string cacheKey = String.Empty;

            //cache
            if (ProductService.CatalogueCacheEnabled)
            {
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_PRODUCT, dataAreaIdCacheKey);

                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.ActionFilter, cacheKey, "ActionFilter");
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.BargainFilter, cacheKey, "BargainFilter");
                //request.Category1IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                //request.Category2IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                //request.Category3IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.Currency), cacheKey, request.Currency);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.CurrentPageIndex);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.IsInNewsletterFilter, cacheKey, "IsInNewsletterFilter");
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.ItemsOnPage);
                //request.ManufacturerIdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.NameOrPartNumberFilter), cacheKey, request.NameOrPartNumberFilter);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.NewFilter, cacheKey, "NewFilter");
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilter), cacheKey, request.PriceFilter);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilterRelation), cacheKey, request.PriceFilterRelation);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.Sequence);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.StockFilter, cacheKey, "StockFilter");
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.TextFilter), cacheKey, request.TextFilter);
                //cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.VisitorId), cacheKey, request.VisitorId);

                productList = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.ProductList>(CACHEKEY_PRODUCT);
            }

            if (productList == null)
            {
                productList = productRepository.GetProductList();

                if (ProductService.CatalogueCacheEnabled)
                {
                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.ProductList>(cacheKey, productList, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_PRODUCTS)));
                }
            }

            IQueryable<CompanyGroup.Domain.WebshopModule.Product> filteredQueryableList = productList.AsQueryable().Where(ConstructPredicate(request));

            IQueryable<CompanyGroup.Domain.WebshopModule.Product> filteredQueryableBannerProductList = productList.AsQueryable().Where(ConstructBannerListPredicate(request)).OrderByDescending( x => x.AverageInventory);

            List<CompanyGroup.Domain.WebshopModule.Product> filteredList = filteredQueryableList.ToList();

            List<CompanyGroup.Domain.WebshopModule.Product> filteredBannerProductList = filteredQueryableBannerProductList.ToList();

            List<CompanyGroup.Dto.WebshopModule.BannerProduct> bannerProducts = new List<CompanyGroup.Dto.WebshopModule.BannerProduct>();

            bannerProducts.AddRange(filteredBannerProductList.ConvertAll( x => {
            
                return new CompanyGroup.Dto.WebshopModule.BannerProduct() { Currency = "", 
                                                                            DataAreaId = x.DataAreaId, 
                                                                            ItemName = x.ProductName, 
                                                                            ItemNameEnglish = x.ProductNameEnglish, 
                                                                            PartNumber = x.PartNumber, 
                                                                            Price = String.Format( "{0}", x.Prices.Price2), 
                                                                            PrimaryPicture = new PictureToPicture().Map(x.PrimaryPicture), 
                                                                            ProductId = x.ProductId };
            
            }));

            //ha nincs bejelentkezve, akkor a VisitorId üres
            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            //érvényes belépés esetén ár, valutanem, kosár, hírlevél akció kalkulálása 
            if (visitor.IsValidLogin)
            {

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                filteredList.ForEach(x =>
                {
                    decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId);

                    x.CustomerPrice = this.ChangePrice(price, request.Currency);

                    x.IsInNewsletter = false;

                    x.IsInCart = shoppingCartCollection.IsInCart(x.ProductId);

                });
            }

            IQueryable<CompanyGroup.Domain.WebshopModule.Product> orderedList = filteredQueryableList.OrderByDescending(x => x.AverageInventory).Skip((request.CurrentPageIndex - 1) * request.ItemsOnPage).Take(request.ItemsOnPage);

            CompanyGroup.Domain.WebshopModule.Products products = new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(request.CurrentPageIndex, filteredList.Count(), request.ItemsOnPage));

            products.AddRange(orderedList);

            products.ListCount = filteredList.Count();

            CompanyGroup.Dto.WebshopModule.Products p = new ProductsToProducts().Map(products);

            p.Pager = new PagerToPager().Map(products.Pager, request.ItemsOnPage);

            p.Currency = request.Currency;

            CompanyGroup.Domain.WebshopModule.Structures structures = new CompanyGroup.Domain.WebshopModule.Structures();

            List<CompanyGroup.Domain.WebshopModule.Structure> structureList = filteredList.ConvertAll(x => new CompanyGroup.Domain.WebshopModule.Structure() { Manufacturer = x.Structure.Manufacturer, Category1 = x.Structure.Category1, Category2 = x.Structure.Category2, Category3 = x.Structure.Category3 });

            structures.AddRange(structureList);

            CompanyGroup.Dto.WebshopModule.Structures s = new StructuresToStructures().Map(request.ManufacturerIdList, request.Category1IdList, request.Category2IdList, request.Category3IdList, structures);

            return new CompanyGroup.Dto.WebshopModule.Catalogue(p, s, bannerProducts);
        }

        private static string ConstructDataAreaId(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
            string dataAreaId = String.Empty;

            if (request.HrpFilter && !request.BscFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
            }
            else if (request.BscFilter && !request.HrpFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
            }
            
            return dataAreaId;
        }

        private static System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> ConstructPredicate(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {

            System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> defaultPredicate = CompanyGroup.Helpers.PredicateBuilder.False<CompanyGroup.Domain.WebshopModule.Product>();

            System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> predicate = CompanyGroup.Helpers.PredicateBuilder.True<CompanyGroup.Domain.WebshopModule.Product>();
            //{ "$or" : [{ "DataAreaId" : { "$ne" : "ser" }, "ItemState" : { "$lt" : 2 } }, { "DataAreaId" : { "$ne" : "ser" }, "$where" : { "$code" : "this.SecondHandList.length > 0" } }] }
            //query = { "DataAreaId" : { "$ne" : "ser" }, "$or" : [{ "ItemState" : { "$lt" : 2 } }, { "$where" : { "$code" : "SecondHandList.length > 0" } }] }

            defaultPredicate = defaultPredicate.Or(p => p.ItemState.Equals(ItemState.Active));

            defaultPredicate = defaultPredicate.Or(p => p.ItemState.Equals(ItemState.EndOfSales));

            defaultPredicate = defaultPredicate.Or(p => p.SecondHandList.Count > 0);

            //IEnumerable<CompanyGroup.Domain.WebshopModule.Product> resultList = productList.Where(x => { return (x.ItemState.Equals(ItemState.Active) || x.ItemState.Equals(ItemState.EndOfSales)) || (x.SecondHandList.Count > 0); });

            //MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.LT("ItemState", 2), MongoDB.Driver.Builders.Query.Where("this.SecondHandList.length > 0"));

            string dataAreaId = ConstructDataAreaId(request);

            if (!String.IsNullOrEmpty(dataAreaId))
            {
                predicate = predicate.And(p => p.DataAreaId.Equals(dataAreaId));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId));
            }

            if (request.ManufacturerIdList.Count > 0)
            {
                predicate = predicate.And(p => request.ManufacturerIdList.Contains(p.Structure.Manufacturer.ManufacturerId));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.In("Structure.Manufacturer.ManufacturerId", MongoDB.Bson.BsonArray.Create(manufacturerIdList)));
            }
            if (request.Category1IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category1IdList.Contains(p.Structure.Category1.CategoryId));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.In("Structure.Category1.CategoryId", MongoDB.Bson.BsonArray.Create(category1IdList)));
            }
            if (request.Category2IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category2IdList.Contains(p.Structure.Category2.CategoryId));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.In("Structure.Category2.CategoryId", MongoDB.Bson.BsonArray.Create(category2IdList)));
            }
            if (request.Category3IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category3IdList.Contains(p.Structure.Category3.CategoryId));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.In("Structure.Category3.CategoryId", MongoDB.Bson.BsonArray.Create(category3IdList)));
            }
            if (request.ActionFilter)
            {
                predicate = predicate.And(p => p.Discount.Equals(request.ActionFilter));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.EQ("Discount", actionFilter));
            }
            if (request.IsInNewsletterFilter)
            {
                predicate = predicate.And(p => p.IsInNewsletter.Equals(request.IsInNewsletterFilter));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.EQ("IsInNewsletter", isInNewsletterFilter));
            }
            if (request.NewFilter)
            {
                predicate = predicate.And(p => p.New.Equals(request.NewFilter));
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.EQ("New", newFilter));
            }
            if (request.StockFilter)
            {
                predicate = predicate.And(p => p.Stock.Inner > 0);
                predicate = predicate.Or(p => p.Stock.Outer > 0);
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.GT("Stock.Inner", 0), MongoDB.Driver.Builders.Query.GT("Stock.Outer", 0)));
            }

            if (!String.IsNullOrEmpty(request.TextFilter))
            {
                predicate = predicate.And(p => request.TextFilter.Contains(p.ProductName));

                //MongoDB.Bson.BsonRegularExpression regex = new MongoDB.Bson.BsonRegularExpression(String.Format(".*{0}.*", textFilter), "i");

                //MongoDB.Bson.BsonRegularExpression regex = MongoDB.Bson.BsonRegularExpression.Create(new System.Text.RegularExpressions.Regex(textFilter, System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ProductName", regex), MongoDB.Driver.Builders.Query.Matches("ProductNameEnglish", regex),
                //                                                 MongoDB.Driver.Builders.Query.Matches("Description", regex), MongoDB.Driver.Builders.Query.Matches("DescriptionEnglish", regex)) );
            }

            if (!String.IsNullOrEmpty(request.NameOrPartNumberFilter))
            {
                predicate = predicate.And(p => request.NameOrPartNumberFilter.Contains(p.ProductName));

                //MongoDB.Bson.BsonRegularExpression regex = new MongoDB.Bson.BsonRegularExpression(String.Format(".*{0}.*", nameOrPartNumberFilter), "i");

                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ProductName", regex), MongoDB.Driver.Builders.Query.Matches("ProductNameEnglish", regex),
                //                                                 MongoDB.Driver.Builders.Query.Matches("PartNumber", regex), MongoDB.Driver.Builders.Query.Matches("ProductId", regex)) );
            }

            if (request.PriceFilterRelation.Equals(1))
            {
                int price;

                predicate = predicate.And(p => p.Prices.Price2 >= ((Int32.TryParse(request.PriceFilter, out price)) ? price : 0));

                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.GTE("Prices.Price2", MongoDB.Bson.BsonInt32.Create(priceFilter)));
            }
            else if (request.PriceFilterRelation.Equals(2))
            {
                int price;

                predicate = predicate.And(p => p.Prices.Price2 <= ((Int32.TryParse(request.PriceFilter, out price)) ? price : 0));

                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.LTE("Prices.Price2", MongoDB.Bson.BsonInt32.Create(priceFilter)));
            }
            if (request.BargainFilter)
            {
                predicate = predicate.And(p => p.SecondHandList.Count > 0);

                //query = MongoDB.Driver.Builders.Query.LT("ItemState", 3);
                //query = MongoDB.Driver.Builders.Query.And(query, MongoDB.Driver.Builders.Query.Where("this.SecondHandList.length > 0"));
            }

            //query = { "DataAreaId" : { "$ne" : "ser" }, "$or" : [{ "ItemState" : { "$lt" : 2 } }, { "$where" : { "$code" : "this.SecondHandList.length > 0" } }] }

            return predicate.And(defaultPredicate);
        }

        private static System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> ConstructBannerListPredicate(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> defaultPredicate = CompanyGroup.Helpers.PredicateBuilder.False<CompanyGroup.Domain.WebshopModule.Product>();

            System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> predicate = CompanyGroup.Helpers.PredicateBuilder.True<CompanyGroup.Domain.WebshopModule.Product>();

            defaultPredicate = defaultPredicate.Or(p => p.ItemState.Equals(ItemState.Active));

            defaultPredicate = defaultPredicate.Or(p => p.ItemState.Equals(ItemState.EndOfSales));

            defaultPredicate = defaultPredicate.Or(p => p.SecondHandList.Count > 0);

            string dataAreaId = ConstructDataAreaId(request);

            if (!String.IsNullOrEmpty(dataAreaId))
            {
                predicate = predicate.And(p => p.DataAreaId.Equals(dataAreaId));
            }

            predicate = predicate.And(p => p.Stock.Inner > 0);
            predicate = predicate.Or(p => p.Stock.Outer > 0);

            predicate = predicate.And(p => p.Discount.Equals(request.ActionFilter));

            predicate = predicate.And(p => p.Pictures.Count > 0);

            return predicate.And(defaultPredicate);       
        }

        /// <summary>
        /// struktúrák lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Structures GetStructure(CompanyGroup.Dto.ServiceRequest.GetAllStructure request)
        {
            string dataAreaId = String.Empty;

            string dataAreaIdCacheKey = String.Empty;

            if (request.HrpFilter && !request.BscFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;

                dataAreaIdCacheKey = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
            }
            else if (request.BscFilter && !request.HrpFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;

                dataAreaIdCacheKey = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
            }
            else
            {
                dataAreaIdCacheKey = "all";
            }

            //szűrés ár értékre
            int priceFilterRelation = 0;

            int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

            CompanyGroup.Domain.WebshopModule.Structures structures = null;

            string cacheKey = String.Empty;

            //cache kiolvasás
            if (ProductService.StructureCacheEnabled)
            {
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_STRUCTURE, dataAreaIdCacheKey);

                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.ActionFilter, cacheKey, "ActionFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.BargainFilter, cacheKey, "BargainFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.IsInNewsletterFilter, cacheKey, "IsInNewsletterFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.NewFilter, cacheKey, "NewFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.StockFilter, cacheKey, "StockFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.TextFilter), cacheKey, request.TextFilter);
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilter), cacheKey, request.PriceFilter);
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilterRelation), cacheKey, request.PriceFilterRelation);
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.NameOrPartNumberFilter), cacheKey, request.NameOrPartNumberFilter);

                structures = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.Structures>(cacheKey);
            }

            //vagy nem engedélyezett a cache, vagy nem volt a cache-ben
            if (structures == null)
            {
                structures = productRepository.GetStructure(dataAreaId, request.ActionFilter, request.BargainFilter, request.IsInNewsletterFilter,
                                                            request.NewFilter, request.StockFilter, request.TextFilter, request.PriceFilter, priceFilterRelation,
                                                            request.NameOrPartNumberFilter);

                //cache-be mentés
                if (ProductService.StructureCacheEnabled)
                {
                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.Structures>(cacheKey, structures, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_STRUCTURE)));
                }
            }

            CompanyGroup.Dto.WebshopModule.Structures result = new StructuresToStructures().Map(request.ManufacturerIdList, request.Category1IdList, request.Category2IdList, request.Category3IdList, structures);

            return result;
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// 1. repository-tól elkéri az akciós, készleten lévő, képpel rendelkező legfeljebb 150 elemet tartalmazó terméklistát.
        /// 2. ha a hívóparaméter tartalmaz jelleg1, jelleg2, jelleg3 paramétert, akkor a paraméterek szerint szűri a listát, majd a találati eredményből visszaadja az első 50 elemet
        /// 3. ha a lekérdezésnek nincs eredménye, akkor az 1. pont szerinti lekérdezés eredményéből visszaadja az első 50 elemet
        /// 
        /// ha bejelentkezett felhasználó kérte le a listát, akkor az eredményhalmazban az ár is kitöltésre kerül, egyébként az ár értéke nulla.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.ServiceRequest.GetBannerList request)
        {
            int responseItemCount = 50;

            CompanyGroup.Dto.WebshopModule.BannerList result = new CompanyGroup.Dto.WebshopModule.BannerList();

            request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
            string dataAreaId = String.Empty;

            if (request.HrpFilter && request.BscFilter)
            {
                dataAreaId = String.Empty;
            }
            else if (request.HrpFilter && !request.BscFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
            }
            else if (request.BscFilter && !request.HrpFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
            }

            CompanyGroup.Domain.WebshopModule.Products products = productRepository.GetBannerList(dataAreaId, 300);

            List<CompanyGroup.Domain.WebshopModule.Product> filteredProducts = new List<CompanyGroup.Domain.WebshopModule.Product>();

            if (request.Category1IdList.Count > 0)
            {
                filteredProducts.AddRange(products.Where(x => request.Category1IdList.Contains(x.Structure.Category1.CategoryId)).Take(responseItemCount).ToList());
            }

            if (request.Category2IdList.Count > 0)
            {
                filteredProducts.AddRange(products.Where(x => request.Category2IdList.Contains(x.Structure.Category2.CategoryId)).Take(responseItemCount).ToList());
            }

            if (request.Category3IdList.Count > 0)
            {
                filteredProducts.AddRange(products.Where(x => request.Category3IdList.Contains(x.Structure.Category3.CategoryId)).Take(responseItemCount).ToList());
            }

            if (filteredProducts.Count.Equals(0))
            {
                filteredProducts.AddRange(products.Take(responseItemCount));
            }

            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            if (visitor.IsValidLogin)
            {

                filteredProducts.ForEach(x =>
                {
                    decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId);

                    x.CustomerPrice = this.ChangePrice(price, request.Currency);

                    x.IsInNewsletter = false;

                    x.IsInCart = false;

                });
            }

            CompanyGroup.Dto.WebshopModule.BannerList results = new ProductsToBannerList().Map(filteredProducts);

            return results;
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.ServiceRequest.GetPriceList request)
        {
            request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
            string dataAreaId = String.Empty;

            if (request.HrpFilter && request.BscFilter)
            {
                dataAreaId = String.Empty;
            }
            else if (request.HrpFilter && !request.BscFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
            }
            else if (request.BscFilter && !request.HrpFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
            }

            int priceFilterRelation = 0;

            int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

            CompanyGroup.Domain.WebshopModule.PriceList priceList = productRepository.GetPriceList(dataAreaId,
                                                                                                   request.ManufacturerIdList,
                                                                                                   request.Category1IdList,
                                                                                                   request.Category2IdList,
                                                                                                   request.Category3IdList,
                                                                                                   request.ActionFilter,
                                                                                                   request.BargainFilter,
                                                                                                   request.IsInNewsletterFilter,
                                                                                                   request.NewFilter,
                                                                                                   request.StockFilter,
                                                                                                   request.TextFilter,
                                                                                                   request.PriceFilter,
                                                                                                   priceFilterRelation,
                                                                                                   request.NameOrPartNumberFilter,
                                                                                                   request.Sequence);

            //ha nincs bejelentkezve, akkor a VisitorId üres
            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            //érvényes belépés esetén ár, hírlevél akció kalkulálása 
            if (visitor.IsValidLogin)
            {
                priceList.ForEach(x =>
                {
                    decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId);

                    x.CustomerPrice = this.ChangePrice(price, request.Currency);

                    x.IsInNewsletter = false;
                });
            }

            CompanyGroup.Dto.WebshopModule.PriceList results = new PriceListToPriceList().Map(priceList);

            return results;
        }

        /// <summary>
        /// FROMDATE	            CURRENCYCODE	EXCHRATEMNB	        DATAAREAID
        /// 2012-07-26 00:00:00.000	EUR	            28753.000000000000	mst
        /// 2012-07-26 00:00:00.000	GBP	            36715.000000000000	mst
        /// 2012-07-26 00:00:00.000	RSD	            242.000000000000	mst
        /// 2012-07-26 00:00:00.000	USD	            23716.000000000000	mst
        /// 2012-07-26 00:00:00.000	EUR	            11855.390000000000	ser
        /// </summary>
        /// <param name="prices">pénz értéke, amit váltani kell FT-ben megadva</param>
        /// <param name="currency">pénznem, amire váltani kell</param>
        /// <returns></returns>
        //private decimal ChangePrice(decimal price, string currency)
        //{
        //    if (CompanyGroup.Domain.Core.Constants.CurrencyHuf.Equals(currency) || String.IsNullOrEmpty(currency))
        //    {
        //        return price;
        //    }

        //    try
        //    {
        //        List<CompanyGroup.Domain.WebshopModule.ExchangeRate> exchangeRates = financeRepository.GetCurrentRates();

        //        CompanyGroup.Domain.WebshopModule.ExchangeRate exchangeRate = exchangeRates.Find(x => x.CurrencyCode.Equals(currency));

        //        if (exchangeRate == null)
        //        {
        //            return price;
        //        }

        //        //Pl.: (ennyi Ft 1 EUR) * price
        //        decimal value = (exchangeRate.Rate / 100);

        //        return (price / value);
        //    }
        //    catch { return price; }
        //}

        /// <summary>
        /// objectId egyedi kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Product GetItemByObjectId(string objectId, string visitorId)
        {
            CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(objectId);

            if (product == null) { return new CompanyGroup.Dto.WebshopModule.Product(); }

            //ha nincs bejelentkezve, akkor a VisitorId üres
            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(visitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(visitorId);

            if (visitor.IsValidLogin)
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollectionByVisitor(visitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                product.CustomerPrice = visitor.CalculateCustomerPrice(product.Prices.Price1, 
                                                                       product.Prices.Price2, 
                                                                       product.Prices.Price3, 
                                                                       product.Prices.Price4, 
                                                                       product.Prices.Price5, 
                                                                       product.Structure.Manufacturer.ManufacturerId, 
                                                                       product.Structure.Category1.CategoryId, 
                                                                       product.Structure.Category2.CategoryId, 
                                                                       product.Structure.Category3.CategoryId);
                //product.CustomerPrice = this.ChangePrice(price, request.Currency);

                product.IsInNewsletter = false;

                product.IsInCart = shoppingCartCollection.IsInCart(product.ProductId);
            }
            else
            {
                product.CustomerPrice = 0;

                product.IsInNewsletter = false;

                product.IsInCart = false;
            }

            CompanyGroup.Dto.WebshopModule.Product result = new ProductToProduct().Map(product);

            return result;
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(request.ProductId, request.DataAreaId);

            if (product == null) { return new CompanyGroup.Dto.WebshopModule.Product(); }

            //ha nincs bejelentkezve, akkor a VisitorId üres
            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            if (visitor.IsValidLogin)
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                decimal price = visitor.CalculateCustomerPrice(product.Prices.Price1,
                                                               product.Prices.Price2,
                                                               product.Prices.Price3,
                                                               product.Prices.Price4,
                                                               product.Prices.Price5,
                                                               product.Structure.Manufacturer.ManufacturerId,
                                                               product.Structure.Category1.CategoryId,
                                                               product.Structure.Category2.CategoryId,
                                                               product.Structure.Category3.CategoryId);

                product.CustomerPrice = this.ChangePrice(price, request.Currency);

                product.IsInNewsletter = false;

                product.IsInCart = shoppingCartCollection.IsInCart(product.ProductId);
            }
            else
            {
                product.CustomerPrice = 0;

                product.IsInNewsletter = false;

                product.IsInCart = false;
            }

            CompanyGroup.Dto.WebshopModule.Product result = new ProductToProduct().Map(product);

            return result;
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="prefix"></param>
        /// <param name="completionType">0: nincs megadva, 1: termékazonosító-cikkszám, 2: minden</param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(string dataAreaId, string prefix, string completionType) //CompanyGroup.Dto.ServiceRequest.ProductListComplation request
        {
            if (String.IsNullOrWhiteSpace(dataAreaId) || String.IsNullOrWhiteSpace(prefix))
            {
                return new Dto.WebshopModule.CompletionList();
            }

            CompanyGroup.Domain.WebshopModule.CompletionType completion = (CompanyGroup.Domain.WebshopModule.CompletionType) Enum.Parse(typeof(CompanyGroup.Domain.WebshopModule.CompletionType), completionType);

            CompanyGroup.Domain.WebshopModule.CompletionList result = productRepository.GetComplationList(dataAreaId, prefix, ProductComplationLimit, completion);

            return new CompletionToCompletion().Map(result);
        }

        #region "Termék kompatibilitás"

        public CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            CompanyGroup.Dto.WebshopModule.CompatibleProducts result = new CompanyGroup.Dto.WebshopModule.CompatibleProducts();

            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            result.Items = this.GetCompatibilityList(request.ProductId, request.DataAreaId, request.VisitorId, visitor);

            result.ReverseItems = this.GetReverseCompatibilityList(request.ProductId, request.DataAreaId, request.VisitorId, visitor);

            return result;
        }

        private List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> GetCompatibilityList(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> result = new List<CompanyGroup.Dto.WebshopModule.CompatibleProduct>();

            List<CompanyGroup.Domain.MaintainModule.CompatibilityItem> compatibilityItems = maintainProductRepository.GetCompatibilityItemList(productId, dataAreaId, false);

            compatibilityItems.ForEach(x => result.Add(this.GetCompatibleProduct(x.ItemId, x.DataAreaId, visitorId, visitor)));

            return result;
        }

        private List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> GetReverseCompatibilityList(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> result = new List<CompanyGroup.Dto.WebshopModule.CompatibleProduct>();

            List<CompanyGroup.Domain.MaintainModule.CompatibilityItem> compatibilityItems = maintainProductRepository.GetCompatibilityItemList(productId, dataAreaId, true);

            compatibilityItems.ForEach(x => result.Add(this.GetCompatibleProduct(x.ItemId, x.DataAreaId, visitorId, visitor)));

            return result;
        }

        private CompanyGroup.Dto.WebshopModule.CompatibleProduct GetCompatibleProduct(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(productId, dataAreaId);

            if (product == null) { return new CompanyGroup.Dto.WebshopModule.CompatibleProduct(); }

            if (visitor.IsValidLogin)
            {
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollectionByVisitor(visitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                product.CustomerPrice = visitor.CalculateCustomerPrice(product.Prices.Price1,
                                                                       product.Prices.Price2,
                                                                       product.Prices.Price3,
                                                                       product.Prices.Price4,
                                                                       product.Prices.Price5,
                                                                       product.Structure.Manufacturer.ManufacturerId,
                                                                       product.Structure.Category1.CategoryId,
                                                                       product.Structure.Category2.CategoryId,
                                                                       product.Structure.Category3.CategoryId);
                //product.CustomerPrice = this.ChangePrice(price, request.Currency);

                product.IsInNewsletter = false;

                product.IsInCart = shoppingCartCollection.IsInCart(product.ProductId);
            }
            else
            {
                product.CustomerPrice = 0;

                product.IsInNewsletter = false;

                product.IsInCart = false;
            }

            CompanyGroup.Dto.WebshopModule.CompatibleProduct result = new ProductToCompatibleProduct().Map(product);

            return result;
        }

        #endregion
    }
}
