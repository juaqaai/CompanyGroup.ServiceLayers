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
        /// <summary>
        /// kép lekérdezés műveletek
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetPictureItem")]
        public HttpResponseMessage GetPictureItem()
        {
            string pictureId = CompanyGroup.Helpers.QueryStringParser.GetString("PictureId");

            string maxWidth = CompanyGroup.Helpers.QueryStringParser.GetString("MaxWidth");

            string maxHeight = CompanyGroup.Helpers.QueryStringParser.GetString("MaxHeight");

            return GetPictureItemById(pictureId, maxWidth, maxHeight);
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
        [ActionName("GetPicture")]
        public HttpResponseMessage GetPicture(string productId, string recId, string dataAreaId, string maxWidth, string maxHeight)
        {
            byte[] picture = this.DownloadData(String.Format("Picture/GetItem/{0}/{1}/{2}/{3}/{4}", productId, recId, dataAreaId, maxWidth, maxHeight));

            if (picture == null)
            {
                return Request.CreateResponse<CompanyGroup.WebClient.Models.ApiMessage>(HttpStatusCode.NotFound, new CompanyGroup.WebClient.Models.ApiMessage("Picture not found"));
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new ByteArrayContent(picture);

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return result;
        }

        /// <summary>
        /// kép lekérdezése stream-be
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetPictureItemById")]
        public HttpResponseMessage GetPictureItemById(string pictureId, string maxWidth, string maxHeight)
        {
            byte[] picture = this.DownloadData(String.Format("Picture/GetItemById/{0}/{1}/{2}", pictureId, maxWidth, maxHeight));

            if (picture == null)
            {
                return Request.CreateResponse<CompanyGroup.WebClient.Models.ApiMessage>(HttpStatusCode.NotFound, new CompanyGroup.WebClient.Models.ApiMessage("Picture not found"));
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new ByteArrayContent(picture);

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            return result;
        }

        /// <summary>
        /// termékhez tartozó képek lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetListByProduct")]
        public CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.WebshopModule.PictureFilterRequest request)
        {
            //request.DataAreaId = CatalogueController.DataAreaId;

            request.ProductId = System.Web.HttpUtility.UrlDecode(request.ProductId);

            CompanyGroup.Dto.WebshopModule.Pictures pictures = this.PostJSonData<CompanyGroup.Dto.WebshopModule.PictureFilterRequest, CompanyGroup.Dto.WebshopModule.Pictures>("Picture", "GetListByProduct", request);

            return pictures;
        }
    }
}
