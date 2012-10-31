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
        [AttributeRouting.Web.Http.POST("GetAll")]
        public CompanyGroup.Dto.WebshopModule.Products GetAll(CompanyGroup.Dto.ServiceRequest.GetAllProduct request)
        {
            return this.service.GetAll(request);
        }

        /// <summary>
        /// banner lista a termeklista bannerhez
        /// 1. repository-tól elkéri az akciós, készleten lévő, képpel rendelkező legfeljebb 150 elemet tartalmazó terméklistát.
        /// 2. ha a hívóparaméter tartalmaz jelleg1, jelleg2, jelleg3 paramétert, akkor a paraméterek szerint szűri a listát, majd a találati eredményből visszaadja az első 50 elemet
        /// 3. ha a lekérdezésnek nincs eredménye, akkor az 1. pont szerinti lekérdezés eredményéből visszaadja az első 50 elemet
        /// 
        /// ha bejelentkezett felhasználó kérte le a listát, akkor az eredményhalmazban az ár is kitöltésre kerül, egyébként az ár értéke nulla.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetBannerList")]
        public CompanyGroup.Dto.WebshopModule.BannerList GetBannerList(CompanyGroup.Dto.ServiceRequest.GetBannerList request)
        {
            return this.service.GetBannerList(request);
        }

        /// <summary>
        /// összes, a feltételeknek megfelelő árlista elem leválogatása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetPriceList")]
        public CompanyGroup.Dto.WebshopModule.PriceList GetPriceList(CompanyGroup.Dto.ServiceRequest.GetPriceList request)
        {
            return this.service.GetPriceList(request);
        }

        /// <summary>
        /// objectId egyedi kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.GET("GetItemByObjectId/{objectId}/{visitorId}")]
        public CompanyGroup.Dto.WebshopModule.Product GetItemByObjectId(string objectId, string visitorId)
        {
            return this.service.GetItemByObjectId(request);
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.POST("GetItemByProductId")]
        public CompanyGroup.Dto.WebshopModule.Product GetItemByProductId(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            return this.service.GetItemByProductId(request);
        }

        /// <summary>
        /// terméknév kiegészítő lista
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="prefix"></param>
        /// <param name="completionType">0: nincs megadva, 1: termékazonosító-cikkszám, 2: minden</param>
        /// <returns></returns>
        [AttributeRouting.Web.Http.GET("GetCompletionList/{DataAreaId}/{Prefix}/{CompletionType}")]
        public CompanyGroup.Dto.WebshopModule.CompletionList GetCompletionList(string dataAreaId, string prefix, string completionType) //CompanyGroup.Dto.ServiceRequest.ProductListComplation request
        {
            return this.service.GetCompletionList(dataAreaId, prefix, completionType);
        }

        [AttributeRouting.Web.Http.POST("GetCompatibleProducts")]
        public CompanyGroup.Dto.WebshopModule.CompatibleProducts GetCompatibleProducts(CompanyGroup.Dto.ServiceRequest.GetItemByProductId request)
        {
            return this.service.GetCompatibleProducts(request);
        }


    }
}
