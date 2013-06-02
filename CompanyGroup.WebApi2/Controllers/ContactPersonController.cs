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

        /// <summary>
        /// konstruktor service paraméterrel
        /// </summary>
        /// <param name="service"></param>
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
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("ContactPersonController ChangePassword {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

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
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("ContactPersonController UndoChangePassword {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

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
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();

                logWriter.Write(String.Format("ContactPersonController GetById {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

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
                Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter logWriter = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Logging.LogWriter>();
               
                logWriter.Write(String.Format("ContactPersonController ForgetPassword {0} {1} {2}", ex.Message, ex.Source, ex.StackTrace), "Failure Category");

                return ThrowHttpError(ex);
            }
        }

    }
}
