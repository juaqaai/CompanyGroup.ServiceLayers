using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    /// <summary>
    /// kapcsolattartóhoz kapcsolt műveletek (jelszómódosítás, jelszóemlékeztező, jelszómódosítás visszavonás)
    /// </summary>
    public class ContactPersonApiController : ApiBaseController
    {
        /// <summary>
        /// jelszómódosítás művelet
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ChangePwd")]
        public HttpResponseMessage ChangePwd(CompanyGroup.WebClient.Models.ChangePasswordRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.PartnerModule.ChangePasswordRequest req = new CompanyGroup.Dto.PartnerModule.ChangePasswordRequest()
            {
                VisitorId = visitorData.VisitorId,
                Language = visitorData.Language,
                NewPassword = request.NewPassword,
                OldPassword = request.OldPassword,
                UserName = request.UserName
            };

            CompanyGroup.Dto.PartnerModule.ChangePassword response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ChangePasswordRequest, CompanyGroup.Dto.PartnerModule.ChangePassword>("ContactPerson", "ChangePassword", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.WebClient.Models.ChangePasswordResponse changePasswordResponse = new CompanyGroup.WebClient.Models.ChangePasswordResponse( response, visitor);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.ChangePasswordResponse>(HttpStatusCode.Created, changePasswordResponse);

            return httpResponseMessage;
        }

        /// <summary>
        /// jelszóemlékeztező művelet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ForgetPwd")]
        public HttpResponseMessage ForgetPwd(CompanyGroup.WebClient.Models.ForgetPasswordRequest request)
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest req = new CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest()
            {
                Language = visitorData.Language,
                UserName = request.UserName
            };

            CompanyGroup.Dto.PartnerModule.ForgetPassword response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest, CompanyGroup.Dto.PartnerModule.ForgetPassword>("ContactPerson", "ForgetPassword", req);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.WebClient.Models.ForgetPasswordResponse forgetPasswordResponse = new CompanyGroup.WebClient.Models.ForgetPasswordResponse(response, visitor);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse<CompanyGroup.WebClient.Models.ForgetPasswordResponse>(HttpStatusCode.Created, forgetPasswordResponse);

            return httpResponseMessage;
        }

        /// <summary>
        /// jelszómódosítást visszavonó művelet - view kezdőérték beállításokkal
        /// </summary>
        /// <param name="undoChangePassword"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("UndoChangePassword")]
        public CompanyGroup.WebClient.Models.UndoChangePassword UndoChangePassword(string id)
        {
            string undoChangePassword = id;

            CompanyGroup.Dto.PartnerModule.UndoChangePassword response;

            //üres jleszómódosítás token
            if (String.IsNullOrEmpty(undoChangePassword))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest request = new CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest(undoChangePassword);

            response = this.PostJSonData<CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest, CompanyGroup.Dto.PartnerModule.UndoChangePassword>("ContactPerson", "UndoChangePassword", request);

//          response = new CompanyGroup.Dto.PartnerModule.UndoChangePassword() { Succeeded = false, Message = "A jelszómódosítás visszavonásához tartozó azonosító nem lett megadva!" };

            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.WebClient.Models.UndoChangePassword model = new CompanyGroup.WebClient.Models.UndoChangePassword(response, visitor);

            return model;
        }
    }
}
