using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class VisitorApiController : ApiBaseController
    {
        /// <summary>
        /// látogató adatainak kiolvasása
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetVisitorInfo")]
        public CompanyGroup.WebClient.Models.Visitor GetVisitorInfo()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return visitor;
        }

        /// <summary>
        /// látogatói jogosultságok
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetRoles")]
        public List<string> GetRoles()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.VisitorInfo req = new CompanyGroup.Dto.ServiceRequest.VisitorInfo() { DataAreaId = VisitorApiController.DataAreaId, ObjectId = visitorData.ObjectId };

            List<string> response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.VisitorInfo, List<string>>("Customer", "GetRoles", req);

            return response;
        }

    }
}
