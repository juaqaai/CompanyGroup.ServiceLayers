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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.ServiceRequest.AssociateCart request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //visitor lekérdezés
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(0, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(newShoppingCart);

                //beállítja az új VisitorId-t, a permanens helyett
                shoppingCartRepository.AssociateCart(request.PermanentId, request.VisitorId);

                //aktív státusz beállítása
                shoppingCartRepository.SetActive(newCartId, request.VisitorId);

                //kosárlista frissítése kiolvasással
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                //válaszüzenet előállítása
                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //aktív kosár kiolvasása
                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //leasing kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                           { 
                                                                               ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart), 
                                                                               OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                               StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                               LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                                                                               //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                           };

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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request)
        {
            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //lehetséges-e az új kosár létrehozása? ellenörzés, hogy elértük-e a kosár limitek számát?

                //kosár név kalkulálása
                string cartName = String.IsNullOrEmpty(request.CartName) ? ShoppingCartService.CreateCartName() : request.CartName;

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(0, request.VisitorId, visitor.CustomerId, visitor.PersonId, cartName, visitor.Currency, true);

                int newCartId = shoppingCartRepository.Add(shoppingCart);

                shoppingCartRepository.SetActive(newCartId, request.VisitorId);

                //kosarak lista frissítése, válaszüzenet generálása
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCarts = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCarts);

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart = new ShoppingCartToShoppingCart().Map(shoppingCartCollection.GetActiveCart());

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               { 
                                                                                   ActiveCart = activeCart, 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new Dto.WebshopModule.LeasingOptions(), 
                                                                                   //FinanceOffer = new Dto.WebshopModule.FinanceOffer()
                                                                               };

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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo SaveCart(CompanyGroup.Dto.ServiceRequest.SaveCart request)
        {
            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Name), "Name cannot be null, or empty!");

            //Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "CartId cannot be null, or empty!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //nincs kosár azonosító
                if (request.CartId == 0)
                {
                    //kosár név kalkulálása
                    string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;

                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, cartName, visitor.Currency, true);

                    int newCartId = shoppingCartRepository.Add(shoppingCart);

                    shoppingCartRepository.Store(newCartId, ShoppingCartService.CreateCartName());
                }
                else
                {
                    string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;
                    
                    shoppingCartRepository.Store(request.CartId, cartName);
                }

                //kosár lista frissítése, válaszüzenet elkészítése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               { 
                                                                                   ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart), 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                                                                                   //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                               };

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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo RemoveCart(CompanyGroup.Dto.ServiceRequest.RemoveCart request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "Visitor id cannot be null!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                shoppingCartRepository.Remove(request.CartId);

                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //a törlést követően is van kosárelem
                if (shoppingCartCollection.ExistsItem)
                {
                    //nincs aktív kosár
                    if (shoppingCartCollection.GetActiveCart() == null)
                    {
                        CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        shoppingCartRepository.SetActive(firstCart.Id, request.VisitorId);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                    int newCartId = shoppingCartRepository.Add(newShoppingCart);

                    shoppingCartRepository.SetActive(newCartId, request.VisitorId);
                }

                //kosár lista frissítése, válaszüzenet elkészítése
                shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               {
                                                                                   ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart), 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                                                                                   //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                               };
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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.ServiceRequest.ActivateCart request)
        {
            //ellenörzés, visitorId-re, productId-re, DataAreaId-re
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //active-ra állítás
                shoppingCartRepository.SetActive(request.CartId, request.VisitorId);

                //kosárlista frissítése, válaszüzenet elkészítése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCarts = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCarts);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               { 
                                                                                   ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart), 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                                                                                   //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                               };
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
            return String.Format("Kosár {0}.{1}.{2} - {3} : {4} : {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        #region "kosár sor műveletek"

        /// <summary>
        /// új elem hozzáadása az aktív kosárhoz, 
        /// ha nincs aktív kosár, akkor létrehoz egy új kosarat és ahhoz adja hozzá az új elemet
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions AddLine(CompanyGroup.Dto.ServiceRequest.AddLine request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.DataAreaId), "DataAreaId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.ProductId), "Product cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require(request.Quantity > 0, "Quantity must be greather than zero!");

            Helpers.DesignByContract.Require((request.CartId > 0), "CartId cannot be null, or empty!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId); 

                //ha nincs meg a CartId, akkor új kosár létrehozása és hozzáadása szükséges
                if (shoppingCart == null)
                {
                    shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                    shoppingCartRepository.Add(shoppingCart);
                }

                Helpers.DesignByContract.Invariant(shoppingCart != null, "ShoppingCart cannot be null!");

                //termék lekérdezés, vizsgálat
                CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(request.ProductId, request.DataAreaId);

                Helpers.DesignByContract.Invariant(product != null, "productRepository.GetItem result cannot be null!");

                //új shoppingCartItem létrehozása, inicializálás
                CompanyGroup.Domain.WebshopModule.ShoppingCartItem shoppingCartItem = new Domain.WebshopModule.ShoppingCartItem();

                shoppingCartItem.SetProduct(product);

                shoppingCartItem.CartId = shoppingCart.Id;

                shoppingCartItem.Quantity = request.Quantity;

                //mi van, ha létezik a termék a kosárban? 
                if (shoppingCart.IsInCart(request.ProductId))
                {
                    //kosár elem módosítás
                    shoppingCartRepository.UpdateLineQuantity(shoppingCart.Id, request.Quantity);
                }
                else
                {
                    //hozzáadás kosárhoz
                    shoppingCartRepository.AddLine(shoppingCartItem);
                }

                CompanyGroup.Domain.WebshopModule.ShoppingCart cart = shoppingCartRepository.GetShoppingCart(shoppingCart.Id);

                //finanszírozandó összeg
                int financedAmount = cart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = cart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(cart, leasingOptions);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLine request)
        {
            //ellenörzés 
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null!");

            Helpers.DesignByContract.Require((request.LineId > 0), "LineId cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //sor törlése
                shoppingCartRepository.RemoveLine(request.LineId);

                //kosár olvasása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //finanszírozandó összeg
                int financedAmount = shoppingCart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(shoppingCart, leasingOptions);

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
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity request)
        {
            //ellenörzés, 
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            Helpers.DesignByContract.Require((request.LineId > 0), "Line id cannot be null!");

            try
            {
                shoppingCartRepository.UpdateLineQuantity(request.LineId, request.Quantity);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                //finanszírozandó összeg
                int financedAmount = shoppingCart.SumTotal;

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = shoppingCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = new ShoppingCartAndLeasingOptionsToShoppingCartAndLeasingOptions().Map(shoppingCart, leasingOptions);

                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "kosár lekérdezések"

        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetShoppingCartInfo(CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                Helpers.DesignByContract.Invariant((request.CartId > 0), "CartId cannot be null!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart;

                if (shoppingCartCollection.Carts.Exists(x => x.Id.Equals(request.CartId)))
                {
                    activeCart = shoppingCartRepository.GetShoppingCart(request.CartId);
                }
                else
                {
                    activeCart = shoppingCartCollection.GetActiveCart();
                }

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo()
                                                                               {
                                                                                   ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart),
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems,
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions) 
                                                                                   //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                               };
                return response;
            }
            catch
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo()
                           {
                               ActiveCart = new Dto.WebshopModule.ShoppingCart()
                                            {
                                                DeliveryTerms = 0,
                                                Id = 0,
                                                Items = new List<Dto.WebshopModule.ShoppingCartItem>(),
                                                PaymentTerms = 0,
                                                Shipping = new Dto.WebshopModule.Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.MinValue, InvoiceAttached = false, Street = "", ZipCode = "" },
                                                SumTotal = 0
                                            },
                               OpenedItems = new List<Dto.WebshopModule.OpenedShoppingCart>(),
                               StoredItems = new List<Dto.WebshopModule.StoredShoppingCart>(), 
                               //FinanceOffer = new Dto.WebshopModule.FinanceOffer(), 
                               LeasingOptions = new Dto.WebshopModule.LeasingOptions()
                           };
                return response;
            }
        }

        /// <summary>
        /// céghez, azon belül személyhez tartozó érvényes / tárolt kosarak kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection GetStoredOpenedShoppingCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                Helpers.DesignByContract.Invariant(!String.IsNullOrWhiteSpace(visitor.CustomerId), "CompanyId id cannot be null!");

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                return new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection(); }
        }

        /// <summary>
        /// felhasználóhoz tartozó érvényes kosarak kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartCollection GetCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                return new ShoppingCartCollectionToShoppingCartCollection().Map(shoppingCartCollection);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartCollection(); }
        }

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKey request)
        {
            try
            {
                Helpers.DesignByContract.Require((request.CartId > 0), "CartId cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetShoppingCart(request.CartId);

                CompanyGroup.Dto.WebshopModule.ShoppingCart result = new ShoppingCartToShoppingCart().Map(shoppingCart);

                return result;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCart(); }
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetActiveCart(CompanyGroup.Dto.ServiceRequest.GetActiveCart request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               { 
                                                                                   ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart), 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions) 
                                                                                   //FinanceOffer = new FinanceOfferToFinanceOffer().Map(new CompanyGroup.Domain.WebshopModule.FinanceOffer())
                                                                               };
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
        public CompanyGroup.Dto.WebshopModule.OrderFulFillment CreateOrder(CompanyGroup.Dto.ServiceRequest.SalesOrderCreate request)
        {
            CompanyGroup.Dto.WebshopModule.OrderFulFillment response = new CompanyGroup.Dto.WebshopModule.OrderFulFillment();

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //kosár tartalom lekérdezése
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCartToAdd = shoppingCartRepository.GetShoppingCart(request.CartId);

                Helpers.DesignByContract.Require((shoppingCartToAdd != null), "ShoppinCart cannot be null!");

                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddressList = customerRepository.GetDeliveryAddress(visitor.CustomerId, visitor.DataAreaId);

                CompanyGroup.Domain.PartnerModule.DeliveryAddress deliveryAddress = deliveryAddressList.Find(x => x.RecId.Equals(request.DeliveryAddressRecId));

                if (deliveryAddress == null)
                {
                    deliveryAddress = (deliveryAddressList.Count > 0) ? deliveryAddressList.First() : new CompanyGroup.Domain.PartnerModule.DeliveryAddress();
                }

                List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate> salesOrderLineCreateRequest = new List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>();

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

                //HRP AX rendelés összeállítás
                if (shoppingCartToAdd.ItemCountByDataAreaId(DataAreaId.Hrp) > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = "",
                        DeliveryDate = String.Format("{0}-{1}-{2}", request.DeliveryDate.Year, request.DeliveryDate.Month, request.DeliveryDate.Day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = "",
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = request.DeliveryRequest ? CompanyGroup.Domain.Core.Constants.OuterStockHrp : CompanyGroup.Domain.Core.Constants.InnerStockHrp,
                        Lines = shoppingCartToAdd.Items.ToList().ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
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
                    salesOrderRepository.Create(salesOrderCreate);
                }

                //BSC AX rendelés összeállítás
                if (shoppingCartToAdd.ItemCountByDataAreaId(DataAreaId.Bsc) > 0)
                {
                    CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                    {
                        ContactPersonId = visitor.PersonId,
                        CustomerId = visitor.CustomerId,
                        DataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp, //visitor.DataAreaId
                        DeliveryCity = deliveryAddress.City,
                        DeliveryCompanyName = "",
                        DeliveryDate = String.Format("{0}-{1}-{2}", request.DeliveryDate.Year, request.DeliveryDate.Month, request.DeliveryDate.Day),
                        DeliveryEmail = "",
                        DeliveryId = "",
                        DeliveryPersonName = "",
                        DeliveryPhone = "",
                        DeliveryStreet = deliveryAddress.Street,
                        DeliveryZip = deliveryAddress.ZipCode,
                        InventLocationId = request.DeliveryRequest ? CompanyGroup.Domain.Core.Constants.OuterStockBsc : CompanyGroup.Domain.Core.Constants.InnerStockBsc,
                        Lines = shoppingCartToAdd.Items.ToList().ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
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
                    salesOrderRepository.Create(salesOrderCreate);
                }

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

                //ha van még a felhasználónak kosara, akkor valamelyiket aktiválni kell, ha nincs, akkor létre kell hozni
                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //az elküldést követően is van kosárelem
                if (shoppingCartCollection.ExistsItem)
                {
                    //nincs aktív kosár
                    if (shoppingCartCollection.GetActiveCart() == null)
                    {
                        CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        shoppingCartRepository.SetActive(firstCart.Id, request.VisitorId);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.CartId, request.VisitorId, visitor.CustomerId, visitor.PersonId, ShoppingCartService.CreateCartName(), visitor.Currency, true);

                    shoppingCartRepository.Add(newShoppingCart);
                }

                //kosárlista frissítése kiolvasással
                shoppingCartList = shoppingCartRepository.GetCartCollection(request.VisitorId);

                //válaszüzenet előállítása
                shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                response = new CompanyGroup.Dto.WebshopModule.OrderFulFillment()
                {
                    ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart),
                    OpenedItems = storedOpenedShoppingCarts.OpenedItems,
                    StoredItems = storedOpenedShoppingCarts.StoredItems,
                    LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                    Created = false,
                    WaitForAutoPost = true,     
                    Message = ""
                };

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
