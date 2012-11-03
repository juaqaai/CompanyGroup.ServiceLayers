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
    public class StructureController : ApiController
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
        [ActionName("GetAll")]
        [HttpPost]
        public CompanyGroup.Dto.WebshopModule.Structures GetAll(CompanyGroup.Dto.ServiceRequest.GetAllStructure request)
        {
            return this.service.GetAll(request);
        }
    }
}
