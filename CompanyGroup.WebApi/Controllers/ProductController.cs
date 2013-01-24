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
    public class ProductController : ApiController
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
        /// [GET("contactRange/{id:range(1,3)}")]
        /// [GET(@"text/{e:regex(^[A-Z][a-z][0-9]$)}")]
        /// [RoutePrefix("items", TranslationKey = "itemsKey")]
        [ActionName("GetProducts")]
        [HttpPost]
        public HttpResponseMessage GetProducts(CompanyGroup.Dto.ServiceRequest.GetAllProductRequest request)
        {
            CompanyGroup.Dto.WebshopModule.Products response = this.service.GetProducts(request);

            if (response == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Products>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetBannerList")]
        [HttpPost]
        public HttpResponseMessage GetBannerList(CompanyGroup.Dto.ServiceRequest.GetBannerListRequest request)
        {
            CompanyGroup.Dto.WebshopModule.BannerList response = this.service.GetBannerList(request);

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.BannerList>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetPriceList")]
        [HttpPost]
        public HttpResponseMessage GetPriceList(CompanyGroup.Dto.ServiceRequest.GetPriceListRequest request)
        {
            CompanyGroup.Dto.WebshopModule.PriceList response = this.service.GetPriceList(request);

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.PriceList>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [ActionName("GetItemByProductId")]
        [HttpPost]
        public HttpResponseMessage GetItemByProductId(CompanyGroup.Dto.ServiceRequest.GetItemByProductIdRequest request)
        {
            CompanyGroup.Dto.WebshopModule.Product response = this.service.GetItemByProductId(request);

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Product>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompletionList")]
        [HttpPost]
        public HttpResponseMessage GetCompletionList(CompanyGroup.Dto.ServiceRequest.ProductListComplationRequest request) 
        {
            CompanyGroup.Dto.WebshopModule.CompletionList response = this.service.GetCompletionList(request);

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.CompletionList>(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// kompatibilitás lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompatibleProducts")]
        [HttpPost]
        public HttpResponseMessage GetCompatibleProducts(CompanyGroup.Dto.ServiceRequest.GetItemByProductIdRequest request)
        {
            CompanyGroup.Dto.WebshopModule.CompatibleProducts response = this.service.GetCompatibleProducts(request);

            return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.CompatibleProducts>(HttpStatusCode.OK, response);
        }


    }
}
