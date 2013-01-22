using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class ForgetPassword
    {
        public ForgetPassword() : this(false, String.Empty) { }

        public ForgetPassword(bool succeeded, string message)
        {
            this.Succeeded = succeeded;

            this.Message = message;
        }

        public bool Succeeded { set; get; }

        public string Message { set; get; }
    }
}
