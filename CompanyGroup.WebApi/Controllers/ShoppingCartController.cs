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
        [ActionName("AssociateCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.ServiceRequest.AssociateCart request)
        {
            return service.AssociateCart(request);
        }

        /// <summary>
        /// bevásárlókosár hozzáadása bevásárlókosár kollekcióhoz, 
        /// új kosár inicializálása + új elem hozzáadása
        /// </summary>
        [ActionName("AddCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request)
        {
            return service.AddCart(request);
        }

        /// <summary>
        /// kosár mentése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("SaveCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo SaveCart(CompanyGroup.Dto.ServiceRequest.SaveCartRequest request)
        {
            return service.SaveCart(request);
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár törlése kosár kollekcióból 
        /// </summary>
        /// <param name="cartId"></param>
        [ActionName("RemoveCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo RemoveCart(CompanyGroup.Dto.ServiceRequest.RemoveCartRequest request)
        {
            return service.RemoveCart(request);
        }

        /// <summary>
        /// aktív kosár beállítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        [ActionName("ActivateCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.ServiceRequest.ActivateCartRequest request)
        {
            return service.ActivateCart(request);
        }

        /// <summary>
        /// új elem hozzáadása az aktív kosárhoz, 
        /// ha nincs aktív kosár, akkor létrehoz egy új kosarat és ahhoz adja hozzá az új elemet
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddLine")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions AddLine(CompanyGroup.Dto.ServiceRequest.AddLineRequest request)
        {
            return service.AddLine(request);
        }

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        [ActionName("RemoveLine")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLineRequest request)
        {
            return service.RemoveLine(request);
        }

        /// <summary>
        /// meglévő kosárban elem frissítése 
        /// </summary>
        /// <param name="request"></param>
        [ActionName("UpdateLineQuantity")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantityRequest request)
        {
            return service.UpdateLineQuantity(request);
        }

        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetShoppingCartInfo(CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfoRequest request)
        {
            return service.GetShoppingCartInfo(request);
        }

        /// <summary>
        /// céghez, azon belül személyhez tartozó érvényes / tárolt kosarak kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetStoredOpenedShoppingCartCollectionByVisitor")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection GetStoredOpenedShoppingCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitorRequest request)
        {
            return service.GetStoredOpenedShoppingCartCollectionByVisitor(request);
        }

        /// <summary>
        /// felhasználóhoz tartozó érvényes kosarak kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        [ActionName("GetCartCollectionByVisitor")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartCollection GetCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitorRequest request)
        {
            return service.GetCartCollectionByVisitor(request);
        }

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [ActionName("GetCartByKey")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKeyRequest request)
        {
            return service.GetCartByKey(request);
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetActiveCart")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetActiveCart(CompanyGroup.Dto.ServiceRequest.GetActiveCartRequest request)
        {
            return service.GetActiveCart(request);
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ActionName("CreateFinanceOffer")]
        //[HttpPost]
        //public CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer request)
        //{
        //    return service.CreateFinanceOffer(request);
        //}

        /// <summary>
        /// rendelés elküldése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("CreateOrder")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.OrderFulFillment CreateOrder(CompanyGroup.Dto.ServiceRequest.SalesOrderCreateRequest request)
        {
            return service.CreateOrder(request);
        }

    }
}
