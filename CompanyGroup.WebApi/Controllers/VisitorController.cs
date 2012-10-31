using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// látogatóval kapcsolatos műveletek
    /// </summary>
    public class VisitorController : ApiController
    {
        private CompanyGroup.ApplicationServices.PartnerModule.IVisitorService service;

        /// <summary>
        /// konstruktor látogatóval kapcsolatos műveletek interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        public VisitorController(CompanyGroup.ApplicationServices.PartnerModule.IVisitorService service)
        { 
            this.service = service;
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó információk kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor GetVisitorInfo(CompanyGroup.Dto.ServiceRequest.VisitorInfo request)
        {
            return service.GetVisitorInfo(request);
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó jogosultságok listázása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<string> GetRoles(CompanyGroup.Dto.ServiceRequest.VisitorInfo request)
        {
            return service.GetRoles(request);
        }

    }
}
