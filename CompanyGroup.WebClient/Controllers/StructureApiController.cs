using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class StructureApiController : ApiBaseController
    {
        /// <summary>
        /// struktúrák lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetStructure")]
        public CompanyGroup.WebClient.Models.Structures GetStructure(CompanyGroup.Dto.ServiceRequest.GetAllStructure request)
        {
            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllStructure, CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetStructures", request);

            CompanyGroup.WebClient.Models.Structures response = new CompanyGroup.WebClient.Models.Structures(structures);

            return response;
        }


    }
}
