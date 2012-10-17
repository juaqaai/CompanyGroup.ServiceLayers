using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás válaszüzenet
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "ChangePasswordResult", Namespace = "http://CompanyGroup.Domain.PartnerModule/ChangePasswordResult")]
    public class ChangePasswordCreateResult : System.Xml.Serialization.IXmlSerializable
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

        public int ResultCode { get; set; }

        public string Message { get; set; }

        public string DataAreaId { get; set; }

        public bool Succeeded { get { return this.ResultCode.Equals(1); } }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("ResultCode", String.Format("{0}", this.ResultCode));
            writer.WriteElementString("Message", this.Message);
            writer.WriteElementString("DataAreaId", this.DataAreaId);
            writer.WriteEndElement();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            this.ResultCode = reader.ReadContentAsInt();
            this.Message = reader.ReadElementString();
            this.DataAreaId = reader.ReadElementString();
        }
    }
}
