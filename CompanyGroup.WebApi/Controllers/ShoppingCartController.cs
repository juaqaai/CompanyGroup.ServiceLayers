using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// kosár műveletek
    /// </summary>
    public class ShoppingCartController : ApiBaseController
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
        public HttpResponseMessage AssociateCart(CompanyGroup.Dto.WebshopModule.AssociateCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AssociateCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// bevásárlókosár hozzáadása bevásárlókosár kollekcióhoz, 
        /// új kosár inicializálása + új elem hozzáadása
        /// </summary>
        [ActionName("AddCart")]
        [HttpPost]
        public HttpResponseMessage AddCart(CompanyGroup.Dto.WebshopModule.AddCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.AddCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kosár mentése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("SaveCart")]
        [HttpPost]
        public HttpResponseMessage SaveCart(CompanyGroup.Dto.WebshopModule.SaveCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.SaveCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// látogatóhoz tartozó aktív kosár törlése kosár kollekcióból 
        /// </summary>
        /// <param name="cartId"></param>
        [ActionName("RemoveCart")]
        [HttpPost]
        public HttpResponseMessage RemoveCart(CompanyGroup.Dto.WebshopModule.RemoveCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.RemoveCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// aktív kosár beállítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        [ActionName("ActivateCart")]
        [HttpPost]
        public HttpResponseMessage ActivateCart(CompanyGroup.Dto.WebshopModule.ActivateCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.ActivateCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// új elem hozzáadása az aktív kosárhoz, 
        /// ha nincs aktív kosár, akkor létrehoz egy új kosarat és ahhoz adja hozzá az új elemet
        /// </summary>
        /// <param name="request"></param>
        [ActionName("AddLine")]
        [HttpPost]
        public HttpResponseMessage AddLine(CompanyGroup.Dto.WebshopModule.AddLineRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = service.AddLine(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        [ActionName("RemoveLine")]
        [HttpPost]
        public HttpResponseMessage RemoveLine(CompanyGroup.Dto.WebshopModule.RemoveLineRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = service.RemoveLine(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// meglévő kosárban elem frissítése 
        /// </summary>
        /// <param name="request"></param>
        [ActionName("UpdateLineQuantity")]
        [HttpPost]
        public HttpResponseMessage UpdateLineQuantity(CompanyGroup.Dto.WebshopModule.UpdateLineQuantityRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions response = service.UpdateLineQuantity(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [ActionName("GetShoppingCartInfo")]
        [HttpPost]
        public HttpResponseMessage GetShoppingCartInfo(CompanyGroup.Dto.WebshopModule.GetShoppingCartInfoRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.GetShoppingCartInfo(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// céghez, azon belül személyhez tartozó érvényes / tárolt kosarak kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetStoredOpenedShoppingCartCollectionByVisitor")]
        [HttpPost]
        public HttpResponseMessage GetStoredOpenedShoppingCartCollectionByVisitor(CompanyGroup.Dto.WebshopModule.GetCartCollectionByVisitorRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection response = service.GetStoredOpenedShoppingCartCollectionByVisitor(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// felhasználóhoz tartozó érvényes kosarak kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        [ActionName("GetCartCollectionByVisitor")]
        [HttpPost]
        public HttpResponseMessage GetCartCollectionByVisitor(CompanyGroup.Dto.WebshopModule.GetCartCollectionByVisitorRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartCollection response = service.GetCartCollectionByVisitor(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartCollection>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [ActionName("GetCartByKey")]
        [HttpPost]
        public HttpResponseMessage GetCartByKey(CompanyGroup.Dto.WebshopModule.GetCartByKeyRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCart response = service.GetCartByKey(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCart>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// aktív kosár kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetActiveCart")]
        [HttpPost]
        public HttpResponseMessage GetActiveCart(CompanyGroup.Dto.WebshopModule.GetActiveCartRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo response = service.GetActiveCart(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ActionName("CreateFinanceOffer")]
        //[HttpPost]
        //public CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.WebshopModule.CreateFinanceOffer request)
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
        public HttpResponseMessage CreateOrder(CompanyGroup.Dto.WebshopModule.SalesOrderCreateRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.OrderFulFillment response = service.CreateOrder(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.OrderFulFillment>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
