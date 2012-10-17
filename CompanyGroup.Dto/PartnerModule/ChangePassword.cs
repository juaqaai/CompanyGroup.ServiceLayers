using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "ChangePassword", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class ChangePassword
    {
        [System.Runtime.Serialization.DataMember(Name = "OperationSucceeded", Order = 1)]
        public bool OperationSucceeded { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "SendMailSucceeded", Order = 2)]
        public bool SendMailSucceeded { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 3)]
        public string Message { set; get; }
    }
}
