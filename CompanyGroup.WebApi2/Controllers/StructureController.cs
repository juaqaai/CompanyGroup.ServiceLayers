using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// struktúrával kapcsolatos műveletek
    /// </summary>
    public class StructureController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.WebshopModule.IStructureService service;

        /// <summary>
        /// konstruktor struktúrákkal kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public StructureController(CompanyGroup.ApplicationServices.WebshopModule.IStructureService service)
        {
            this.service = service;
        }

        /// <summary>
        /// struktúrák lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("GetStructures")]
        [HttpPost]
        public HttpResponseMessage GetStructures(CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.Structures response = this.service.GetAll(request);
                
                return Request.CreateResponse<CompanyGroup.Dto.WebshopModule.Structures>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("ShoppingCartController GetStructures {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }
    }
}
