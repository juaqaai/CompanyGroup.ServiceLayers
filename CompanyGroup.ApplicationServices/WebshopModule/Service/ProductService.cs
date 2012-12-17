using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

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

        private const double CACHE_EXPIRATION_PRODUCTS = 24d;

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
        //public CompanyGroup.Dto.WebshopModule.Products GetAll(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            long count = 0;

            request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
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

            int priceFilterRelation = 0;

            int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

            //cache
            string cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_PRODUCT, dataAreaIdCacheKey);

            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.ActionFilter, cacheKey, "ActionFilter");
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.BargainFilter, cacheKey, "BargainFilter");
            request.Category1IdList.ForEach( x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x ));
            request.Category2IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
            request.Category3IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.Currency), cacheKey, request.Currency);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.CurrentPageIndex);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.IsInNewsletterFilter, cacheKey, "IsInNewsletterFilter");
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.ItemsOnPage);
            request.ManufacturerIdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.NameOrPartNumberFilter), cacheKey, request.NameOrPartNumberFilter);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.NewFilter, cacheKey, "NewFilter");
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilter), cacheKey, request.PriceFilter);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilterRelation), cacheKey, request.PriceFilterRelation);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.Sequence);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.StockFilter, cacheKey, "StockFilter");
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.TextFilter), cacheKey, request.TextFilter);
            cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.VisitorId), cacheKey, request.VisitorId); 

            CompanyGroup.Domain.WebshopModule.Products products = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.Products>(cacheKey);

            if (products == null || !CatalogueCacheEnabled)
            {
                products = productRepository.GetList(dataAreaId,
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
                                                     request.Sequence,
                                                     request.CurrentPageIndex,
                                                     request.ItemsOnPage, ref count);

                if (CatalogueCacheEnabled)
                {
                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.Products>(cacheKey, products, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_PRODUCTS)));
                }
            }

            //ha nincs bejelentkezve, akkor a VisitorId üres
            CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

            //érvényes belépés esetén ár, valutanem, kosár, hírlevél akció kalkulálása 
            if (visitor.IsValidLogin)
            {

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                products.ForEach(x =>
                {
                    decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId);

                    x.CustomerPrice = this.ChangePrice(price, request.Currency);

                    x.IsInNewsletter = false;

                    x.IsInCart = shoppingCartCollection.IsInCart(x.ProductId);

                });
            }

            products.ListCount = count;

            CompanyGroup.Dto.WebshopModule.Products results = new ProductsToProducts().Map(products);

            results.Pager = new PagerToPager().Map(products.Pager, request.ItemsOnPage);

            results.Currency = request.Currency;

            return results;
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
