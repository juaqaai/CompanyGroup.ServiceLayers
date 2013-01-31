using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// kapcsolattartókkal kapcsolatos műveletek
    /// </summary>
    public class ContactPersonController : ApiBaseController
    {

        private CompanyGroup.ApplicationServices.PartnerModule.IContactPersonService service;

        public ContactPersonController(CompanyGroup.ApplicationServices.PartnerModule.IContactPersonService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("ContactPersonService");
            }

            this.service = service;
        }


        [HttpPost]
        [ActionName("ChangePassword")]
        public HttpResponseMessage ChangePassword(CompanyGroup.Dto.PartnerModule.ChangePasswordRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.ChangePassword response = service.ChangePassword(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.ChangePassword>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [HttpPost]
        [ActionName("UndoChangePassword")]
        public HttpResponseMessage UndoChangePassword(CompanyGroup.Dto.PartnerModule.UndoChangePasswordRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.UndoChangePassword response = service.UndoChangePassword(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.UndoChangePassword>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [HttpPost]
        [ActionName("GetById")]
        public HttpResponseMessage GetById(CompanyGroup.Dto.PartnerModule.GetContactPersonByIdRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.ContactPerson response = service.GetContactPersonById(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.ContactPerson>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        [HttpPost]
        [ActionName("ForgetPassword")]
        public HttpResponseMessage ForgetPassword(CompanyGroup.Dto.PartnerModule.ForgetPasswordRequest request)
        {
            try
            {
                CompanyGroup.Dto.PartnerModule.ForgetPassword response = service.ForgetPassword(request);

                return Request.CreateResponse<CompanyGroup.Dto.PartnerModule.ForgetPassword>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

    }
}
