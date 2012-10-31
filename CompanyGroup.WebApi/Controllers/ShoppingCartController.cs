using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IShoppingCartService service;

        public ShoppingCartController(CompanyGroup.ApplicationServices.WebshopModule.IShoppingCartService service)
        {
            this.service = service;
        }

        /// <summary>
        /// új visitorId beállítása a régi helyére, ha a kosár mentett státuszú, 
        /// hozzáad egy nyitott kosarat a felhasználó kosaraihoz
        /// 1. permanens visitorId alapján a kosarak kiolvasása
        /// 2. végig az előző eredménylistán, és az új visitorId beállítása a permanensId helyére
        /// 3. kosarak lista visszaadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>CompanyGroup.Dto.ServiceResponse
        [AttributeRouting.Web.Http.POST("AssociateCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.ServiceRequest.AssociateCart request)
        {
            return service.AssociateCart(request);
        }

        /// <summary>
        /// bevásárlókosár hozzáadása bevásárlókosár kollekcióhoz, 
        /// új kosár inicializálása + új elem hozzáadása
        /// </summary>
        [AttributeRouting.Web.Http.POST("AddCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request)
        {
            return service.AddCart(request);
        }

        /// <summary>
        /// kosár mentése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("SaveCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo SaveCart(CompanyGroup.Dto.ServiceRequest.SaveCart request)
        {
            return service.SaveCart(request);
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár törlése kosár kollekcióból 
        /// </summary>
        /// <param name="cartId"></param>
        [AttributeRouting.Web.Http.POST("RemoveCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo RemoveCart(CompanyGroup.Dto.ServiceRequest.RemoveCart request)
        {
            return service.RemoveCart(request);
        }

        /// <summary>
        /// aktív kosár beállítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        [AttributeRouting.Web.Http.POST("ActivateCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.ServiceRequest.ActivateCart request)
        {
            return service.ActivateCart(request);
        }

        /// <summary>
        /// új elem hozzáadása az aktív kosárhoz, 
        /// ha nincs aktív kosár, akkor létrehoz egy új kosarat és ahhoz adja hozzá az új elemet
        /// </summary>
        /// <param name="request"></param>
        [AttributeRouting.Web.Http.POST("AddLine")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions AddLine(CompanyGroup.Dto.ServiceRequest.AddLine request)
        {
            return service.AddLine(request);
        }

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        [AttributeRouting.Web.Http.POST("RemoveLine")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLine request)
        {
            return service.RemoveLine(request);
        }

        /// <summary>
        /// meglévő kosárban elem frissítése 
        /// </summary>
        /// <param name="request"></param>
        [AttributeRouting.Web.Http.POST("UpdateLineQuantity")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity request)
        {
            return service.UpdateLineQuantity(request);
        }

        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetShoppingCartInfo(CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo request)
        {
            return service.GetShoppingCartInfo(request);
        }

        /// <summary>
        /// céghez, azon belül személyhez tartozó érvényes / tárolt kosarak kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetStoredOpenedShoppingCartCollectionByVisitor")]
        public CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection GetStoredOpenedShoppingCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request)
        {
            return service.GetStoredOpenedShoppingCartCollectionByVisitor(request);
        }

        /// <summary>
        /// felhasználóhoz tartozó érvényes kosarak kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetCartCollectionByVisitor")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartCollection GetCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request)
        {
            return service.GetCartCollectionByVisitor(request);
        }

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetCartByKey")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKey request)
        {
            return service.GetCartByKey(request);
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetActiveCart")]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetActiveCart(CompanyGroup.Dto.ServiceRequest.GetActiveCart request)
        {
            return service.GetActiveCart(request);
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("CreateFinanceOffer")]
        public CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer request)
        {
            return service.CreateFinanceOffer(request);
        }

        /// <summary>
        /// rendelés elküldése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("CreateOrder")]
        public CompanyGroup.Dto.WebshopModule.OrderFulFillment CreateOrder(CompanyGroup.Dto.ServiceRequest.SalesOrderCreate request)
        {
            return service.CreateOrder(request);
        }

    }
}
