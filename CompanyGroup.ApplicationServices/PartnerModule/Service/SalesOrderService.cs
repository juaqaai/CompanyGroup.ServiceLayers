using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class SalesOrderService : ServiceBase, ISalesOrderService
    {
        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        private CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository;

        private CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public SalesOrderService(CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository, 
                                 CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                                 CompanyGroup.Domain.PartnerModule.ISalesOrderRepository salesOrderRepository,
                                 CompanyGroup.Domain.WebshopModule.IShoppingCartRepository shoppingCartRepository,
                                 CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
            if (customerRepository == null)
            {
                throw new ArgumentNullException("CustomerRepository");
            }

            if (salesOrderRepository == null)
            {
                throw new ArgumentNullException("SalesOrderRepository");
            }

            if (shoppingCartRepository == null)
            {
                throw new ArgumentNullException("ShoppingCartRepository");
            }

            this.customerRepository = customerRepository;

            this.salesOrderRepository = salesOrderRepository;

            this.shoppingCartRepository = shoppingCartRepository;
        }

        /// <summary>
        /// megrendelés információk lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.PartnerModule.OrderInfo> GetOrderInfo(CompanyGroup.Dto.ServiceRequest.GetOrderInfo request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //látogató alapján kikeresett vevő rendelés listája
                List<CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo> lineInfos = salesOrderRepository.GetOrderDetailedLineInfo(visitor.CustomerId, visitor.DataAreaId);

                //megrendelés info aggregátum elkészítése
                IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>> groupedLineInfos = lineInfos.GroupBy(x => x.SalesId).OrderBy(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

                List<CompanyGroup.Domain.PartnerModule.OrderInfo> orderInfoList = new List<CompanyGroup.Domain.PartnerModule.OrderInfo>();

                foreach (var lineInfo in groupedLineInfos)
                { 
                    CompanyGroup.Domain.PartnerModule.OrderInfo orderInfo = CompanyGroup.Domain.PartnerModule.OrderInfo.Create(lineInfo.ToList());

                    orderInfoList.Add(orderInfo);
                }

                //konverzió dto-ra
                return new OrderInfoToOrderInfo().Map(orderInfoList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// vevőrendelés létrehozása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.OrderFulFillment Create(CompanyGroup.Dto.ServiceRequest.SalesOrderCreate request)
        {

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.CartId > 0), "Cart id cannot be null!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //kosár tartalom lekérdezése
                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCartToAdd = shoppingCartRepository.GetShoppingCart(request.CartId);

                Helpers.DesignByContract.Require((shoppingCartToAdd != null), "ShoppingCart cannot be null!");

                //szállítási cím keresése
                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddressList = customerRepository.GetDeliveryAddress(visitor.CustomerId, visitor.DataAreaId);

                CompanyGroup.Domain.PartnerModule.DeliveryAddress deliveryAddress = deliveryAddressList.Find(x => x.RecId.Equals(request.DeliveryAddressRecId));

                if (deliveryAddress == null)
                {
                    deliveryAddress = (deliveryAddressList.Count > 0) ? deliveryAddressList.First() : new CompanyGroup.Domain.PartnerModule.DeliveryAddress();
                }

                CompanyGroup.Domain.PartnerModule.SalesOrderCreate salesOrderCreate = new CompanyGroup.Domain.PartnerModule.SalesOrderCreate()
                {
                    ContactPersonId = visitor.PersonId,
                    CustomerId = visitor.CustomerId,
                    DataAreaId = visitor.DataAreaId,
                    DeliveryCity = deliveryAddress.City,
                    DeliveryCompanyName = "",
                    DeliveryDate = String.Format("{0}-{1}-{2}", request.DeliveryDate.Year, request.DeliveryDate.Month, request.DeliveryDate.Day),
                    DeliveryEmail = "",
                    DeliveryId = "",
                    DeliveryPersonName = "",
                    DeliveryPhone = "",
                    DeliveryStreet = deliveryAddress.Street,
                    DeliveryZip = deliveryAddress.ZipCode,
                    InventLocationId = "",
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

                //kosár beállítása elküldött státuszba  request.CartId, (PaymentTerms)request.PaymentTerm, (DeliveryTerms)request.DeliveryTerm, shipping
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
                        //CompanyGroup.Domain.WebshopModule.ShoppingCart firstCart = shoppingCartCollection.Carts.FirstOrDefault();

                        //shoppingCartRepository.SetActive(firstCart.Id, true);
                    }
                }
                else
                {
                    //új kosár létrehozása és hozzáadása
                    //CompanyGroup.Domain.WebshopModule.ShoppingCart newShoppingCart = CompanyGroup.Domain.WebshopModule.Factory.CreateShoppingCart(request.VisitorId, visitor.CompanyId, visitor.PersonId, "", true);

                    //shoppingCartRepository.Add(newShoppingCart);
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
                //CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                //leasingOptions.Amount = activeCart.SumTotal;

                //leasingOptions.ValidateAmount();

               // CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection storedOpenedShoppingCarts = new ShoppingCartCollectionToStoredOpenedShoppingCartCollection().Map(shoppingCartCollection);

                CompanyGroup.Dto.WebshopModule.OrderFulFillment response = new CompanyGroup.Dto.WebshopModule.OrderFulFillment();
                //{
                //    ActiveCart = new ShoppingCartToShoppingCart().Map(activeCart),
                //    OpenedItems = storedOpenedShoppingCarts.OpenedItems,
                //    StoredItems = storedOpenedShoppingCarts.StoredItems,
                //    LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                //    Created = false,
                //    WaitForAutoPost = true,
                //    Message = ""
                //};

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
