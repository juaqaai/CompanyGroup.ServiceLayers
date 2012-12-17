using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using CompanyGroup.Data.MaintainModule;
using System.Linq;
using NHibernate;
using CompanyGroup.Domain.MaintainModule;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] 
    public class ProductService : IProductService
    {
        private CompanyGroup.Domain.MaintainModule.IProductRepository productMaintainRepository;

        private CompanyGroup.Domain.WebshopModule.IProductRepository productWebshopRepository;

        /// <summary>
        /// konstruktor repository interfész paraméterrel
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductService(CompanyGroup.Domain.MaintainModule.IProductRepository productMaintainRepository, CompanyGroup.Domain.WebshopModule.IProductRepository productWebshopRepository)
        {
            if (productMaintainRepository == null)
            {
                throw new ArgumentNullException("ProductRepository");
            }

            this.productMaintainRepository = productMaintainRepository;

            this.productWebshopRepository = productWebshopRepository;
        }

        /// <summary>
        /// terméklista cache törlése
        /// </summary>
        /// <returns></returns>
        public bool ClearProductCache()
        {
            try
            {
                //terméklista törlése
                productWebshopRepository.RemoveAllItemsFromCollection();

                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        ///  terméklista cache újra töltése vállalatkódtól függően
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public bool RefillProductCache(string dataAreaId)
        {
            return FillProductCache(dataAreaId, true);
        }

        /// <summary>
        ///  terméklista cache töltése vállalatkódtól függően
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public bool FillProductCache(string dataAreaId)
        { 
            return FillProductCache(dataAreaId, false);
        }

        /// <summary>
        /// terméklista cache opcionális újra töltése vállalatkódtól függően
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="clearCache"></param>
        private bool FillProductCache(string dataAreaId, bool clearCache)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(dataAreaId), "The dataareaId cannot be null!");

            CompanyGroup.Helpers.DesignByContract.Require(dataAreaId.Equals(Domain.Core.Constants.DataAreaIdHrp) || dataAreaId.Equals(Domain.Core.Constants.DataAreaIdBsc), "The value of dataareaId can be hrp / bsc !");

            try
            {
                //struktúra (jelleg1, jelleg2, jelleg3) lekérdezések
                List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> firstLevelCategories = productMaintainRepository.GetFirstLevelCategoryList(dataAreaId);

                List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> secondLevelCategories = productMaintainRepository.GetSecondLevelCategoryList(dataAreaId);

                List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> thirdLevelCategories = productMaintainRepository.GetThirdLevelCategoryList(dataAreaId);

                List<CompanyGroup.Domain.MaintainModule.Manufacturer> manufacturers = productMaintainRepository.GetManufacturerList(dataAreaId);

                //termékleírás lekérdezés
                List<CompanyGroup.Domain.MaintainModule.ProductDescription> productDescriptions = productMaintainRepository.GetProductDescriptionList(dataAreaId);

                //termékmanager lekérdezés
                //List<CompanyGroup.Domain.MaintainModule.ProductManager> productManagers = productMaintainRepository.GetProductManagerList(dataAreaId);

                //képek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.Picture> pictures = productMaintainRepository.GetPictureList(dataAreaId);

                //készletek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.Stock> stocks = productMaintainRepository.GetStockList(dataAreaId);

                //beszerzési rendelések lekérdezés
                List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine> purchaseOrderLines = productMaintainRepository.GetPurchaseOrderLineList(dataAreaId);

                //használt cikkek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.SecondHand> secondHands = productMaintainRepository.GetSecondHandList(dataAreaId);

                //angol termékmevek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.InventName> inventNames = productMaintainRepository.GetInventNameEnglishList(dataAreaId);

                //használt cikkek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.Product> secondHandProducts = productMaintainRepository.GetSecondHandProductList(dataAreaId);

                //cikkek lekérdezés
                List<CompanyGroup.Domain.MaintainModule.Product> products = productMaintainRepository.GetProductList(dataAreaId);

                //leértékelt lista terméklistához történő hozzáadása        
                products.AddRange(secondHandProducts);

                products.ForEach(delegate(Product item)
                {
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

                    item.ShippingDate = GetShippingDate(purchaseOrderLines, item.ProductId);

                    //item.ProductManager = GetProductManager(productManagers, item.ProductManager.EmployeeId);

                    ProductDescription descHun = GetProductDescription(productDescriptions, item.ProductId, Domain.Core.Constants.LanguageHungarian);

                    ProductDescription descEng = GetProductDescription(productDescriptions, item.ProductId, Domain.Core.Constants.LanguageEnglish);

                    item.Description = descHun.Description;

                    item.DescriptionEnglish = descEng.Description;

                    item.Pictures.AddRange(GetPictures(pictures, item.ProductId));

                    item.InnerStock = GetStock(dataAreaId, dataAreaId == Domain.Core.Constants.DataAreaIdHrp ? Domain.Core.Constants.InnerStockHrp : Domain.Core.Constants.InnerStockBsc, item.ProductId, stocks);
                    item.OuterStock = GetStock(dataAreaId, dataAreaId == Domain.Core.Constants.DataAreaIdHrp ? Domain.Core.Constants.OuterStockHrp : Domain.Core.Constants.OuterStockBsc, item.ProductId, stocks);

                    item.SecondHandList = GetSecondHands(secondHands, item.ProductId);

                });

                List<CompanyGroup.Domain.WebshopModule.Product> productList = products.ConvertAll(x => CompanyGroup.Domain.WebshopModule.Factory.CreateProduct(x));

                //ha törölni kell a cache-t
                if (clearCache)
                {
                    productWebshopRepository.RemoveItemsFromCollection(dataAreaId);
                }

                //termékelemek beszúrása
                productWebshopRepository.InsertList(productList);

                return true;
            }
            catch { return false; }
        }

        #region RefillProductCache private methods

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

        private List<SecondHand> GetSecondHands(List<CompanyGroup.Domain.MaintainModule.SecondHand> secondHands, string key)
        {
            List<SecondHand> secondHandList = secondHands.FindAll(x => x.ProductId.ToLower() == key.ToLower());

            return secondHandList != null ? secondHandList : new List<SecondHand>();
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

        /// <summary>
        /// várható szállítási időpont kalkuláció
        /// </summary>
        /// <param name="purchaseOrderLines"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private DateTime GetShippingDate(List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine> purchaseOrderLines, string key)
        {
            CompanyGroup.Domain.MaintainModule.PurchaseOrderLine purchaseOrderLine = purchaseOrderLines.FirstOrDefault(x => x.ItemId == key);

            return (purchaseOrderLine != null) ? purchaseOrderLine.DeliveryDate : DateTime.MinValue;
        }

        ///// <summary>
        ///// terméklista beírása mongoDb-be
        ///// </summary>
        ///// <param name="items"></param>
        /*
        private void InsertProducts(List<CompanyGroup.Domain.MaintainModule.Product> items)
        {
            CompanyGroup.Domain.WebshopModule.Pager pager = new CompanyGroup.Domain.WebshopModule.Pager(0, 0, 0);

            CompanyGroup.Domain.WebshopModule.Products products = new CompanyGroup.Domain.WebshopModule.Products(pager);

            items.ForEach(x => products.Add(CompanyGroup.Domain.WebshopModule.Factory.CreateProduct(x)));

            productWebshopRepository.InsertList(products);
        }
        */
        #endregion
    }
}
