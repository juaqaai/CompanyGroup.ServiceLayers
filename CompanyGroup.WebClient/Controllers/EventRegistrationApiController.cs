using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class EventRegistrationApiController : ApiBaseController
    {
        /// <summary>
        /// eseményregisztráció hozzáadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddNew")]
        public HttpResponseMessage AddNew(CompanyGroup.WebClient.Models.EventRegistration request)
        {
            try
            {
                //kérés objektum összeállítása a webapi -nak
                CompanyGroup.Dto.PartnerModule.EventRegistration req = new CompanyGroup.Dto.PartnerModule.EventRegistration(request.EventId, request.EventName, request.Data);

                //webapi hívás, válasz előállítása
                bool response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.EventRegistration, bool>("EventRegistration", "AddNew", req);

                HttpResponseMessage httpResponseMessage = Request.CreateResponse<bool>(HttpStatusCode.Created, response);

                return httpResponseMessage;
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }
    }
}
