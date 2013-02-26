using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// terméklista cache karbantartó műveletek
    /// </summary>
    public class MaintainController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.MaintainModule.ISyncService service;

        /// <summary>
        /// konstruktor terméklista cache karbantartó interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public MaintainController(CompanyGroup.ApplicationServices.MaintainModule.ISyncService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("MaintainService");
            }

            this.service = service;
        }

        /// <summary>
        /// terméklista cache újra töltése
        /// </summary>
        [ActionName("StockUpdate")]
        [HttpPost]
        public HttpResponseMessage StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request)
        {
            try
            {
                service.StockUpdate(request);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
