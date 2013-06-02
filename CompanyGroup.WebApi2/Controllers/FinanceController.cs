using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    public class FinanceController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IFinanceService service;

        public FinanceController(CompanyGroup.ApplicationServices.WebshopModule.IFinanceService service)
        {
            this.service = service;
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("CreateFinanceOffer")]
        [HttpPost]
        public HttpResponseMessage CreateFinanceOffer(CompanyGroup.Dto.WebshopModule.CreateFinanceOfferRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.FinanceOfferResponse response = service.CreateFinanceOffer(request);

                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.FinanceOfferResponse>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("FinanceController CreateFinanceOffer {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }
    }
}
