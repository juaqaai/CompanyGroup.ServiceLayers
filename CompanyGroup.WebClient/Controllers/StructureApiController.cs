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
        public CompanyGroup.WebClient.Models.Structures GetStructure(CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request)
        {
            HttpResponseMessage response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllStructureRequest>("Structure", "GetStructures", request);

            CompanyGroup.Dto.WebshopModule.Structures structures = (response.IsSuccessStatusCode) ? response.Content.ReadAsAsync<CompanyGroup.Dto.WebshopModule.Structures>().Result : new CompanyGroup.Dto.WebshopModule.Structures();

            CompanyGroup.WebClient.Models.Structures viewModel = new CompanyGroup.WebClient.Models.Structures(structures);

            return viewModel;
        }


    }
}
