using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "ForgetPassword", Namespace = "http://CompanyGroup.Domain.PartnerModule/ForgetPassword")]
    public class ForgetPasswordCreate : System.Xml.Serialization.IXmlSerializable
    {
        public string DataAreaId { get; set; }

        public string WebLoginName { get; set; }


        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("DataAreaId", this.DataAreaId);
            writer.WriteElementString("WebLoginName", this.WebLoginName);
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
            this.DataAreaId = reader.ReadElementString();
            this.WebLoginName = reader.ReadElementString();
        }
    }
}
