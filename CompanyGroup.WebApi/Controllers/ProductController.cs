using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// termékekkel kapcsolatos műveletek
    /// </summary>
    public class ProductController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IProductService service;

        /// <summary>
        /// konstruktor termékekkel kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public ProductController(CompanyGroup.ApplicationServices.WebshopModule.IProductService service)
        {
            this.service = service;
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetProducts")]
        [HttpPost]
        public HttpResponseMessage GetProducts(CompanyGroup.Dto.WebshopModule.GetAllProductRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.Products response = this.service.GetProducts(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Products>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetBannerList")]
        [HttpPost]
        public HttpResponseMessage GetBannerList(CompanyGroup.Dto.WebshopModule.GetBannerListRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.BannerList response = this.service.GetBannerList(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.BannerList>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetPriceList")]
        [HttpPost]
        public HttpResponseMessage GetPriceList(CompanyGroup.Dto.WebshopModule.GetPriceListRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.PriceList response = this.service.GetPriceList(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.PriceList>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [ActionName("GetItemByProductId")]
        [HttpPost]
        public HttpResponseMessage GetItemByProductId(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.Product response = this.service.GetItemByProductId(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Product>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompletionList")]
        [HttpPost]
        public HttpResponseMessage GetCompletionList(CompanyGroup.Dto.WebshopModule.ProductListComplationRequest request) 
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.CompletionList response = this.service.GetCompletionList(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.CompletionList>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// kompatibilitás lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompatibleProducts")]
        [HttpPost]
        public HttpResponseMessage GetCompatibleProducts(CompanyGroup.Dto.WebshopModule.GetItemByProductIdRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.CompatibleProducts response = this.service.GetCompatibleProducts(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.CompatibleProducts>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// részletes adatlap log lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCatalogueDetailsLogList")]
        [HttpPost]
        public HttpResponseMessage GetCatalogueDetailsLogList(CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogListRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList response = this.service.GetCatalogueDetailsLogList(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// készlet databszám befrissítése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("StockUpdate")]
        [HttpPost]
        public HttpResponseMessage StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request)
        {
            try
            {
                this.service.StockUpdate(request);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }        
        }
    }
}
