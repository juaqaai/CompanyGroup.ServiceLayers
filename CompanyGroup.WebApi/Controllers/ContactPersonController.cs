using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{

    public class ContactPersonController : ApiController
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

        [AttributeRouting.Web.Http.POST("ChangePassword")]
        public CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.ServiceRequest.ChangePassword request)
        {
            return service.ChangePassword(request);
        }

        [AttributeRouting.Web.Http.POST("UndoChangePassword")]
        public CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.ServiceRequest.UndoChangePassword request)
        {
            return service.UndoChangePassword(request);
        }

        [AttributeRouting.Web.Http.POST("ContactPerson")]
        //[AttributeRouting.Web.Http.GET("ContactPerson/{visitorId}/{languageId}")]
        public CompanyGroup.Dto.PartnerModule.ContactPerson GetContactPersonById(string visitorId, string languageId)
        {
            CompanyGroup.Dto.ServiceRequest.GetContactPersonById request = new Dto.ServiceRequest.GetContactPersonById() { LanguageId = languageId, VisitorId = visitorId };

            CompanyGroup.Dto.PartnerModule.ContactPerson response = service.GetContactPersonById(request);

            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return response;
        }

        [AttributeRouting.Web.Http.POST("ForgetPassword")]
        public CompanyGroup.Dto.PartnerModule.ForgetPassword ForgetPassword(CompanyGroup.Dto.ServiceRequest.ForgetPassword request)
        {
            return service.ForgetPassword(request);
        }

        //// GET api/contactperson
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/contactperson/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/contactperson
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/contactperson/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/contactperson/5
        //public void Delete(int id)
        //{
        //}
    }
}
