using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// kapcsolattartóhoz kapcsolt műveletek
    /// </summary>
    public class ContactPersonApiController : ApiBaseController
    {
        /// <summary>
        /// jelszómódosítás művelet
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ChangePwd")]
        public CompanyGroup.WebClient.Models.ChangePasswordResponse ChangePwd(CompanyGroup.WebClient.Models.ChangePasswordRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.ChangePassword req = new CompanyGroup.Dto.ServiceRequest.ChangePassword()
            {
                VisitorId = visitorData.ObjectId,
                Language = visitorData.Language,
                NewPassword = request.NewPassword,
                OldPassword = request.OldPassword,
                UserName = request.UserName
            };

            CompanyGroup.Dto.PartnerModule.ChangePassword response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.ChangePassword, CompanyGroup.Dto.PartnerModule.ChangePassword>("ContactPerson", "ChangePassword", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ChangePasswordResponse( response, visitor);
        }

        /// <summary>
        /// jelszóemlékeztező művelet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ForgetPwd")]
        public CompanyGroup.WebClient.Models.ForgetPasswordResponse ForgetPwd(CompanyGroup.WebClient.Models.ForgetPasswordRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.ForgetPassword req = new CompanyGroup.Dto.ServiceRequest.ForgetPassword()
            {
                Language = visitorData.Language,
                UserName = request.UserName
            };

            CompanyGroup.Dto.PartnerModule.ForgetPassword response = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.ForgetPassword, CompanyGroup.Dto.PartnerModule.ForgetPassword>("ContactPerson", "ForgetPassword", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            return new CompanyGroup.WebClient.Models.ForgetPasswordResponse(response, visitor);
        }


    }
}
