﻿using System;
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

        #region "Finance leasing műveletek (GetMinMaxLeasingValue)"

        private const string CACHEKEY_MINMAXLEASINGVALUE = "MinMaxLeasingValue";

        private const double CACHE_EXPIRATION_MINMAXLEASINGVALUE = 3600d;

        /// <summary>
        /// tartós bérlet számítás legkissebb és legnagyobb érték lekérdezése
        /// </summary>
        /// <returns></returns>
        private CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue GetMinMaxLeasingValue()
        {
            CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue result = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue>(CACHEKEY_MINMAXLEASINGVALUE);

            if (result == null)
            {
                result = this.financeRepository.GetMinMaxLeasingValues();

                CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue>(CACHEKEY_MINMAXLEASINGVALUE, result, DateTime.Now.AddMinutes(CACHE_EXPIRATION_MINMAXLEASINGVALUE));
            }

            return result;
        }

        #endregion

        #region "kosár műveletek"

        /// <summary>
        /// új visitorId beállítása a régi helyére, ha a kosár mentett státuszú, 
        /// hozzáad egy nyitott kosarat a felhasználó kosaraihoz
        /// 1. permanens visitorId alapján a kosarak kiolvasása
        /// 2. végig az előző eredménylistán, és az új visitorId beállítása a permanensId helyére
        /// 3. kosarak lista visszaadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>CompanyGroup.Dto.ServiceResponse
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.ServiceRequest.AssociateCart request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //visitor lekérdezés
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, ShoppingCartService.CreateCartName(), true);

                shoppingCartRepository.Add(newShoppingCart);

                // mentett kosarak lista kiolvasása
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = (!String.IsNullOrEmpty(request.PermanentId)) ? shoppingCartRepository.GetCartCollectionByVisitor(request.PermanentId, true) : new List<CompanyGroup.Domain.WebshopModule.ShoppingCart>();

                if (shoppingCartList.Count > 0)
                {
                    //beállítja az új VisitorId-t, a permanens helyett
                    shoppingCartList.ForEach(x => shoppingCartRepository.AssociateCart(x.Id, request.VisitorId));

                    //aktív státusz hamis értékre állítása
                    shoppingCartList.ForEach(x => shoppingCartRepository.SetActive(x.Id, false));
                }

                //kosárlista frissítése kiolvasással
                shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
                                                                               FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
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
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //céghez és ha van személyhez tartozó kosarak lista elkérése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCarts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCarts);

                //lehetséges-e az új kosár létrehozása? ellenörzés, hogy elértük-e a kosár limitek számát?

                //kosár elemek deaktiválása
                if (shoppingCartCollection.ExistsItem)
                {
                    //összes meglévő kosár active = false beállítása
                    shoppingCarts.ForEach(x => shoppingCartRepository.SetActive(x.Id, false));
                }

                //kosár név kalkulálása
                string cartName = String.IsNullOrEmpty(request.CartName) ? ShoppingCartService.CreateCartName() : request.CartName;

                //új kosár létrehozása és hozzáadása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, cartName, true);

                shoppingCartRepository.Add(shoppingCart);

                //kosarak lista frissítése, válaszüzenet generálása
                shoppingCarts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCarts);

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart = new ShoppingCartToShoppingCart().Map(shoppingCartCollection.GetActiveCart());

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo() 
                                                                               { 
                                                                                   ActiveCart = activeCart, 
                                                                                   OpenedItems = storedOpenedShoppingCarts.OpenedItems, 
                                                                                   StoredItems = storedOpenedShoppingCarts.StoredItems, 
                                                                                   LeasingOptions = new Dto.WebshopModule.LeasingOptions(), 
                                                                                   FinanceOffer = new Dto.WebshopModule.FinanceOffer()
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
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = null;

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = null;

                //nincs kosár azonosító
                if (String.IsNullOrEmpty(request.CartId))
                {
                    //kosár név kalkulálása
                    string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;

                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, cartName, true);

                    shoppingCartRepository.Add(shoppingCart);

                    shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                    shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                    CompanyGroup.Domain.WebshopModule.ShoppingCart cart = shoppingCartCollection.GetActiveCart();

                    shoppingCartRepository.Store(cart.Id, ShoppingCartService.CreateCartName());
                }
                else
                {
                    string cartName = String.IsNullOrEmpty(request.Name) ? ShoppingCartService.CreateCartName() : request.Name;
                    
                    shoppingCartRepository.Store(request.CartId, cartName);
                }

                //kosár lista frissítése, válaszüzenet elkészítése
                shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

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
                                                                                   FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
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

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                shoppingCartRepository.Remove(request.CartId);

                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //a törlést követően is van kosárelem
                if (shoppingCartCollection.ExistsItem)
                {
                    //nincs aktív kosár
                    if (shoppingCartCollection.GetActiveCart() == null)
                    {
                        CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        shoppingCartRepository.SetActive(firstCart.Id, true);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, ShoppingCartService.CreateCartName(), true);

                    shoppingCartRepository.Add(newShoppingCart);                    
                }

                //kosár lista frissítése, válaszüzenet elkészítése
                shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
                                                                                   FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
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

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCarts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                //összes kosár státuszának active = false -ra állítása
                shoppingCarts.ForEach(x => shoppingCartRepository.SetActive(x.Id, false));

                //active-ra állítás
                shoppingCartRepository.SetActive(request.CartId, true);

                //kosárlista frissítése, válaszüzenet elkészítése
                shoppingCarts = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
                                                                                   FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
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

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "CartId cannot be null, or empty!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetCartByKey(request.CartId); 

                //ha nincs meg a CartId, akkor új kosár létrehozása és hozzáadása szükséges
                if (shoppingCart == null)
                {
                    shoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, ShoppingCartService.CreateCartName(), true);

                    shoppingCartRepository.Add(shoppingCart);
                }

                Helpers.DesignByContract.Invariant(shoppingCart != null, "ShoppingCart cannot be null!");

                //termék lekérdezés, vizsgálat
                CompanyGroup.Domain.WebshopModule.Product product = productRepository.GetItem(request.ProductId, request.DataAreaId);

                Helpers.DesignByContract.Invariant(product != null, "productRepository.GetItem result cannot be null!");

                //új shoppingCartItem létrehozása, inicializálás
                CompanyGroup.Domain.WebshopModule.ShoppingCartItem shoppingCartItem = new Domain.WebshopModule.ShoppingCartItem();

                shoppingCartItem.SetProduct(product);

                shoppingCartItem.Id = shoppingCart.Id;

                shoppingCartItem.Quantity = request.Quantity;

                //mi van, ha létezik a termék a kosárban? 
                bool existsProductInCart = shoppingCartRepository.ExistsProductInCart(shoppingCart.Id, request.ProductId);

                if (existsProductInCart)
                {
                    //kosár elem módosítás
                    shoppingCartRepository.UpdateLineQuantity(shoppingCart.Id, request.ProductId, request.Quantity);
                }
                else
                {
                    //hozzáadás kosárhoz
                    shoppingCartRepository.AddLine(shoppingCartItem);
                }

                CompanyGroup.Domain.WebshopModule.ShoppingCart cart = shoppingCartRepository.GetCartByKey(shoppingCart.Id);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(cart.SumTotal);

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
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.ProductId), "Productart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //céghez, azon belül személyhez kapcsolódó kosár lista kiolvasása 
                //List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                //CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartRepository.GetCartByKey(request.CartId); //shoppingCartCollection.GetActiveCart();

                //Helpers.DesignByContract.Invariant(activeCart != null, "Active cart cannot be null!");

                //sor törlése
                shoppingCartRepository.RemoveLine(request.CartId, request.ProductId);

                //kosár olvasása
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetCartByKey(request.CartId);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

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

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.ProductId), "Productart id cannot be null!");

            try
            {
                shoppingCartRepository.UpdateLineQuantity(request.CartId, request.ProductId, request.Quantity);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetCartByKey(request.CartId);

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(shoppingCart.SumTotal);

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

                Helpers.DesignByContract.Invariant(!String.IsNullOrWhiteSpace(request.CartId), "CartId cannot be null!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart;

                if (shoppingCartCollection.Carts.Exists(x => x.Id.Equals(request.CartId)))
                {
                    activeCart = shoppingCartRepository.GetCartByKey(request.CartId);
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
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                   FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
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
                                                Id = "",
                                                Items = new List<Dto.WebshopModule.ShoppingCartItem>(),
                                                PaymentTerms = 0,
                                                Shipping = new Dto.WebshopModule.Shipping() { AddrRecId = 0, City = "", Country = "", DateRequested = DateTime.MinValue, InvoiceAttached = false, Street = "", ZipCode = "" },
                                                SumTotal = 0
                                            },
                               OpenedItems = new List<Dto.WebshopModule.OpenedShoppingCart>(),
                               StoredItems = new List<Dto.WebshopModule.StoredShoppingCart>(), 
                               FinanceOffer = new Dto.WebshopModule.FinanceOffer(), 
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

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                Helpers.DesignByContract.Invariant(!String.IsNullOrWhiteSpace(visitor.CompanyId), "CompanyId id cannot be null!");

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "CartId cannot be null, or empty!");

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = shoppingCartRepository.GetCartByKey(request.CartId);

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

                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
                                                                                   LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                                                                                   FinanceOffer = new FinanceOfferToFinanceOffer().Map(activeCart.FinanceOffer)
                                                                               };
                return response;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.ShoppingCartInfo(); }
        }

        #endregion

        #region "finance műveletek"

        private static readonly string OfferingMailSubject = Helpers.ConfigSettingsParser.GetString("OfferingMailSubject", "HRP Finance ajánlatkérés üzenet");

        private static readonly string OfferingMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("OfferingMailHtmlTemplateFile", "offering.html");

        private static readonly string OfferingMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("OfferingMailTextTemplateFile", "offering.txt");

        private static readonly string OfferingMailToAddress = Helpers.ConfigSettingsParser.GetString("OfferingMailToAddress", "jverebelyi@hrp.hu"); //berlet@hrpfinance.hu

        private static readonly string OfferingMailToName = Helpers.ConfigSettingsParser.GetString("OfferingMailToName", "HRP Finance");

        private static readonly string OfferingMailCcAddress = Helpers.ConfigSettingsParser.GetString("OfferingMailCcAddress", "ajuhasz@hrp.hu"); //rtokes

        private static readonly string OfferingMailCcName = Helpers.ConfigSettingsParser.GetString("OfferingMailCcName", "Tőkés Róbert");

        private static readonly string OfferingMailSmtpHost = Helpers.ConfigSettingsParser.GetString("OfferingMailSmtpHost", "195.30.7.14");

        private const string OfferItemHtml = "<tr>\n" +
                                             "<td>Termékazonosító:</td>\n" +
                                             "<td>$ProductId$</td>\n" +
                                             "</tr>\n" + 
                                             "<tr>\n" +
                                             "<td>Termék neve:</td>\n" +
                                             "<td>$ProductName$</td>\n" +
                                             "</tr>\n" +
                                             //"<tr>\n" +
                                             //    "<td>Gyártó:</td>\n" +
                                             //    "<td>$Manufacturer$</td>\n" +
                                             //"</tr>\n" +
                                             //"<tr>\n" +
                                             //    "<td>Jelleg1:</td>\n" +
                                             //    "<td>$Category1$</td>\n" +
                                             //"</tr>\n" +
                                             "<tr>\n" +
                                                 "<td>Darabszám:</td>\n" +
                                                 "<td>$Quantity$</td>\n" +
                                             "</tr>\n" +
                                             "<tr>\n" +
                                                 "<td>Nettó vételára:</td>\n" +
                                                 "<td>$Price$ Ft</td>\n" +
                                             "</tr>\n" +
                                             "<tr>\n" +
                                                 "<td colspan=\"2\"><hr/></td>\n" +
                                             "</tr>\n";

        private const string OfferItemTxt = "\n" +
                                            "Termékazonosító: $ProductId$ \n" +
                                            "Termék neve: $ProductName$ \n" +
                                            //"Gyártó: $Manufacturer$ \n" +
                                            //"Jelleg1: $Category1$ \n" +
                                            "Darabszám: $Quantity$ \n" +
                                            "Nettó vételára: $Price$ Ft \n" +
                                            "________________________________________\n\n";
        /// <summary>
        /// finanszírozási ajánlat levél txt template
        /// </summary>
        private static string PlainText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + OfferingMailTextTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat levél html template
        /// </summary>
        private static string HtmlText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + OfferingMailHtmlTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //kosár tartalom lekérdezése, levélküldés
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCartToAdd = shoppingCartRepository.GetCartByKey(request.CartId);
                
                Helpers.DesignByContract.Require((shoppingCartToAdd != null), "ShoppinCart cannot be null!");

                //levélküldés
                bool sendSuccess = SendFinanceOfferMail(request, shoppingCartToAdd, visitor);

                //kosár beállítása finanszírozásra elküldött státuszba, ajánlathoz szükséges adatok mentése
                shoppingCartRepository.PostFinanceOffer(request.CartId, new Domain.WebshopModule.FinanceOffer() 
                                                                            { 
                                                                                Address = request.Address, 
                                                                                NumOfMonth = request.NumOfMonth, 
                                                                                PersonName = request.PersonName, 
                                                                                Phone = request.Phone, 
                                                                                StatNumber = request.StatNumber 
                                                                            });

                //ha van még a felhasználónak kosara, akkor valamelyiket aktiválni kell, ha nincs, akkor létre kell hozni
                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //az elküldést követően is van kosárelem
                if (shoppingCartCollection.ExistsItem)
                {
                    //nincs aktív kosár
                    if (shoppingCartCollection.GetActiveCart() == null)
                    {
                        CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        shoppingCartRepository.SetActive(firstCart.Id, true);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, ShoppingCartService.CreateCartName(), true);

                    shoppingCartRepository.Add(newShoppingCart);
                }

                //kosárlista frissítése kiolvasással
                shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                //válaszüzenet előállítása
                shoppingCartCollection = new Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                CompanyGroup.Domain.WebshopModule.ShoppingCart activeCart = shoppingCartCollection.GetActiveCart();

                //finanszírozandó összeg
                int financedAmount = Convert.ToInt32(activeCart.SumTotal);

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(financedAmount);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new CompanyGroup.Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = activeCart.SumTotal;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment response = new CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment()
                {
                    ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart),
                    OpenedItems = storedOpenedShoppingCarts.OpenedItems,
                    StoredItems = storedOpenedShoppingCarts.StoredItems,
                    LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions), 
                    EmaiNotification = sendSuccess, 
                    Message = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// finance ajánlatkérés levélküldés 
        /// </summary>
        /// <param name="financeOffer"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        private bool SendFinanceOfferMail(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer financeOffer, 
                                          CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart,
                                          CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                double financedAmount = 0;

                string offerItemsHtml = String.Empty;
                string offerItemsTxt = String.Empty;

                shoppingCart.Items.ForEach(
                    delegate(CompanyGroup.Domain.WebshopModule.ShoppingCartItem line)
                    {
                        financedAmount += (line.Quantity * line.CustomerPrice);

                        offerItemsHtml += ShoppingCartService.OfferItemHtml.Replace("$ProductId$", line.ProductId)
                                                                    //.Replace("$Manufacturer$", line.Structure.Manufacturer.ManufacturerName)
                                                                    //.Replace("$Category1$", line.Structure.Category1.CategoryName)
                                                                    .Replace("$ProductName$", line.ProductName)
                                                                    .Replace("$Quantity$", Helpers.ConvertData.ConvertIntToString(line.Quantity))
                                                                    .Replace("$Price$", Convert.ToString(line.CustomerPrice));

                        offerItemsTxt += ShoppingCartService.OfferItemTxt.Replace("$ProductId$", line.ProductId)
                                                                    //.Replace("$Manufacturer$", line.Structure.Manufacturer.ManufacturerName)
                                                                    //.Replace("$Category1$", line.Structure.Category1.CategoryName)
                                                                    .Replace("$ProductName$", line.ProductName)
                                                                    .Replace("$Quantity$", Helpers.ConvertData.ConvertIntToString(line.Quantity))
                                                                    .Replace("$Price$", Convert.ToString(line.CustomerPrice));
                    });

                string tmpHtml = ShoppingCartService.HtmlText;
                string html = tmpHtml.Replace("$PersonName$", financeOffer.PersonName)
                                        .Replace("$Address$", financeOffer.Address)
                                        .Replace("$Phone$", financeOffer.Phone)
                                        .Replace("$StatNumber$", financeOffer.StatNumber)
                                        .Replace("$NumOfMonth$", Helpers.ConvertData.ConvertIntToString(financeOffer.NumOfMonth))
                                        .Replace("$FinancedAmount$", Convert.ToString(financedAmount))
                                        .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                        .Replace("$OfferItems$", offerItemsHtml)
                                        .Replace("$CustName$", visitor.CompanyName)
                                        .Replace("$CustPersonName$", visitor.PersonName)
                                        .Replace("$CustPhone$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "")
                                        .Replace("$CustEmail$", String.IsNullOrEmpty(visitor.PersonName) ? "" : visitor.PersonName);

                string tmpPlain = ShoppingCartService.PlainText;
                string plain = tmpPlain.Replace("$PersonName$", financeOffer.PersonName)
                                        .Replace("$Address$", financeOffer.Address)
                                        .Replace("$Phone$", financeOffer.Phone)
                                        .Replace("$StatNumber$", financeOffer.StatNumber)
                                        .Replace("$NumOfMonth$", Helpers.ConvertData.ConvertIntToString(financeOffer.NumOfMonth))
                                        .Replace("$FinancedAmount$", Convert.ToString(financedAmount))
                                        .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                        .Replace("$OfferItems$", offerItemsHtml)
                                        .Replace("$CustName$", visitor.CompanyName)
                                        .Replace("$CustPersonName$", visitor.PersonName)
                                        .Replace("$CustPhone$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "")
                                        .Replace("$CustEmail$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "");

                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage(OfferingMailSubject, plain, html);

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
                mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + OfferingMailToAddress + ">", OfferingMailToName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, "<webadmin@hrp.hu>", "web adminisztrátor csoport", System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.CC, "<" + OfferingMailCcAddress + ">", OfferingMailCcName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, "<ajuhasz@hrp.hu>", "Juhász Attila", System.Text.Encoding.Default));

                //mail sender
                MailMergeLib.MailMergeSender mailSender = new MailMergeLib.MailMergeSender();

                //esemenykezelok beallitasa, ha van
                mailSender.OnSendFailure += new EventHandler<MailMergeLib.MailSenderSendFailureEventArgs>(delegate(object obj, MailMergeLib.MailSenderSendFailureEventArgs args)
                //( ( obj, args ) =>
                {
                    string errorMsg = args.Error.Message;
                    MailMergeLib.MailMergeMessage.MailMergeMessageException ex = args.Error as MailMergeLib.MailMergeMessage.MailMergeMessageException;
                    if (ex != null && ex.Exceptions.Count > 0)
                    {
                        errorMsg = string.Format("{0}", ex.Exceptions[0].Message);
                    }
                    string text = string.Format("Error: {0}", errorMsg);

                });

                mailSender.LocalHostName = Environment.MachineName; //"mail." + 
                mailSender.MaxFailures = 1;
                mailSender.DelayBetweenMessages = 1000;
                string messageOutputDir = System.IO.Path.GetTempPath() + @"\mail";
                if (!System.IO.Directory.Exists(messageOutputDir))
                {
                    System.IO.Directory.CreateDirectory(messageOutputDir);
                }
                mailSender.MailOutputDirectory = messageOutputDir;
                mailSender.MessageOutput = MailMergeLib.MessageOutput.SmtpServer;  // change to MessageOutput.Directory if you like

                // smtp details
                mailSender.SmtpHost = OfferingMailSmtpHost;
                mailSender.SmtpPort = 25;
                //mailSender.SetSmtpAuthentification( "username", "password" );

                mailSender.Send(mmm);

                return true;
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

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

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.CartId), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(request.VisitorId);

                //kosár tartalom lekérdezése
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCartToAdd = shoppingCartRepository.GetCartByKey(request.CartId);

                Helpers.DesignByContract.Require((shoppingCartToAdd != null), "ShoppinCart cannot be null!");

                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddressList = customerRepository.GetDeliveryAddress(visitor.CompanyId, visitor.DataAreaId);

                CompanyGroup.Domain.PartnerModule.DeliveryAddress deliveryAddress = deliveryAddressList.Find(x => x.RecId.Equals(request.DeliveryAddressRecId));

                if (deliveryAddress == null)
                {
                    deliveryAddress = (deliveryAddressList.Count > 0) ? deliveryAddressList.First() : new CompanyGroup.Domain.PartnerModule.DeliveryAddress();
                }

                List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate> salesOrderLineCreateRequest = new List<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>();

                shoppingCartToAdd.Items.ForEach(x =>
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
                        CustomerId = visitor.CompanyId,
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
                        Lines = shoppingCartToAdd.Items.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
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
                        CustomerId = visitor.CompanyId,
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
                        Lines = shoppingCartToAdd.Items.ConvertAll<CompanyGroup.Domain.PartnerModule.SalesOrderLineCreate>(x =>
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
                shoppingCartRepository.Post(request.CartId, (PaymentTerms) request.PaymentTerm, (DeliveryTerms) request.DeliveryTerm, shipping);

                //ha van még a felhasználónak kosara, akkor valamelyiket aktiválni kell, ha nincs, akkor létre kell hozni
                //céghez, azon belül személyhez kapcsolódó kosár listából az aktív elem kikeresése
                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

                CompanyGroup.Domain.WebshopModule.ShoppingCartCollection shoppingCartCollection = new CompanyGroup.Domain.WebshopModule.ShoppingCartCollection(shoppingCartList);

                //az elküldést követően is van kosárelem
                if (shoppingCartCollection.ExistsItem)
                {
                    //nincs aktív kosár
                    if (shoppingCartCollection.GetActiveCart() == null)
                    {
                        CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        shoppingCartRepository.SetActive(firstCart.Id, true);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, ShoppingCartService.CreateCartName(), true);

                    shoppingCartRepository.Add(newShoppingCart);
                }

                //kosárlista frissítése kiolvasással
                shoppingCartList = shoppingCartRepository.GetCartCollectionByVisitor(request.VisitorId);

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
