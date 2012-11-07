using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class ChangePasswordResponse : CompanyGroup.Dto.PartnerModule.ChangePassword
    {
        public ChangePasswordResponse(CompanyGroup.Dto.PartnerModule.ChangePassword changePassword, Visitor visitor)
        {
            this.Message = changePassword.Message;

            this.OperationSucceeded = changePassword.OperationSucceeded;

            this.SendMailSucceeded = changePassword.SendMailSucceeded;

            this.Visitor = visitor; 
        }

        public Visitor Visitor { get; set; }
    }
}