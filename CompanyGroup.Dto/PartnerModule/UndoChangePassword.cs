using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "UndoChangePassword", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class UndoChangePassword
    {
        [System.Runtime.Serialization.DataMember(Name = "Succeeded", Order = 1)]
        public bool Succeeded { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 3)]
        public string Message { set; get; }
    }
}
