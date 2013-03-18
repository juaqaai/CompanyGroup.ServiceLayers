using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// képekkel kapcsolatos műveletek
    /// </summary>
    public class PictureController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IPictureService service;

        /// <summary>
        /// konstruktor képekkel kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public PictureController(CompanyGroup.ApplicationServices.WebshopModule.IPictureService service)
        {
            this.service = service;
        }

        /// <summary>
        /// képek listát visszaadó service
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        [ActionName("GetListByProduct")]
        [HttpPost]
        public HttpResponseMessage GetListByProduct(CompanyGroup.Dto.WebshopModule.PictureFilterRequest request)
        {           
            try
            {
                CompanyGroup.Dto.WebshopModule.Pictures response = this.service.GetListByProduct(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Pictures>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        /// <summary>
        /// képtartalmat adja vissza 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [ActionName("GetItem")] ///{ProductId}/{RecId}/{Width}/{Height}
        [HttpGet]
        public HttpResponseMessage GetItem(string productId, string recId, string maxWidth, string maxHeight) //CompanyGroup.Dto.ServiceRequest.PictureFilter request
        {
            try
            {
                Stream stream = this.service.GetItem(productId, recId, maxWidth, maxHeight);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new System.Net.Http.StreamContent(stream);

                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [ActionName("GetItemById")] 
        [HttpGet]
        public HttpResponseMessage GetItemById(string pictureId, string maxWidth, string maxHeight) 
        {
            try
            {
                int id = 0;

                if (!Int32.TryParse(pictureId, out id))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                Stream stream = this.service.GetItemById(id, maxWidth, maxHeight);

                if (stream == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new System.Net.Http.StreamContent(stream);

                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [HttpGet]
        [ActionName("GetInvoicePicture")]
        public HttpResponseMessage GetInvoicePicture()
        {
            string id = CompanyGroup.Helpers.QueryStringParser.GetString("Id");

            string maxWidth = CompanyGroup.Helpers.QueryStringParser.GetString("MaxWidth");

            string maxHeight = CompanyGroup.Helpers.QueryStringParser.GetString("MaxHeight");

            return this.GetInvoicePictureByRecId(id, maxWidth, maxHeight);
        }

        /// <summary>
        /// számla listaelemhez tartozó kép kiolvasása stream-ként 		requestUrl	"Picture/GetInvoicePicture/5637928067/500/500"	string
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        [ActionName("GetInvoicePictureByRecId")]
        [HttpGet]
        public HttpResponseMessage GetInvoicePictureByRecId(string id, string maxWidth, string maxHeight)
        {
            try
            {
                int invoiceLineId = 0;

                if (!Int32.TryParse(id, out invoiceLineId))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                Stream stream = this.service.GetInvoicePicture(invoiceLineId, maxWidth, maxHeight);

                if (stream == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new System.Net.Http.StreamContent(stream);

                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
