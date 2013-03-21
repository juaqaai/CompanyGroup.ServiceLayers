using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// bevásárlókosárhoz tartozó application szervizek
    /// </summary>
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                InstanceContextMode = InstanceContextMode.PerCall,
    //                ConcurrencyMode = ConcurrencyMode.Multiple,
    //                IncludeExceptionDetailInFaults = true),
    //                System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()]
    public class ShoppingCartService : ServiceBase, IShoppingCartService
    {
        private CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository;

        private CompanyGroup.Domain.WebshopModule.IProductRepository productRepository;

        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        private CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository;

        public ShoppingCartService(CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository, 
                                   CompanyGroup.Domain.WebshopModule.IProductRepository productRepository, 
                                   CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository,
                                   CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository,
                                   CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository,
                                   CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository) : base(financeRepository, visitorRepository)
        {
            if (shoppingCartRepository == null)
            {
                throw new ArgumentNullException("ShoppingCartRepository");
            }

            if (productRepository == null)
            {
                throw new ArgumentNullException("ProductRepository");
            }

            if (customerRepository == null)
            {
                throw new ArgumentNullException("CustomerRepository");
            }

            if (salesOrderRepository == null)
            {
                throw new ArgumentNullException("SalesOrderRepository");
            }

            this.shoppingCartRepository = shoppingCartRepository;

            this.productRepository = productRepository;

            this.visitorRepository = visitorRepository;

            this.customerRepository = customerRepository;

            this.salesOrderRepository = salesOrderRepository;
        }

        #region "kosár műveletek"

        /// <summary>
        /// új visitorId beállítása a régi helyére, ha a kosár mentett státuszú, 
        /// hozzáad egy nyitott kosarat a felhasználó kosaraihoz
        /// 1. permanens visitorId alapján a kosarak kiolvasása
        /// 2. végig az előző eredménylistán, és az új visitorId beállítása a permanensId helyére
        /// 3. kosarak lista visszaadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.WebshopModule.AssociateCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "ShoppingCartService AssociateCart request cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "ShoppingCartService AssociateCart VisitorId cannot be null, or empty!");

            try
            {
                //visitor lekérdezés
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService AssociateCart visitor must be logged in!");

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(0, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(newShoppingCart);

                Helpers.DesignByContract.Ensure(newCartId > 0, "ShoppingCartService AssociateCart newCartId can not be zero!");

                //permanent visitor lekérdezés
                CompanyGroup.Domain.PartnerModule.Visitor permanentVisitor = this.GetVisitor(request.PermanentId);

                //ha a korábban bejelentkező ugyanaz a cég, vagy személy, mint korábban
                if (visitor.EqualsVisitor(permanentVisitor))
                {
                    //beállítja az új VisitorId-t, a permanens helyett a mentett kosarakra
                    shoppingCartRepository.AssociateCart(request.PermanentId, request.VisitorId);
                }

                //aktív státusz beállítása
                //shoppingCartRepository.SetActive(newCartId, request.VisitorId);

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(newCartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //felhasználóhoz tartozó kosár fejlécek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart), 
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// bevásárlókosár hozzáadása bevásárlókosár kollekcióhoz, 
        /// új kosár inicializálása + új elem hozzáadása
        /// </summary>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.WebshopModule.AddCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "AddCart request cannot be null!");

            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService AddCart visitor must be logged in!");                

                //kosár név kalkulálása
                string cartName = String.IsNullOrEmpty(request.CartName) ? ShoppingCartService.CreateCartName() : request.CartName;

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(0, request.VisitorId, visitor.CustomerId, visitor.PersonId, cartName, visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(shoppingCart);

                Helpers.DesignByContract.Ensure(newCartId > 0, "ShoppingCartService AddCart newCartId can not be zero!");

                //shoppingCartRepository.SetActive(newCartId, request.VisitorId);

                //aktív kosár frissítése kiolvasással
                shoppingCart = shoppingCartRepository.GetShoppingCart(newCartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //felhasználóhoz tartozó kosár fejléccek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems, 
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// kosár mentése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo SaveCart(CompanyGroup.Dto.WebshopModule.SaveCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "SaveCart request cannot be null!");

            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Name), "Name cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "CartId must be greather than zero!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService SaveCart visitor must be logged in!");         

                //nincs kosár azonosító
                //if (request.CartId == 0)
                //{
                //    //kosár név kalkulálása
                //    string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;

                //    //új kosár létrehozása és hozzáadása
                //    CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, cartName, visitor.Currency, true);

                //    int newCartId = shoppingCartRepository.Add(shoppingCart);

                //    Helpers.DesignByContract.Ensure(newCartId > 0, "ShoppingCartService AddCart newCartId can not be zero!");

                //    shoppingCartRepository.Store(newCartId, ShoppingCartService.CreateCartName());
                //}
                //else
                //{
                string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;
                    
                shoppingCartRepository.Store(request.CartId, cartName);
                //}

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //felhasználóhoz tartozó kosár fejléccek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár törlése kosár kollekcióból 
        /// </summary>
        /// <param name="cartId"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo RemoveCart(CompanyGroup.Dto.WebshopModule.RemoveCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "RemoveCart request cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "Visitor id cannot be null!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService RemoveCart visitor must be logged in!");       

                shoppingCartRepository.Remove(request.CartId);

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(0, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(newShoppingCart);

                Helpers.DesignByContract.Ensure(newCartId > 0, "ShoppingCartService AssociateCart newCartId can not be zero!");

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(newCartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //felhasználóhoz tartozó kosár fejlécek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// aktív kosár beállítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.WebshopModule.ActivateCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "ActivateCart request cannot be null!");

            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService ActivateCart visitor must be logged in!");   

                //active-ra állítás
                shoppingCartRepository.SetActive(request.CartId, request.VisitorId);

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //felhasználóhoz tartozó kosár fejléccek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// kosár név kalkulálása
        /// </summary>
        /// <returns></returns>
        private static string CreateCartName()
        {
            string month = (DateTime.Now.Month < 10) ? String.Format("0{0}", DateTime.Now.Month) : String.Format("{0}", DateTime.Now.Month);

            string day = (DateTime.Now.Day < 10) ? String.Format("0{0}", DateTime.Now.Day) : String.Format("{0}", DateTime.Now.Day);

            string hour = (DateTime.Now.Hour < 10) ? String.Format("0{0}", DateTime.Now.Hour) : String.Format("{0}", DateTime.Now.Hour);

            string minute = (DateTime.Now.Minute < 10) ? String.Format("0{0}", DateTime.Now.Minute) : String.Format("{0}", DateTime.Now.Minute);

            string second = (DateTime.Now.Second < 10) ? String.Format("0{0}", DateTime.Now.Second) : String.Format("{0}", DateTime.Now.Second);

            return String.Format("Kosár {0}.{1}.{2} - {3} : {4} : {5}", DateTime.Now.Year, month, day, hour, minute, second);
        }

        #region "kosár sor műveletek"

        /// <summary>
        /// új elem hozzáadása az aktív kosárhoz, 
        /// ha nincs aktív kosár, akkor létrehoz egy új kosarat és ahhoz adja hozzá az új elemet
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions AddLine(CompanyGroup.Dto.WebshopModule.AddLineRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "AddLine request cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.DataAreaId), "DataAreaId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.ProductId), "Product cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require(request.Quantity > 0, "Quantity must be greather than zero!");

            Helpers.DesignByContract.Require((request.CartId > 0), "CartId cannot be null, or empty!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService AddLine visitor must be logged in!");

                //termék lekérdezés, vizsgálat
                CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(request.ProductId, request.DataAreaId);

                Helpers.DesignByContract.Invariant(product != null, "productRepository.GetItem result cannot be null!");

                //új shoppingCartItem létrehozása, inicializálás
                CompanyGroup.Domain.WebshopModule.ShoppingCartItem shoppingCartItem = new Domain.WebshopModule.ShoppingCartItem();

                //ha használt rendelés történik    
                if (request.SecondHand)
                {
                    CompanyGroup.Domain.WebshopModule.SecondHand secondHand = this.GetSecondHand(request.ProductId);

                    shoppingCartItem.SetSecondHandProduct(secondHand, product.ProductName, product.ProductNameEnglish, product.PartNumber, product.ItemState);
                }
                else
                {
                    shoppingCartItem.SetProduct(product);

                    //ár beállítás csak akkor, ha kereskedelmi készletről történik az értékesítés
                    shoppingCartItem.CustomerPrice = Convert.ToInt32(visitor.CalculateCustomerPrice(product.Prices.Price1, product.Prices.Price2, product.Prices.Price3, product.Prices.Price4, product.Prices.Price5,
                                                                                                    product.Structure.Manufacturer.ManufacturerId, product.Structure.Category1.CategoryId, product.Structure.Category2.CategoryId, product.Structure.Category3.CategoryId));

                }

                shoppingCartItem.CartId = request.CartId;

                shoppingCartItem.Quantity = request.Quantity;

                 //ha létezik a termék a kosárban, akkor a cikk mennyisége frissítésre kerül, ha nem létezik akkor hozzáadás kosárhoz
                shoppingCartRepository.AddLine(shoppingCartItem);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //finanszírozandó összeg
                int financedAmount = shoppingCart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(shoppingCart, leasingOptions);

                response.Currency = request.Currency;  

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region "SecondHand"

        private const string CACHEKEY_SECONDHAND = "secondhand";

        private const double CACHE_EXPIRATION_SECONDHAND = 1d;

        /// <summary>
        /// lekérdezi a SecondHand listát és kiveszi belőle a keresett értéket
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private CompanyGroup.Domain.WebshopModule.SecondHand GetSecondHand(string productId)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrEmpty(productId), "ShoppingCartService GetSecondHand productId parameter cannot be null, or empty!");

            CompanyGroup.Domain.WebshopModule.SecondHandList secondHandList = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.SecondHandList>(CACHEKEY_SECONDHAND);

            //ha nincs a cache-ben
            if (secondHandList == null)
            {
                secondHandList = productRepository.GetSecondHandList();

                //cache-be mentés
                CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.SecondHandList>(CACHEKEY_SECONDHAND, secondHandList, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_SECONDHAND)));
            }

            //ha nincs min dolgozni, akkor üres elemet adunk vissza
            if (secondHandList == null)
            {
                return new CompanyGroup.Domain.WebshopModule.SecondHand();
            }

            //a termékazonosítóval rendelkező listát kell szűrni
            IEnumerable<CompanyGroup.Domain.WebshopModule.SecondHand> resultList = secondHandList.Where(x => x.ProductId.Equals(productId));

            CompanyGroup.Domain.WebshopModule.SecondHand secondHand = resultList.ToList().FirstOrDefault();

            return secondHand == null ? new CompanyGroup.Domain.WebshopModule.SecondHand() : secondHand;
        }

        #endregion

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions RemoveLine(CompanyGroup.Dto.WebshopModule.RemoveLineRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "RemoveLine request cannot be null!");

            //ellenörzés 
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null!");

            Helpers.DesignByContract.Require((request.LineId > 0), "LineId cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService ActivateCart visitor must be logged in!");   

                //sor törlése
                shoppingCartRepository.RemoveLine(request.LineId);

                //kosár olvasása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //finanszírozandó összeg
                int financedAmount = shoppingCart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(shoppingCart, leasingOptions);

                response.Currency = request.Currency;  

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// meglévő kosárban elem frissítése 
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions UpdateLineQuantity(CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest request)
        {
            //ellenörzés, 
            Helpers.DesignByContract.Require((request != null), "UpdateLineQuantity request cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            Helpers.DesignByContract.Require((request.LineId > 0), "Line id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "ShoppingCartService UpdateLineQuantity visitor must be logged in!"); 

                shoppingCartRepository.UpdateLineQuantity(request.LineId, request.Quantity);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //finanszírozandó összeg
                int financedAmount = shoppingCart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(shoppingCart, leasingOptions);

                response.Currency = request.Currency;  

                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "kosár lekérdezések"

        /// <summary>
        /// kosár lekérdezés kosárazonosító alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetShoppingCartInfo(CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "GetShoppingCartInfo request cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Invariant((request.CartId > 0), "CartId cannot be null!");

            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Invariant(visitor.IsValidLogin, "ShoppingCartService GetShoppingCartInfo visitor must be logged in!");

                //felhasználóhoz tartozó kosár fejlécek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach( x => { 
                        decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                        x.CustomerPrice = (int) price;
                    });

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                                                                                                                               request.Currency);
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
                //CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo()
                //           {
                //               ActiveCart = new Dto.WebshopModule.ShoppingCart()
                //                            {
                //                                DeliveryTerms = 0,
                //                                Id = 0,
                //                                Items = new List<Dto.WebshopModule.ShoppingCartItem>(),
                //                                PaymentTerms = 0,
                //                                Shipping = new Dto.WebshopModule.Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.MinValue, InvoiceAttached = false, Street = "", ZipCode = "" },
                //                                SumTotal = 0
                //                            },
                //               OpenedItems = new List<Dto.WebshopModule.OpenedShoppingCart>(),
                //               StoredItems = new List<Dto.WebshopModule.StoredShoppingCart>(), 
                //               //FinanceOffer = new Dto.WebshopModule.FinanceOffer(), 
                //               LeasingOptions = new Dto.WebshopModule.LeasingOptions()
                //           };
                //return response;
            }
        }

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "GetCartByKey request cannot be null!");

            Helpers.DesignByContract.Require((request.CartId > 0), "CartId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Invariant(visitor.IsValidLogin, "ShoppingCartService GetCartByKey visitor must be logged in!");  

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                CompanyGroup.Dto.WebshopModule.ShoppingCart response = new ShoppingCartToShoppingCart().Map(shoppingCart);

                //response.Currency = request.Currency;

                return response;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCart(); }
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetActiveCart(CompanyGroup.Dto.WebshopModule.GetActiveCartRequest request)
        {
            Helpers.DesignByContract.Require((request != null), "GetActiveCart request cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Invariant(visitor.IsValidLogin, "ShoppingCartService GetActiveCart visitor must be logged in!");  

                //felhasználóhoz tartozó kosár fejlécek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(shoppingCartHeaderCollection.ActiveCartId);

                //valutanem szerinti összeg beállítás
                shoppingCart.GetItems().ForEach(x =>
                {
                    decimal price = this.ChangePrice(x.CustomerPrice, request.Currency);
                    x.CustomerPrice = (int)price;
                });

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(storedOpenedShoppingCarts.StoredItems,
                                                                                                                               storedOpenedShoppingCarts.OpenedItems,
                                                                                                                               new ShoppingCartToShoppingCart().Map(shoppingCart),
                                                                                                                               new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                                                               request.Currency);
                return response;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(); }
        }

        #endregion

        #region "rendelés feladása"

        /// <summary>
        /// rendelés elküldése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.OrderFulFillment CreateOrder(CompanyGroup.Dto.WebshopModule.SalesOrderCreateRequest request)
        {
            CompanyGroup.Dto.WebshopModule.OrderFulFillment response = new CompanyGroup.Dto.WebshopModule.OrderFulFillment();

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            bool isValidated = false;

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                Helpers.DesignByContract.Invariant(visitor.IsValidLogin, "ShoppingCartService CreateOrder visitor must be logged in!"); 

                //kosár tartalom lekérdezése
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCartToAdd = shoppingCartRepository.GetShoppingCart(request.CartId);

                Helpers.DesignByContract.Require((shoppingCartToAdd != null), "ShoppinCart cannot be null!");

                if (shoppingCartToAdd.Items.Count.Equals(0) || shoppingCartToAdd.Items[0] == null)
                {
                    response.Message = "A rendelés nem teljesíthető, mert nincs a kosárban termék!";

                    response.IsValidated = isValidated;

                    return response;
                }

                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddressList = customerRepository.GetDeliveryAddress(visitor.CustomerId, visitor.DataAreaId);

                CompanyGroup.Domain.PartnerModule.DeliveryAddress deliveryAddress = deliveryAddressList.Find(x => x.RecId.Equals(request.DeliveryAddressRecId));

                if (deliveryAddress == null)
                {
                    deliveryAddress = (deliveryAddressList.Count > 0) ? deliveryAddressList.First() : new CompanyGroup.Domain.PartnerModule.DeliveryAddress();
                }

                List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate> salesOrderLineCreateRequest = new List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>();

                //végig a kosár elemeken
                shoppingCartToAdd.Items.ToList().ForEach(x =>
                {
                    CompanyGroup.Domain.PartnerModule.ProductOrderCheck check = salesOrderRepository.GetProductOrderCheck(x.ProductId, x.DataAreaId, x.Quantity);

                    // -1;	-- nincs meg a cikk, nincs ConfigId
                    if (check.ResultCode == -1)
                    {
                        response.LineMessages.Add("Nincs ilyen cikk a rendszerben");
                    }
                    // -2;	-- nem webes a cikk
                    if (check.ResultCode == -2)
                    {
                        response.LineMessages.Add("Nincs ilyen cikk a webshopban");
                    }
                    // -3;	-- kifutott cikk 
                    if (check.ResultCode == -3)
                    {
                        response.LineMessages.Add("A cikk kifutott");
                    }
                    // -4;	-- kifuto cikk és nincs elegendő
                    if (check.ResultCode == -4)
                    {
                        if (check.AvailableQuantity > 0)
                        {
                            salesOrderLineCreateRequest.Add(new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate() 
                                                                { 
                                                                    ConfigId = x.ConfigId, 
                                                                    InventDimId = "", 
                                                                    ItemId = x.ProductId, 
                                                                    Qty = check.AvailableQuantity, 
                                                                    TaxItem = "" 
                                                                });

                            response.LineMessages.Add(String.Format("A {0} cikk kifutó, nincs belőle elegendő: {1} db", x.ProductId, x.Quantity));
                        }
                        else
                        {
                            response.LineMessages.Add(String.Format("A {0} cikk kifutó, nincs belőle elegendő: {1} db", x.ProductId, x.Quantity));
                        }
                    }
                    // -5;  -- cikk nem rendelhető 
                    if (check.ResultCode == -5)
                    {
                        response.LineMessages.Add(String.Format("A {0} cikk nem rendelhető", x.ProductId));
                    }
                    //sikeres tétel rendelés
                    if (check.ResultCode > 0)
                    {
                        salesOrderLineCreateRequest.Add(new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate()
                        {
                            ConfigId = x.ConfigId,
                            InventDimId = "",
                            ItemId = x.ProductId,
                            Qty = x.Quantity,
                            TaxItem = ""
                        });

                        response.LineMessages.Add(String.Format("A {0} cikk rendelése sikeresen megtörtént: {1} db", x.ProductId, x.Quantity));
                    }
                }
                );

                #region "HRP - BSC rendelés feladása"

                int year = request.DeliveryDate.Year == 1 ? DateTime.Now.Year : request.DeliveryDate.Year;

                int month = request.DeliveryDate.Month == 1 ? DateTime.Now.Month : request.DeliveryDate.Month;

                int day = request.DeliveryDate.Day == 1 ? DateTime.Now.Day : request.DeliveryDate.Day;

                CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult salesOrderCreateResultHrp = null;

                List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem> hrpLines = shoppingCartToAdd.GetItemsByDataAreaId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                //HRP AX rendelés összeállítás
                if (hrpLines.Count > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = visitor.CustomerName,
                        DeliveryDate = String.Format("{0}-{1}-{2}", year, month, day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = visitor.PersonName,
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = CompanyGroup.Domain.Core.Constants.OuterStockHrp,
                        Lines = hrpLines.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
                        {
                            return new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate() { ConfigId = x.ConfigId, InventDimId = "", ItemId = x.ProductId, Qty = x.Quantity, TaxItem = "" };
                        }
                        ),
                        Message = request.CustomerOrderNote,
                        PartialDelivery = true,
                        RequiredDelivery = request.DeliveryRequest,
                        SalesSource = 1,
                        Transporter = ""
                    };

                    //összeállított rendelés elküldése AX-be
                    salesOrderCreateResultHrp = salesOrderRepository.Create(salesOrderCreate);
                }

                CompanyGroup.Domain.PartnerModule.SalesOrderCreateResult salesOrderCreateResultBsc = null;

                List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem> bscLines = shoppingCartToAdd.GetItemsByDataAreaId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);

                //BSC AX rendelés összeállítás
                if (bscLines.Count > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = "",
                        DeliveryDate = String.Format("{0}-{1}-{2}", year, month, day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = "",
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = CompanyGroup.Domain.Core.Constants.OuterStockBsc,
                        Lines = bscLines.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
                        {
                            return new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate() { ConfigId = x.ConfigId, InventDimId = "", ItemId = x.ProductId, Qty = x.Quantity, TaxItem = "" };
                        }
                        ),
                        Message = "",
                        PartialDelivery = true,
                        RequiredDelivery = request.DeliveryRequest,
                        SalesSource = 1,
                        Transporter = ""
                    };

                    //összeállított rendelés elküldése AX-be
                    salesOrderCreateResultBsc = salesOrderRepository.Create(salesOrderCreate);
                }

                #endregion

                string salesOrderCreateSecondHandResultHrp = String.Empty;

                List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem> secondHandItemsHrp = shoppingCartToAdd.GetSecondHandItemsByDataAreaId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                if (secondHandItemsHrp.Count > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreateSecondHandHrp = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = "",
                        DeliveryDate = String.Format("{0}-{1}-{2}", year, month, day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = "",
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = CompanyGroup.Domain.Core.Constants.OuterStockHrp,
                        Lines = secondHandItemsHrp.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
                        {
                            return new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate() { ConfigId = x.ConfigId, InventDimId = "", ItemId = x.ProductId, Qty = x.Quantity, TaxItem = "" };
                        }
                        ),
                        Message = "",
                        PartialDelivery = true,
                        RequiredDelivery = request.DeliveryRequest,
                        SalesSource = 1,
                        Transporter = ""
                    };

                    //összeállított rendelés elküldése AX-be
                    salesOrderCreateSecondHandResultHrp = salesOrderRepository.CreateSecondHandOrder(salesOrderCreateSecondHandHrp);

                }

                string salesOrderCreateSecondHandResultBsc = String.Empty;

                List<CompanyGroup.Domain.WebshopModule.ShoppingCartItem> secondHandItemsBsc = shoppingCartToAdd.GetSecondHandItemsByDataAreaId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);

                if (secondHandItemsBsc.Count > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreateSecondHandBsc = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = "",
                        DeliveryDate = String.Format("{0}-{1}-{2}", year, month, day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = "",
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = CompanyGroup.Domain.Core.Constants.OuterStockHrp,
                        Lines = secondHandItemsBsc.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
                        {
                            return new CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate() { ConfigId = x.ConfigId, InventDimId = "", ItemId = x.ProductId, Qty = x.Quantity, TaxItem = "" };
                        }
                        ),
                        Message = "",
                        PartialDelivery = true,
                        RequiredDelivery = request.DeliveryRequest,
                        SalesSource = 1,
                        Transporter = ""
                    };

                    //összeállított rendelés elküldése AX-be
                    salesOrderCreateSecondHandResultBsc = salesOrderRepository.CreateSecondHandOrder(salesOrderCreateSecondHandBsc);

                }

                //ha volt mit rendelni
                isValidated = salesOrderLineCreateRequest.Count > 0;

                //TODO:   
                CompanyGroup.Domain.WebshopModule.Shipping shipping = new CompanyGroup.Domain.WebshopModule.Shipping() 
                                                                          { 
                                                                              AddrRecId = request.DeliveryAddressRecId, 
                                                                              City = deliveryAddress.City,
                                                                              Country = deliveryAddress.CountryRegionId, 
                                                                              DateRequested = request.DeliveryDate, 
                                                                              InvoiceAttached = false,
                                                                              Street = deliveryAddress.Street,
                                                                              ZipCode = deliveryAddress.ZipCode 
                                                                          };

                //kosár beállítása elküldött státuszba
                shoppingCartRepository.Post(shoppingCartToAdd);

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(newShoppingCart);

                //aktív kosár frissítése kiolvasással
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(newCartId);

                //felhasználóhoz tartozó kosár fejléccek lekérdezése 
                List<CompanyGroup.Domain.WebshopModule.ShoppingCartHeader> shoppingCartHeaders = shoppingCartRepository.GetShoppingCartHeaders(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartHeaderCollection shoppingCartHeaderCollection = new Domain.WebshopModule.ShoppingCartHeaderCollection(shoppingCartHeaders);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartHeaderCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartHeaderCollection);

                response.ActiveCart = new ShoppingCartToShoppingCart().Map(shoppingCart);
                response.OpenedItems = storedOpenedShoppingCarts.OpenedItems;
                response.StoredItems = storedOpenedShoppingCarts.StoredItems;
                response.LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions);
                response.Created = true;
                response.WaitForAutoPost = false;
                response.Message = String.Format("HRP: {0}  BSC: {1} HRP használt: {2} BSC használt: {3}", (salesOrderCreateResultHrp != null) ? salesOrderCreateResultHrp.SalesId : "", (salesOrderCreateResultBsc != null) ? salesOrderCreateResultBsc.SalesId : "", !String.IsNullOrEmpty(salesOrderCreateSecondHandResultHrp) ? salesOrderCreateSecondHandResultHrp : "", !String.IsNullOrEmpty(salesOrderCreateSecondHandResultBsc) ? salesOrderCreateSecondHandResultBsc : "");
                response.IsValidated = (salesOrderCreateResultHrp != null) || (salesOrderCreateResultBsc != null) || (!String.IsNullOrEmpty(salesOrderCreateSecondHandResultHrp)) || (!String.IsNullOrEmpty(salesOrderCreateSecondHandResultBsc));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
