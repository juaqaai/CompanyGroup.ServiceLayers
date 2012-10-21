using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás válaszüzenet
    /// </summary>
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(ElementName = "ChangePasswordResult", Namespace = "http://CompanyGroup.Domain.PartnerModule/ChangePasswordResult")]
    public class ChangePasswordCreateResult // : System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// jelszómódosítás válaszüzenet
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        //public ChangePasswordCreateResult(int resultCode, string message)
        //{
        //    this.ResultCode = resultCode;

        //    this.Message = message;
        //}

        [System.Xml.Serialization.XmlElement(ElementName = "ResultCode", Order = 1)]
        public int ResultCode { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Message", Order = 2)]
        public string Message { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataAreaId", Order = 3)]
        public string DataAreaId { get; set; }

        public bool Succeeded { get { return this.ResultCode.Equals(1); } }

        //public void WriteXml(System.Xml.XmlWriter writer)
        //{
        //    writer.WriteElementString("ResultCode", String.Format("{0}", this.ResultCode));
        //    writer.WriteElementString("Message", this.Message);
        //    writer.WriteElementString("DataAreaId", this.DataAreaId);
        //}

        //public System.Xml.Schema.XmlSchema GetSchema()
        //{
        //    return (null);
        //}

        //public void ReadXml(System.Xml.XmlReader reader)
        //{
        //    reader.MoveToElement(); //MoveToContent
        //    this.ResultCode = reader.ReadElementContentAsInt();
        //    this.Message = reader.ReadElementContentAsString();
        //    this.DataAreaId = reader.ReadElementContentAsString();
        //}
    }
}
