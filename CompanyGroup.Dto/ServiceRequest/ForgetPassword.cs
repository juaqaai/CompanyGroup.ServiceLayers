using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "ForgetPassword", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class ForgetPassword
    {
        public ForgetPassword() : this("", "") { }

        public ForgetPassword(string language, string userName)
        { 
            Language = language;
            UserName = userName;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Language", Order = 1)]
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "UserName", Order = 2)]
        public string UserName { get; set; }
    }
}
