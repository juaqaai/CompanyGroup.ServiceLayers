using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// kosárral kapcsolatos műveletek
    /// </summary>
    public class ShoppingCartApiController : ApiBaseController
    {
        /// <summary>
        /// kosár azonosító alapján történő lekérdezése
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKey request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetCartByKey")]
        public CompanyGroup.WebClient.Models.ShoppingCart GetCartByKey(CompanyGroup.WebClient.Models.GetCartByKey request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetCartByKey req = new CompanyGroup.Dto.ServiceRequest.GetCartByKey(visitorData.Language, visitorData.CartId, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCart response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetCartByKey, CompanyGroup.Dto.WebshopModule.ShoppingCart>("ShoppingCart", "GetCartByKey", req);

            return new CompanyGroup.WebClient.Models.ShoppingCart(response);
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetActiveCart")]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo GetActiveCart(CompanyGroup.WebClient.Models.GetActiveCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetActiveCart req = new CompanyGroup.Dto.ServiceRequest.GetActiveCart(visitorData.Language, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetActiveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetActiveCart", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);

        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddCart")]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo AddCart(CompanyGroup.WebClient.Models.AddCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddCart req = new CompanyGroup.Dto.ServiceRequest.AddCart(visitorData.Language, visitorData.VisitorId, String.Empty);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AddCart", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.ServiceRequest.ActivateCart request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ActivateCart")]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo ActivateCart(CompanyGroup.WebClient.Models.ActivateCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.ActivateCart req = new CompanyGroup.Dto.ServiceRequest.ActivateCart(request.CartId, visitorData.Language, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.ActivateCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "ActivateCart", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveCart")]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo RemoveCart()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveCart req = new CompanyGroup.Dto.ServiceRequest.RemoveCart(visitorData.CartId, visitorData.Language, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "RemoveCart", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SaveCart")]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo SaveCart(CompanyGroup.WebClient.Models.SaveCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.SaveCart req = new CompanyGroup.Dto.ServiceRequest.SaveCart(visitorData.Language, visitorData.VisitorId, visitorData.CartId, request.Name);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SaveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "SaveCart", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);
        }

        /// <summary>
        /// finanszírozási ajánlat elküldése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("CreateFinanceOffer")]
        public CompanyGroup.WebClient.Models.FinanceOfferFulFillment CreateFinanceOffer( CompanyGroup.WebClient.Models.CreateFinanceOffer request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.CreateFinanceOfferRequest req = new CompanyGroup.Dto.ServiceRequest.CreateFinanceOfferRequest()
            {
                Address = request.Address,
                NumOfMonth = request.NumOfMonth,
                PersonName = request.PersonName,
                Phone = request.Phone,
                StatNumber = request.StatNumber,
                VisitorId = visitorData.VisitorId
            };

            CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.CreateFinanceOfferRequest, CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment>("ShoppingCart", "CreateFinanceOffer", req);

            //aktív kosár azonosítójának mentése http cookie-ba
            //visitorData.CartId = response..Id;

            //this.WriteCookie(visitorData);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.FinanceOfferFulFillment(response, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened, visitor);
        }

        [HttpPost]
        [ActionName("CreateOrder")]
        public CompanyGroup.WebClient.Models.OrderFulFillment CreateOrder(CompanyGroup.WebClient.Models.CreateOrder request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.CreateOrder req = new CompanyGroup.Dto.ServiceRequest.CreateOrder()
            {
                CartId = visitorData.CartId,
                CustomerOrderId = request.CustomerOrderId,
                CustomerOrderNote = request.CustomerOrderNote,
                DeliveryAddressRecId = request.DeliveryAddressRecId,
                DeliveryDate = request.DeliveryDate,
                DeliveryRequest = request.DeliveryRequest,
                DeliveryTerm = request.DeliveryTerm,
                PaymentTerm = request.PaymentTerm,
                VisitorId = visitorData.VisitorId
            };

            CompanyGroup.Dto.WebshopModule.OrderFulFillment response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.CreateOrder, CompanyGroup.Dto.WebshopModule.OrderFulFillment>("ShoppingCart", "CreateOrder", req);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.OrderFulFillment(response, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened, visitor);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart AddLine(CompanyGroup.Dto.ServiceRequest.AddLine request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddLine")]
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo AddLine(CompanyGroup.WebClient.Models.AddLine request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddLine req = new CompanyGroup.Dto.ServiceRequest.AddLine(visitorData.CartId, request.ProductId, visitorData.Language, ShoppingCartApiController.DataAreaId, request.Quantity, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddLine, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "AddLine", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLine request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("RemoveLine")]
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo RemoveLine(CompanyGroup.WebClient.Models.RemoveLine request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveLine req = new CompanyGroup.Dto.ServiceRequest.RemoveLine(visitorData.CartId, request.LineId, visitorData.Language, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveLine, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "RemoveLine", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateLineQuantity")]
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo UpdateLineQuantity(CompanyGroup.WebClient.Models.UpdateLineQuantity request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity updateLineQuantity = new CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity(visitorData.CartId, request.LineId, visitorData.Language, ShoppingCartApiController.DataAreaId, request.Quantity, visitorData.VisitorId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "UpdateLineQuantity", updateLineQuantity);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        [HttpPost]
        [ActionName("SaveShoppingCartOpenStatus")]
        public void SaveShoppingCartOpenStatus(CompanyGroup.WebClient.Models.ShoppingCartOpenStatus request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            visitorData.IsShoppingCartOpened = request.IsOpen;

            this.WriteCookie(visitorData);

            return;
        }

        [HttpPost]
        [ActionName("ReadShoppingCartOpenStatus")]
        public bool ReadShoppingCartOpenStatus()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            return visitorData.IsShoppingCartOpened;
        }

        [HttpPost]
        [ActionName("SaveCatalogueOpenStatus")]
        public void SaveCatalogueOpenStatus(CompanyGroup.WebClient.Models.CatalogueOpenStatus request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            visitorData.IsCatalogueOpened = request.IsOpen;

            this.WriteCookie(visitorData);

            return;
        }

        [HttpPost]
        [ActionName("ReadCatalogueOpenStatus")]
        public bool ReadCatalogueOpenStatus()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            return visitorData.IsCatalogueOpened;
        }
    }
}
