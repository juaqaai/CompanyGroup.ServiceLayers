using System;

namespace CompanyGroup.Dto.PartnerModule
{
    public class ForgetPasswordRequest
    {
        public ForgetPasswordRequest() : this("", "") { }

        public ForgetPasswordRequest(string language, string userName)
        { 
            Language = language;
            UserName = userName;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public string UserName { get; set; }
    }
}
