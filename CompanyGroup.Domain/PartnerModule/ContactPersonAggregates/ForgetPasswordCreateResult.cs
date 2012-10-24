using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás válaszüzenet
    /// </summary>
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(ElementName = "ForgetPasswordResult", Namespace = "http://CompanyGroup.Domain.PartnerModule/ForgetPasswordResult")]
    public class ForgetPasswordCreateResult 
    {
        [System.Xml.Serialization.XmlElement(ElementName = "ResultCode", Order = 1)]
        public int ResultCode { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Message", Order = 2)]
        public string Message { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataAreaId", Order = 3)]
        public string DataAreaId { get; set; }

        public bool Succeeded { get { return this.ResultCode.Equals(1); } }
    }
}
