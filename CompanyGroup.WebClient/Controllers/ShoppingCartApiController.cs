using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class ShoppingCartApiController : ApiBaseController
    {
        /// <summary>
        /// kosár azonosító alapján történő lekérdezése
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKey request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ShoppingCart GetCartByKey(CompanyGroup.WebClient.Models.GetCartByKey request)
        {
            CompanyGroup.Dto.ServiceRequest.GetCartByKey requestBody = new CompanyGroup.Dto.ServiceRequest.GetCartByKey(this.ReadLanguageFromCookie(), this.ReadCartIdFromCookie(), this.ReadObjectIdFromCookie());

            CompanyGroup.Dto.WebshopModule.ShoppingCart response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetCartByKey, CompanyGroup.Dto.WebshopModule.ShoppingCart>("ShoppingCart", "GetCartByKey", requestBody);

            return new CompanyGroup.WebClient.Models.ShoppingCart(response);
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo GetActiveCart(CompanyGroup.WebClient.Models.GetActiveCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetActiveCart requestBody = new CompanyGroup.Dto.ServiceRequest.GetActiveCart(this.ReadLanguageFromCookie(), this.ReadObjectIdFromCookie());

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetActiveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetActiveCart", requestBody);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartInfo(response, visitor, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened);

        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ShoppingCartInfo AddCart(CompanyGroup.WebClient.Models.AddCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddCart addCart = new CompanyGroup.Dto.ServiceRequest.AddCart(visitorData.Language, visitorData.ObjectId, String.Empty);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "AddCart", addCart);

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
        public CompanyGroup.WebClient.Models.ShoppingCartInfo ActivateCart(CompanyGroup.WebClient.Models.ActivateCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.ActivateCart activateCart = new CompanyGroup.Dto.ServiceRequest.ActivateCart(request.CartId, visitorData.Language, visitorData.ObjectId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.ActivateCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "ActivateCart", activateCart);

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
        public CompanyGroup.WebClient.Models.ShoppingCartInfo RemoveCart()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveCart removeCart = new CompanyGroup.Dto.ServiceRequest.RemoveCart(visitorData.CartId, visitorData.Language, visitorData.ObjectId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "RemoveCart", removeCart);

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
        public CompanyGroup.WebClient.Models.ShoppingCartInfo SaveCart(CompanyGroup.WebClient.Models.SaveCart request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.SaveCart saveCart = new CompanyGroup.Dto.ServiceRequest.SaveCart(visitorData.Language, visitorData.ObjectId, visitorData.CartId, request.Name);

            CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.SaveCart, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "SaveCart", saveCart);

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
        public CompanyGroup.WebClient.Models.FinanceOfferFulFillment CreateFinanceOffer( CompanyGroup.WebClient.Models.CreateFinanceOffer request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer createFinanceOffer = new CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer()
            {
                Address = request.Address,
                CartId = visitorData.CartId,
                NumOfMonth = request.NumOfMonth,
                PersonName = request.PersonName,
                Phone = request.Phone,
                StatNumber = request.StatNumber,
                VisitorId = visitorData.ObjectId
            };

            CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer, CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment>("ShoppingCart", "CreateFinanceOffer", createFinanceOffer);

            //aktív kosár azonosítójának mentése http cookie-ba
            visitorData.CartId = response.ActiveCart.Id;

            this.WriteCookie(visitorData);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.FinanceOfferFulFillment(response, visitorData.IsCatalogueOpened, visitorData.IsShoppingCartOpened, visitor);
        }

        [HttpPost]
        public CompanyGroup.WebClient.Models.OrderFulFillment CreateOrder(CompanyGroup.WebClient.Models.CreateOrder request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.CreateOrder createOrder = new CompanyGroup.Dto.ServiceRequest.CreateOrder()
            {
                CartId = this.ReadCartIdFromCookie(),
                CustomerOrderId = request.CustomerOrderId,
                CustomerOrderNote = request.CustomerOrderNote,
                DeliveryAddressRecId = request.DeliveryAddressRecId,
                DeliveryDate = request.DeliveryDate,
                DeliveryRequest = request.DeliveryRequest,
                DeliveryTerm = request.DeliveryTerm,
                PaymentTerm = request.PaymentTerm,
                VisitorId = this.ReadObjectIdFromCookie()
            };

            CompanyGroup.Dto.WebshopModule.OrderFulFillment response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.CreateOrder, CompanyGroup.Dto.WebshopModule.OrderFulFillment>("ShoppingCart", "CreateOrder", createOrder);

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
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo AddLine(CompanyGroup.WebClient.Models.AddLine request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.AddLine addLine = new CompanyGroup.Dto.ServiceRequest.AddLine(visitorData.CartId, request.ProductId, visitorData.Language, ShoppingCartApiController.DataAreaId, request.Quantity, visitorData.ObjectId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.AddLine, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "AddLine", addLine);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLine request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo RemoveLine(CompanyGroup.WebClient.Models.RemoveLine request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.RemoveLine removeLine = new CompanyGroup.Dto.ServiceRequest.RemoveLine(visitorData.CartId, request.ProductId, visitorData.Language, ShoppingCartApiController.DataAreaId, visitorData.ObjectId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.RemoveLine, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "RemoveLine", removeLine);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        /// <summary>
        /// CompanyGroup.Dto.WebshopModule.ShoppingCart UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public CompanyGroup.WebClient.Models.ShoppingCartLineInfo UpdateLineQuantity(CompanyGroup.WebClient.Models.UpdateLineQuantity request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity updateLineQuantity = new CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity(visitorData.CartId, request.ProductId, visitorData.Language, ShoppingCartApiController.DataAreaId, request.Quantity, visitorData.ObjectId);

            CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions shoppingCart = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity, CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>("ShoppingCart", "UpdateLineQuantity", updateLineQuantity);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ShoppingCartLineInfo(visitor, shoppingCart);
        }

        [HttpPost]
        public void SaveShoppingCartOpenStatus(CompanyGroup.WebClient.Models.ShoppingCartOpenStatus request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            visitorData.IsShoppingCartOpened = request.IsOpen;

            this.WriteCookie(visitorData);

            return;
        }

        [HttpPost]
        public bool ReadShoppingCartOpenStatus()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            return visitorData.IsShoppingCartOpened;
        }

        [HttpPost]
        public void SaveCatalogueOpenStatus(CompanyGroup.WebClient.Models.CatalogueOpenStatus request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            visitorData.IsCatalogueOpened = request.IsOpen;

            this.WriteCookie(visitorData);

            return;
        }

        [HttpPost]
        public bool ReadCatalogueOpenStatus()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            return visitorData.IsCatalogueOpened;
        }
    }
}
