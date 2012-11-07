using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ForgetPasswordResponse : CompanyGroup.Dto.PartnerModule.ForgetPassword
    {
        public ForgetPasswordResponse(CompanyGroup.Dto.PartnerModule.ForgetPassword forgetPassword, Visitor visitor)
        {
            this.Message = forgetPassword.Message;

            this.Succeeded = forgetPassword.Succeeded;

            this.Visitor = visitor; 
        }

        public Visitor Visitor { get; set; }
    }
}