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
        public CompanyGroup.Dto.WebshopModule.Products GetProducts(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            CompanyGroup.Dto.WebshopModule.Products response = this.service.GetProducts(request);

            if (response == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return response;
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetBannerList")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.ServiceRequest.GetBannerList request)
        {
            return this.service.GetBannerList(request);
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetPriceList")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.ServiceRequest.GetPriceList request)
        {
            return this.service.GetPriceList(request);
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [ActionName("GetItemByProductId")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            return this.service.GetItemByProductId(request);
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompletionList")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(CompanyGroup.Dto.ServiceRequest.ProductListComplation request) 
        {
            return this.service.GetCompletionList(request);
        }

        /// <summary>
        /// kompatibilitás lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetCompatibleProducts")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            return this.service.GetCompatibleProducts(request);
        }


    }
}
