using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás válaszüzenet
    /// </summary>
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(ElementName = "ChangePasswordResponse", Namespace = "http://CompanyGroup.Domain.PartnerModule/ChangePasswordResponse")]
    public class ChangePasswordCreateResult : System.Xml.Serialization.IXmlSerializable
    {
        /*
         Üzenet (04:12:01)
        <?xml version="1.0" encoding="utf-16"?><ChangePasswordResponse xmlns="http://CompanyGroup.Domain.PartnerModule/ChangePasswordResponse"><Code>1</Code><Message>Succeeded</Message><DataAreaId>hrp</DataAreaId></ChangePassword>
         * 
xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><ChangePassword xmlns=\"http://CompanyGroup.Domain.PartnerModule/ChangePasswordCreateResult\"><Code>1</Code><Message>The operation successfully completed!</Message><DataAreaId>bsc</DataAreaId></ChangePassword>"         
         */

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

        [System.Xml.Serialization.XmlElement(ElementName = "Code", Order = 1)]
        public int Code { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Message", Order = 2)]
        public string Message { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "DataAreaId", Order = 3)]
        public string DataAreaId { get; set; }

        public bool Succeeded { get { return this.Code.Equals(1); } }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Code", String.Format("{0}", this.Code));
            writer.WriteElementString("Message", this.Message);
            writer.WriteElementString("DataAreaId", this.DataAreaId);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent(); 
            this.Code = reader.ReadElementContentAsInt();
            this.Message = reader.ReadElementContentAsString();
            this.DataAreaId = reader.ReadElementContentAsString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ChangePasswordCreateResult Create(int code, string dataAreaId)
        {
            string message = code.Equals(1) ? "Change password operation successfully completed!" : "Change password operation failed!";

            return new ChangePasswordCreateResult() { Code = code, DataAreaId = dataAreaId, Message = message };
        }
    }
}
