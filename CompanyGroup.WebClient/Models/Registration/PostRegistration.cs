using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class PostRegistration : CompanyGroup.Dto.ServiceResponse.PostRegistration
    {
        public PostRegistration(CompanyGroup.Dto.ServiceResponse.PostRegistration registration)
        {
            this.Message = registration.Message;

            this.Successed = registration.Successed;
        }
    }
}