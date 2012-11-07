using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class PictureApiController : ApiBaseController
    {
        [HttpGet]
        [ActionName("PictureItem")]
        public System.Web.Mvc.FileStreamResult PictureItem()
        {
            string productId = CompanyGroup.Helpers.QueryStringParser.GetString("ProductId");

            string recId = CompanyGroup.Helpers.QueryStringParser.GetString("RecId");

            string dataAreaId = CompanyGroup.Helpers.QueryStringParser.GetString("DataAreaId");

            string maxWidth = CompanyGroup.Helpers.QueryStringParser.GetString("MaxWidth");

            string maxHeight = CompanyGroup.Helpers.QueryStringParser.GetString("MaxHeight");

            return Picture(productId, recId, dataAreaId, maxWidth, maxHeight);
        }

        /// <summary>
        /// kép lekérdezése stream-be
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Picture")]
        public System.Web.Mvc.FileStreamResult Picture(string productId, string recId, string dataAreaId, string maxWidth, string maxHeight)
        {
            byte[] picture = this.DownloadData(String.Format("Picture/GetItem/{0}/{1}/{2}/{3}/{4}", productId, recId, dataAreaId, maxWidth, maxHeight));

            return new System.Web.Mvc.FileStreamResult(new System.IO.MemoryStream(picture), "image/jpeg");
        }

        /// <summary>
        /// termékhez tartozó képek lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetListByProduct")]
        public CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.ServiceRequest.PictureFilter request)
        {
            //request.DataAreaId = CatalogueController.DataAreaId;

            request.ProductId = System.Web.HttpUtility.UrlDecode(request.ProductId);

            CompanyGroup.Dto.WebshopModule.Pictures pictures = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.PictureFilter, CompanyGroup.Dto.WebshopModule.Pictures>("Picture", "GetListByProduct", request);

            return pictures;
        }
    }
}
