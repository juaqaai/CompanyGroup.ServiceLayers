﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using CompanyGroup.Helpers;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// termékek szolgáltatás
    /// </summary>
    public class ProductService : ServiceBase, IProductService
    {
        private const string CACHEKEY_PRODUCT = "product";

        private const string CACHEKEY_SECONDHAND = "secondhand";

        private const double CACHE_EXPIRATION_PRODUCTS = 1d;

        private const double CACHE_EXPIRATION_SECONDHAND = 1d;

        private static readonly bool CatalogueCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("CatalogueCacheEnabled", false);

        private static readonly int ProductComplationLimit = Helpers.ConfigSettingsParser.GetInt("ProductComplationLimit", 15);

        private const string CACHEKEY_STRUCTURE = "structure";

        private const double CACHE_EXPIRATION_STRUCTURE = 24d;

        private static readonly bool StructureCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("StructureCacheEnabled", true);

        private CompanyGroup.Domain.WebshopModule.IProductRepository productRepository;

        private CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository;

        private CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository;

        private CompanyGroup.Domain.WebshopModule.IChangeTrackingRepository changeTrackingRepository;

        /// <summary>
        /// konstruktor repository interfész paraméterrel
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="pictureRepository"></param>
        /// <param name="changeTrackingRepository"></param>
        /// <param name="financeRepository"></param>
        /// <param name="visitorRepository"></param>
        public ProductService(CompanyGroup.Domain.WebshopModule.IProductRepository productRepository,
                              CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository,
                              CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository,  
                              CompanyGroup.Domain.WebshopModule.IChangeTrackingRepository changeTrackingRepository, 
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

            if (pictureRepository == null)
            {
                throw new ArgumentNullException("PictureRepository");
            }

            if (changeTrackingRepository == null)
            {
                throw new ArgumentNullException("PictureRepository");
            }

            this.productRepository = productRepository;

            this.shoppingCartRepository = shoppingCartRepository;

            this.pictureRepository = pictureRepository;

            this.changeTrackingRepository = changeTrackingRepository;
        }

        private string CreateTextFilterCondition(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            System.Text.StringBuilder textFilterCondition = new System.Text.StringBuilder();

            string[] textFilterList = System.Text.RegularExpressions.Regex.Split(value, @"\s+");

            for (int index = 0; index < textFilterList.Length; index++)
            {
                textFilterCondition.Append("\"");

                textFilterCondition.Append(textFilterList[index]);

                textFilterCondition.Append("*");

                textFilterCondition.Append("\"");

                if (index < textFilterList.Length - 1)
                {
                    textFilterCondition.Append(" AND ");
                }
            }

            return textFilterCondition.ToString();
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Products GetProducts(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request)
        {
            try
            {
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

                //szöveges kereső paraméter FULL TEXT SEARCH paraméterré alakítása "ASUS*" AND "számítógép*"
                string textFilter = CreateTextFilterCondition(request.TextFilter);

                CompanyGroup.Domain.WebshopModule.InventSumList inventSumList = changeTrackingRepository.InventSumCT(0);

                //termékazonosítók listája, melyek nem kell hogy szerepeljenek a terméklista lekérdezés eredményében
                List<string> excludedItems = inventSumList.NotValidItemIdList().ToList();

                //lekérdező paraméterek alapján visszaadott elemek száma
                long count = productRepository.GetListCount(dataAreaId,
                                                             ConvertData.ConvertStringListToDelimitedString(request.ManufacturerIdList),
                                                             ConvertData.ConvertStringListToDelimitedString(request.Category1IdList),
                                                             ConvertData.ConvertStringListToDelimitedString(request.Category2IdList),
                                                             ConvertData.ConvertStringListToDelimitedString(request.Category3IdList),
                                                             request.DiscountFilter,
                                                             request.SecondhandFilter,
                                                             request.IsInNewsletterFilter,
                                                             request.NewFilter,
                                                             request.StockFilter,
                                                             request.Sequence,
                                                             textFilter,
                                                             request.PriceFilter,
                                                             priceFilterRelation,
                                                             ConvertData.ConvertStringListToDelimitedString(excludedItems));

                CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.Products products = null;

                string cacheKey = String.Empty;

                //cache
                if (ProductService.CatalogueCacheEnabled)
                {
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_PRODUCT, dataAreaIdCacheKey);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.DiscountFilter, cacheKey, "DiscountFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.SecondhandFilter, cacheKey, "SecondhandFilter");
                    request.Category1IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                    request.Category2IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                    request.Category3IdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.Currency), cacheKey, request.Currency);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.CurrentPageIndex);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.IsInNewsletterFilter, cacheKey, "IsInNewsletterFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.ItemsOnPage);
                    request.ManufacturerIdList.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.NewFilter, cacheKey, "NewFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilter), cacheKey, request.PriceFilter);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilterRelation), cacheKey, request.PriceFilterRelation);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(cacheKey, request.Sequence);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.StockFilter, cacheKey, "StockFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.TextFilter), cacheKey, request.TextFilter);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.VisitorId), cacheKey, request.VisitorId);
                    excludedItems.ForEach(x => cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(x), cacheKey, x));
                    products = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.Products>(CACHEKEY_PRODUCT);
                }

                if (products == null)
                {
                    products = productRepository.GetList(visitor.Id,
                                                         dataAreaId,
                                                         ConvertData.ConvertStringListToDelimitedString(request.ManufacturerIdList),
                                                         ConvertData.ConvertStringListToDelimitedString(request.Category1IdList),
                                                         ConvertData.ConvertStringListToDelimitedString(request.Category2IdList),
                                                         ConvertData.ConvertStringListToDelimitedString(request.Category3IdList),
                                                         request.DiscountFilter,
                                                         request.SecondhandFilter,
                                                         request.IsInNewsletterFilter,
                                                         request.NewFilter,
                                                         request.StockFilter,
                                                         request.Sequence,
                                                         textFilter,
                                                         request.PriceFilter,
                                                         priceFilterRelation,
                                                         request.CurrentPageIndex,
                                                         request.ItemsOnPage,
                                                         count,
                                                         ConvertData.ConvertStringListToDelimitedString(excludedItems));

                    if (ProductService.CatalogueCacheEnabled)
                    {
                        CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.Products>(cacheKey, products, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_PRODUCTS)));
                    }
                }

                //IQueryable<CompanyGroup.Domain.WebshopModule.Product> filteredQueryableList = productList.AsQueryable().Where(ConstructPredicate(request));

                //IQueryable<CompanyGroup.Domain.WebshopModule.Product> filteredQueryableBannerProductList = productList.AsQueryable().Where(ConstructBannerListPredicate(request)).OrderByDescending( x => x.AverageInventory);

                //List<CompanyGroup.Domain.WebshopModule.Product> filteredList = filteredQueryableList.ToList();

                //List<CompanyGroup.Domain.WebshopModule.Product> filteredBannerProductList = filteredQueryableBannerProductList.ToList();

                //List<CompanyGroup.Dto.WebshopModule.BannerProduct> bannerProducts = new List<CompanyGroup.Dto.WebshopModule.BannerProduct>();

                //bannerProducts.AddRange(filteredBannerProductList.ConvertAll( x => {

                //    return new CompanyGroup.Dto.WebshopModule.BannerProduct() { Currency = "", 
                //                                                                DataAreaId = x.DataAreaId, 
                //                                                                ItemName = x.ProductName, 
                //                                                                ItemNameEnglish = x.ProductNameEnglish, 
                //                                                                PartNumber = x.PartNumber, 
                //                                                                Price = String.Format( "{0}", x.Prices.Price2), 
                //                                                                PrimaryPicture = new PictureToPicture().Map(x.PrimaryPicture), 
                //                                                                ProductId = x.ProductId };

                //}));

                //ha nincs bejelentkezve, akkor a VisitorId üres

                //érvényes belépés esetén ár, valutanem, kosár, hírlevél akció kalkulálása 
                if (visitor.IsValidLogin)
                {

                    List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollection(request.VisitorId);

                    CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                    //legutolsó frissítés óta az árazásban történt változások
                    CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList = changeTrackingRepository.PriceDiscTableCT(0);

                    products.ForEach(x =>
                    {
                        decimal price = 0; int price1 = 0; int price2 = 0; int price3 = 0; int price4 = 0; int price5 = 0;

                        //vevőkódhoz rendelt egyedi ár beállítás CT-ben ha van, akkor kiolvasásra kerül
                        if (priceDiscTableList.IsInList(x.ProductId, x.DataAreaId, visitor.CustomerId))
                        {
                            price = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, visitor.CustomerId);
                        }

                        //ha nincs egyedi beállítás a termékre, akkor lehet 1..5 intervallum érték változás a CT-ben, 
                        if (price.Equals(0))
                        {
                            price1 = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, "1");
                            price2 = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, "2");
                            price3 = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, "3");
                            price4 = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, "4");
                            price5 = priceDiscTableList.GetPrice(x.ProductId, x.DataAreaId, "5");

                            //ha van 1..5 intervallum érték változás a CT-ben, akkor ezzel kell számolni
                            if (!price1.Equals(0) || !price2.Equals(0) || !price3.Equals(0) || !price4.Equals(0) || !price5.Equals(0))
                            {
                                if (price1.Equals(0)) { price1 = x.Prices.Price1; }
                                if (price2.Equals(0)) { price2 = x.Prices.Price2; }
                                if (price3.Equals(0)) { price3 = x.Prices.Price3; }
                                if (price4.Equals(0)) { price4 = x.Prices.Price4; }
                                if (price5.Equals(0)) { price5 = x.Prices.Price5; }

                                price = visitor.CalculateCustomerPrice(price1, price2, price3, price4, price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId, x.DataAreaId);
                            }
                            else
                            {
                                price = x.CustomerPrice;
                            }
                        }

                        x.CustomerPrice = this.ChangePrice(price, request.Currency);

                        x.IsInNewsletter = false;

                        x.IsInCart = shoppingCartCollection.IsInCart(x.ProductId);

                        string inventLocationId = x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, StringComparison.OrdinalIgnoreCase) ? CompanyGroup.Domain.Core.Constants.OuterStockHrp : CompanyGroup.Domain.Core.Constants.OuterStockBsc;

                        if (inventSumList.IsInList(x.ProductId, x.DataAreaId, inventLocationId, x.StandardConfigId))
                        {
                            x.Stock = inventSumList.GetStock(x.ProductId, x.Stock, x.DataAreaId, inventLocationId, x.StandardConfigId);
                        }

                        //használt lista hozzárendelése
                        x.SecondHandList = (x.SecondHand) ? this.GetSecondHandList(x.ProductId, request.Currency, inventSumList) : new Domain.WebshopModule.SecondHandList(new List<Domain.WebshopModule.SecondHand>());

                        //akkor lehet csak igaz a használt flag beállítás, ha van 0-nál nagyobb elemszámmal rendelkező használtcikk lista a cikkhez
                        x.SecondHand = (x.SecondHandList.Count > 0);
                    });
                }
                else
                {
                    //nincs bejelentkezve állapotban a használt lista hozzárendelése termékazonosítónként
                    products.ForEach(x =>
                    {
                        //használt lista hozzárendelése
                        x.SecondHandList = (x.SecondHand) ? this.GetSecondHandList(x.ProductId, request.Currency, new CompanyGroup.Domain.WebshopModule.InventSumList(new List<CompanyGroup.Domain.WebshopModule.InventSum>())) : new Domain.WebshopModule.SecondHandList(new List<Domain.WebshopModule.SecondHand>());

                        //akkor lehet csak igaz a használt flag beállítás, ha van 0-nál nagyobb elemszámmal rendelkező használtcikk lista a cikkhez
                        x.SecondHand = (x.SecondHandList.Count > 0);
                    });
                }


                //IQueryable<CompanyGroup.Domain.WebshopModule.Product> orderedList = filteredQueryableList.OrderByDescending(x => x.AverageInventory).Skip((request.CurrentPageIndex - 1) * request.ItemsOnPage).Take(request.ItemsOnPage);

                //CompanyGroup.Domain.WebshopModule.Products products = new CompanyGroup.Domain.WebshopModule.Products(new CompanyGroup.Domain.WebshopModule.Pager(request.CurrentPageIndex, filteredList.Count(), request.ItemsOnPage));

                //products.AddRange(orderedList);

                products.ListCount = count;

                //logikailag törölt elemeket ki kell szűrni a listából
                products.RemoveEndOfSalesNoStock();

                //ha a használt cikk szűrő be van kapcsolva, akkor ami nem használt cikk, azt nem kell listázni!
                if (request.SecondhandFilter)
                {
                    //products.RemoveNoSecondHand();
                }

                CompanyGroup.Dto.WebshopModule.Products response = new ProductsToProducts().Map(products);

                response.Pager = new PagerToPager().Map(products.Pager, request.ItemsOnPage);

                response.Currency = request.Currency;

                //CompanyGroup.Domain.WebshopModule.Structures structures = new CompanyGroup.Domain.WebshopModule.Structures();

                //List<CompanyGroup.Domain.WebshopModule.Structure> structureList = filteredList.ConvertAll(x => new CompanyGroup.Domain.WebshopModule.Structure() { Manufacturer = x.Structure.Manufacturer, Category1 = x.Structure.Category1, Category2 = x.Structure.Category2, Category3 = x.Structure.Category3 });

                //structures.AddRange(structureList);

                //CompanyGroup.Dto.WebshopModule.Structures s = new StructuresToStructures().Map(request.ManufacturerIdList, request.Category1IdList, request.Category2IdList, request.Category3IdList, structures);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ConstructDataAreaId(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request)
        {
            return ConstructDataAreaId(request.HrpFilter, request.BscFilter);
        }

        private static string ConstructDataAreaId(bool hrpFilter, bool bscFilter)
        {
            //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
            string dataAreaId = String.Empty;

            if (hrpFilter && !bscFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
            }
            else if (bscFilter && !hrpFilter)
            {
                dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
            }

            return dataAreaId;
        }

        private static System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> ConstructPredicate(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request)
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

            if (request.ManufacturerIdList.Count > 0)
            {
                predicate = predicate.And(p => request.ManufacturerIdList.Contains(p.Structure.Manufacturer.ManufacturerId));
            }
            if (request.Category1IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category1IdList.Contains(p.Structure.Category1.CategoryId));
            }
            if (request.Category2IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category2IdList.Contains(p.Structure.Category2.CategoryId));
            }
            if (request.Category3IdList.Count > 0)
            {
                predicate = predicate.And(p => request.Category3IdList.Contains(p.Structure.Category3.CategoryId));
            }
            if (request.DiscountFilter)
            {
                predicate = predicate.And(p => p.Discount.Equals(request.DiscountFilter));
            }
            if (request.IsInNewsletterFilter)
            {
                predicate = predicate.And(p => p.IsInNewsletter.Equals(request.IsInNewsletterFilter));
            }
            if (request.NewFilter)
            {
                predicate = predicate.And(p => p.New.Equals(request.NewFilter));
            }
            if (request.StockFilter)
            {
                predicate = predicate.And(p => p.Stock > 0);
            }

            if (!String.IsNullOrEmpty(request.TextFilter))
            {
                predicate = predicate.And(p => request.TextFilter.Contains(p.ProductName));
            }

            if (request.PriceFilterRelation.Equals(1))
            {
                int price;

                predicate = predicate.And(p => p.Prices.Price2 >= ((Int32.TryParse(request.PriceFilter, out price)) ? price : 0));
            }
            else if (request.PriceFilterRelation.Equals(2))
            {
                int price;

                predicate = predicate.And(p => p.Prices.Price2 <= ((Int32.TryParse(request.PriceFilter, out price)) ? price : 0));
            }
            if (request.SecondhandFilter)
            {
                predicate = predicate.And(p => p.SecondHandList.Count > 0);
            }

            return predicate.And(defaultPredicate);
        }

        private static System.Linq.Expressions.Expression<Func<CompanyGroup.Domain.WebshopModule.Product, bool>> ConstructBannerListPredicate(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request)
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

            predicate = predicate.And(p => p.Stock > 0);

            predicate = predicate.And(p => p.Discount.Equals(request.DiscountFilter));

            predicate = predicate.And(p => p.PictureId > 0);

            return predicate.And(defaultPredicate);       
        }

        /// <summary>
        /// használt lista kiolvasása a megadott termékazonosító szerint
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private CompanyGroup.Domain.WebshopModule.SecondHandList GetSecondHandList(string productId, string currency, CompanyGroup.Domain.WebshopModule.InventSumList inventSumList)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrEmpty(productId), "ProductService GetSecondHandList productId parameter cannot be null, or empty!");

                CompanyGroup.Domain.WebshopModule.SecondHandList secondHandList = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.SecondHandList>(CACHEKEY_SECONDHAND);

                //ha nincs a cache-ben, vagy 0 db. van a listában
                if (secondHandList == null || secondHandList.Count.Equals(0))
                {
                    secondHandList = productRepository.GetSecondHandList();

                    //cache-be mentés
                    if (ProductService.CatalogueCacheEnabled)
                    {
                        CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.SecondHandList>(CACHEKEY_SECONDHAND, secondHandList, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_SECONDHAND)));
                    }
                }

                //ha nincs min dolgozni, akkor üres listát adunk vissza
                if (secondHandList == null)
                {
                    return new CompanyGroup.Domain.WebshopModule.SecondHandList(new List<CompanyGroup.Domain.WebshopModule.SecondHand>());
                }

                //a termékazonosítóval rendelkező listát kell szűrni
                IEnumerable<CompanyGroup.Domain.WebshopModule.SecondHand> results = secondHandList.Where(x => x.ProductId.Equals(productId));

                List<CompanyGroup.Domain.WebshopModule.SecondHand> resultList = results.ToList();

                resultList.ForEach(x => 
                {
                    //használt ár beállítása, 
                    decimal price = Convert.ToDecimal(x.Price);

                    x.CustomerPrice = this.ChangePrice(price, currency);

                    //használt cikk készletváltozás beállítása
                    int stock = inventSumList.GetStock(x.ProductId, x.Quantity, x.DataAreaId, x.InventLocationId, x.ConfigId);

                    x.Quantity = stock;
                });

                CompanyGroup.Domain.WebshopModule.SecondHandList secondHandListResult = new CompanyGroup.Domain.WebshopModule.SecondHandList(resultList);

                //azokat a sorokat távolítja el a listából, melyeknek nincsen darabszáma (használt cikk csak készlet erejéig értékesíthető)
                secondHandListResult.RemoveNoStock();

                return secondHandListResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// 1. repository-tól elkéri az akciós, készleten lévő, képpel rendelkező legfeljebb 50 elemet tartalmazó terméklistát.
        /// 2. ha a hívóparaméter tartalmaz jelleg1, jelleg2, jelleg3 paramétert, akkor a paraméterek szerint szűri a listát, majd a találati eredményből visszaadja az első 50 elemet
        /// 3. ha a lekérdezésnek nincs eredménye, akkor az 1. pont szerinti lekérdezés eredményéből visszaadja az első 50 elemet
        /// 
        /// ha bejelentkezett felhasználó kérte le a listát, akkor az eredményhalmazban az ár is kitöltésre kerül, egyébként az ár értéke nulla.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.WebshopModule.GetBannerListRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "ProductService GetBannerList request cannot be null, or empty!");

            try
            {
                request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
                string dataAreaId = ConstructDataAreaId(request.HrpFilter, request.BscFilter);

                CompanyGroup.Domain.WebshopModule.BannerProducts bannerProducts = productRepository.GetBannerList(dataAreaId,
                                                                                                                  String.Empty,
                                                                                                                  ConvertData.ConvertStringListToDelimitedString(request.Category1IdList),
                                                                                                                  ConvertData.ConvertStringListToDelimitedString(request.Category2IdList),
                                                                                                                  ConvertData.ConvertStringListToDelimitedString(request.Category3IdList));

                CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

                //ár kalkulálás
                if (visitor.IsValidLogin)
                {
                    bannerProducts.ForEach(x =>
                    {
                        decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId, x.DataAreaId);

                        x.CustomerPrice = this.ChangePrice(price, request.Currency);
                    });
                }

                CompanyGroup.Dto.WebshopModule.BannerList results = new BannerProductListToBannerList().Map(bannerProducts);

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.WebshopModule.GetPriceListRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require((request != null), "ProductService GetPriceList request cannot be null, or empty!");

                request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
                string dataAreaId = ConstructDataAreaId(request.HrpFilter, request.BscFilter);

                int priceFilterRelation = 0;

                int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

                CompanyGroup.Domain.WebshopModule.PriceList priceList = productRepository.GetPriceList(dataAreaId,
                                                                                                       ConvertData.ConvertStringListToDelimitedString(request.ManufacturerIdList),
                                                                                                       ConvertData.ConvertStringListToDelimitedString(request.Category1IdList),
                                                                                                       ConvertData.ConvertStringListToDelimitedString(request.Category2IdList),
                                                                                                       ConvertData.ConvertStringListToDelimitedString(request.Category3IdList),
                                                                                                       request.ActionFilter,
                                                                                                       request.BargainFilter,
                                                                                                       request.IsInNewsletterFilter,
                                                                                                       request.NewFilter,
                                                                                                       request.StockFilter,
                                                                                                       request.TextFilter,
                                                                                                       request.PriceFilter,
                                                                                                       priceFilterRelation,
                                                                                                       request.Sequence);

                //ha nincs bejelentkezve, akkor a VisitorId üres
                CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

                //érvényes belépés esetén ár, hírlevél akció kalkulálása 
                if (visitor.IsValidLogin)
                {
                    priceList.ForEach(x =>
                    {
                        decimal price = visitor.CalculateCustomerPrice(x.Prices.Price1, x.Prices.Price2, x.Prices.Price3, x.Prices.Price4, x.Prices.Price5, x.Structure.Manufacturer.ManufacturerId, x.Structure.Category1.CategoryId, x.Structure.Category2.CategoryId, x.Structure.Category3.CategoryId, x.DataAreaId);

                        x.CustomerPrice = this.ChangePrice(price, request.Currency);

                        x.IsInNewsletter = false;
                    });
                }

                CompanyGroup.Dto.WebshopModule.PriceList results = new PriceListToPriceList().Map(priceList);

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require((request != null), "ProductService GetItemByProductId request cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.ProductId), "ProductService GetItemByProductId request.ProductId cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.DataAreaId), "ProductService GetItemByProductId request.DataAreaId cannot be null, or empty!");

                //ha nincs bejelentkezve, akkor a VisitorId üres
                CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(visitor.Id, request.ProductId, request.DataAreaId);

                Helpers.DesignByContract.Ensure((product != null), "ProductService productRepository.GetItem result can not be null!");

                if (visitor.IsValidLogin)
                {
                    CompanyGroup.Domain.WebshopModule.InventSumList inventSumList = changeTrackingRepository.InventSumCT(0);

                    CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList = changeTrackingRepository.PriceDiscTableCT(0);

                    List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollection(request.VisitorId);

                    CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                    decimal price = 0; int price1 = 0; int price2 = 0; int price3 = 0; int price4 = 0; int price5 = 0;

                    //egyedi ár beállítás CT-ben ha van, akkor kiolvasásra kerül
                    if (priceDiscTableList.IsInList(product.ProductId, product.DataAreaId, visitor.CustomerId))
                    {
                        price = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, visitor.CustomerId);
                    }

                    //ha nincs egyedi beállítás a termékre, akkor lehet 1..5 intervallum érték változás a CT-ben, 
                    //vagy a termékhez rendelt 1..5 árral kell kalkulálni
                    if (price.Equals(0))
                    {
                        price1 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "1");
                        price2 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "2");
                        price3 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "3");
                        price4 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "4");
                        price5 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "5");


                        //ha van 1..5 intervallum érték változás a CT-ben, akkor ezzel kell számolni
                        if (!price1.Equals(0) || !price2.Equals(0) || !price3.Equals(0) || !price4.Equals(0) || !price5.Equals(0))
                        {
                        if (price1.Equals(0)) { price1 = product.Prices.Price1; }
                        if (price2.Equals(0)) { price2 = product.Prices.Price2; }
                        if (price3.Equals(0)) { price3 = product.Prices.Price3; }
                        if (price4.Equals(0)) { price4 = product.Prices.Price4; }
                        if (price5.Equals(0)) { price5 = product.Prices.Price5; }

                        price = visitor.CalculateCustomerPrice(price1, price2, price3, price4, price5,
                                                               product.Structure.Manufacturer.ManufacturerId,
                                                               product.Structure.Category1.CategoryId,
                                                               product.Structure.Category2.CategoryId,
                                                               product.Structure.Category3.CategoryId,
                                                               product.DataAreaId);
                        }
                        else
                        {
                            price = product.CustomerPrice;
                        }
                    }

                    product.CustomerPrice = this.ChangePrice(price, request.Currency);

                    product.IsInNewsletter = false;

                    product.IsInCart = shoppingCartCollection.IsInCart(product.ProductId);

                    //használt lista
                    product.SecondHandList = (product.SecondHand) ? this.GetSecondHandList(product.ProductId, request.Currency, inventSumList) : new Domain.WebshopModule.SecondHandList(new List<Domain.WebshopModule.SecondHand>());

                    //készletváltozás kalkuláció
                    string inventLocationId = product.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, StringComparison.OrdinalIgnoreCase) ? CompanyGroup.Domain.Core.Constants.OuterStockHrp : CompanyGroup.Domain.Core.Constants.OuterStockBsc;

                    if (inventSumList.IsInList(product.ProductId, product.DataAreaId, inventLocationId, product.StandardConfigId))
                    {
                        product.Stock = inventSumList.GetStock(product.ProductId, product.Stock, product.DataAreaId, inventLocationId, product.StandardConfigId);
                    }
                }
                else
                {
                    product.CustomerPrice = 0;

                    product.IsInNewsletter = false;

                    product.IsInCart = false;

                    product.SecondHandList = (product.SecondHand) ? this.GetSecondHandList(product.ProductId, request.Currency, new CompanyGroup.Domain.WebshopModule.InventSumList(new List<CompanyGroup.Domain.WebshopModule.InventSum>())) : new Domain.WebshopModule.SecondHandList(new List<Domain.WebshopModule.SecondHand>());
                }

                //részletes adatlap log hozzáadás (akkor is hozzáadja, ha nincs bejelentkezve, de korábban be volt - van visitorId és van customerId)
                if (!String.IsNullOrEmpty(visitor.VisitorId) && !String.IsNullOrEmpty(visitor.CustomerId))
                {
                    productRepository.AddCatalogueDetailsLog(visitor.VisitorId, visitor.CustomerId, visitor.PersonId, request.DataAreaId, request.ProductId);
                }

                product.Pictures = pictureRepository.GetListByProduct(product.ProductId);

                CompanyGroup.Dto.WebshopModule.Product result = new ProductToProduct().Map(product);

                result.Currency = request.Currency;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="prefix"></param>
        /// <param name="completionType">0: nincs megadva, 1: termékazonosító-cikkszám, 2: minden</param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request) 
        {
            try
            {
                Helpers.DesignByContract.Require((request != null), "ProductService GetCompletionList request cannot be null, or empty!");

                if (String.IsNullOrWhiteSpace(request.Prefix))
                {
                    return new CompanyGroup.Dto.WebshopModule.CompletionList();
                }

                //vállalat akkor üres, ha a bsc, illetve a hrp is be van kapcsolva  
                string dataAreaId = ConstructDataAreaId(request.HrpFilter, request.BscFilter);

                request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                int priceFilterRelation = 0;

                int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

                //szöveges kereső paraméter FULL TEXT SEARCH paraméterré alakítása "ASUS*" AND "számítógép*"
                string prefix = CreateTextFilterCondition(request.Prefix);

                CompanyGroup.Domain.WebshopModule.CompletionList result = productRepository.GetCompletionList(prefix,
                                                                                                              dataAreaId,
                                                                                                              ConvertData.ConvertStringListToDelimitedString(request.ManufacturerIdList),
                                                                                                              ConvertData.ConvertStringListToDelimitedString(request.Category1IdList),
                                                                                                              ConvertData.ConvertStringListToDelimitedString(request.Category2IdList),
                                                                                                              ConvertData.ConvertStringListToDelimitedString(request.Category3IdList),
                                                                                                              request.DiscountFilter,
                                                                                                              request.SecondhandFilter,
                                                                                                              request.IsInNewsletterFilter,
                                                                                                              request.NewFilter,
                                                                                                              request.StockFilter,
                                                                                                              request.PriceFilter,
                                                                                                              priceFilterRelation);

                return new CompletionToCompletion().Map(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "Termék kompatibilitás"

        public CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.CompatibleProducts result = new CompanyGroup.Dto.WebshopModule.CompatibleProducts();

                CompanyGroup.Domain.PartnerModule.Visitor visitor = String.IsNullOrEmpty(request.VisitorId) ? CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor() : this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList = changeTrackingRepository.PriceDiscTableCT(0);

                result.Items = this.GetCompatibilityList(request.ProductId, request.DataAreaId, request.VisitorId, priceDiscTableList, visitor);

                result.ReverseItems = this.GetReverseCompatibilityList(request.ProductId, request.DataAreaId, request.VisitorId, priceDiscTableList, visitor);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> GetCompatibilityList(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> result = new List<CompanyGroup.Dto.WebshopModule.CompatibleProduct>();

                List<CompanyGroup.Domain.WebshopModule.CompatibilityItem> compatibilityItems = productRepository.GetCompatibilityItemList(productId, dataAreaId, false);

                compatibilityItems.ForEach(x => result.Add(this.GetCompatibleProduct(x.ProductId, x.DataAreaId, visitorId, priceDiscTableList, visitor)));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> GetReverseCompatibilityList(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                List<CompanyGroup.Dto.WebshopModule.CompatibleProduct> result = new List<CompanyGroup.Dto.WebshopModule.CompatibleProduct>();

                List<CompanyGroup.Domain.WebshopModule.CompatibilityItem> compatibilityItems = productRepository.GetCompatibilityItemList(productId, dataAreaId, true);

                compatibilityItems.ForEach(x => result.Add(this.GetCompatibleProduct(x.ProductId, x.DataAreaId, visitorId, priceDiscTableList, visitor)));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CompanyGroup.Dto.WebshopModule.CompatibleProduct GetCompatibleProduct(string productId, string dataAreaId, string visitorId, CompanyGroup.Domain.WebshopModule.PriceDiscTableList priceDiscTableList, CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(visitor.Id, productId, dataAreaId);

                if (product == null) { return new CompanyGroup.Dto.WebshopModule.CompatibleProduct(); }

                if (visitor.IsValidLogin)
                {
                    List<CompanyGroup.Domain.WebshopModule.ShoppingCart> carts = shoppingCartRepository.GetCartCollection(visitorId);

                    CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(carts);

                    decimal price = 0; int price1 = 0; int price2 = 0; int price3 = 0; int price4 = 0; int price5 = 0;

                    //egyedi ár beállítás CT-ben ha van, akkor kiolvasásra kerül
                    if (priceDiscTableList.IsInList(product.ProductId, product.DataAreaId, visitor.CustomerId))
                    {
                        price = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, visitor.CustomerId);
                    }

                    //ha nincs egyedi beállítás a termékre, akkor lehet 1..5 intervallum érték változás a CT-ben, 
                    //vagy a termékhez rendelt 1..5 árral kell kalkulálni
                    if (price.Equals(0))
                    {
                        price1 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "1");
                        price2 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "2");
                        price3 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "3");
                        price4 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "4");
                        price5 = priceDiscTableList.GetPrice(product.ProductId, product.DataAreaId, "5");


                        //ha van 1..5 intervallum érték változás a CT-ben, akkor ezzel kell számolni
                        if (!price1.Equals(0) || !price2.Equals(0) || !price3.Equals(0) || !price4.Equals(0) || !price5.Equals(0))
                        {
                            if (price1.Equals(0)) { price1 = product.Prices.Price1; }
                            if (price2.Equals(0)) { price2 = product.Prices.Price2; }
                            if (price3.Equals(0)) { price3 = product.Prices.Price3; }
                            if (price4.Equals(0)) { price4 = product.Prices.Price4; }
                            if (price5.Equals(0)) { price5 = product.Prices.Price5; }

                            price = visitor.CalculateCustomerPrice(price1, price2, price3, price4, price5,
                                                                   product.Structure.Manufacturer.ManufacturerId,
                                                                   product.Structure.Category1.CategoryId,
                                                                   product.Structure.Category2.CategoryId,
                                                                   product.Structure.Category3.CategoryId,
                                                                   product.DataAreaId);
                        }
                        else
                        {
                            price = product.CustomerPrice;
                        }
                    }

                    product.CustomerPrice = price;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// részletes adatlap log lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList GetCatalogueDetailsLogList(CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogListRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require((request != null), "ProductService GetCatalogueDetailsLogList request cannot be null, or empty!");

                //ha a látogató azonosító üres, akkor nincs mihez részletes adatlap log listát keresni, üreset kell visszaadni
                if (String.IsNullOrEmpty(request.VisitorId))
                {
                    return new CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList();
                }

                List<CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog> result = productRepository.CatalogueDetailsLogList(request.VisitorId);

                //ne legyen ugyanaz a termék egymás alatt
                List<CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog> filteredList = new List<CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog>();

                string tmp = String.Empty;

                result.ForEach( x => {
                    if (!x.ProductId.Equals(tmp))
                    {
                        filteredList.Add(x);
                    }
                    tmp = x.ProductId;
                });

                return new CatalogueDetailsLogToCatalogueDetailsLog().Map(filteredList);
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        /// <summary>
        /// készlet databszám befrissítése  
        /// </summary>
        /// <param name="request"></param>
        //public void StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request)
        //{
        //    Helpers.DesignByContract.Require((request != null), "ProductService StockUpdate request cannot be null, or empty!");

        //    try
        //    {
        //        CompanyGroup.Domain.WebshopModule.CatalogueStockUpdate req = new Domain.WebshopModule.CatalogueStockUpdate(request.DataAreaId, request.ProductId, request.Stock);

        //        productRepository.StockUpdate(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }      
        //}
    }
}
