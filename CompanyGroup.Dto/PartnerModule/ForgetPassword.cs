using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "ForgetPassword", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class ForgetPassword
    {
        [System.Runtime.Serialization.DataMember(Name = "Succeeded", Order = 1)]
        public bool Succeeded { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 3)]
        public string Message { set; get; }
    }
}
