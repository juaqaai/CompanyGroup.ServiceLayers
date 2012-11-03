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

        [HttpPost]
        [ActionName("ChangePassword")]
        public CompanyGroup.Dto.PartnerModule.ChangePassword ChangePassword(CompanyGroup.Dto.ServiceRequest.ChangePassword request)
        {
            return service.ChangePassword(request);
        }

        [HttpPost]
        [ActionName("UndoChangePassword")]
        public CompanyGroup.Dto.PartnerModule.UndoChangePassword UndoChangePassword(CompanyGroup.Dto.ServiceRequest.UndoChangePassword request)
        {
            return service.UndoChangePassword(request);
        }

        [HttpPost]
        [ActionName("GetById")]
        public CompanyGroup.Dto.PartnerModule.ContactPerson GetById(CompanyGroup.Dto.ServiceRequest.GetContactPersonById request)
        {
            CompanyGroup.Dto.PartnerModule.ContactPerson response = service.GetContactPersonById(request);

            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return response;
        }

        [HttpPost]
        [ActionName("ForgetPassword")]
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
