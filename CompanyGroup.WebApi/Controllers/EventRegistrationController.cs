using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// eseményregisztrációk API kontroller
    /// </summary>
    public class EventRegistrationController : ApiBaseController
    {
        private CompanyGroup.ApplicationServices.PartnerModule.IEventRegistrationService service;

        /// <summary>
        /// konstruktor szerviz paraméterrel
        /// </summary>
        /// <param name="service"></param>
        public EventRegistrationController(CompanyGroup.ApplicationServices.PartnerModule.IEventRegistrationService service)
        {
            this.service = service;
        }

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ActionName("AddNew")]
        [HttpPost]
        public HttpResponseMessage AddNew(CompanyGroup.Dto.PartnerModule.EventRegistration request)
        { 
            try
            {
                bool response = service.AddNew(request);

                return Request.CreateResponse<bool>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("EventRegistrationController AddNew {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }
    }
}
