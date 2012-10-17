using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "ChangePassword", Namespace = "http://CompanyGroup.Domain.PartnerModule/ChangePassword")]
    public class ChangePasswordCreate : System.Xml.Serialization.IXmlSerializable
    {
        public string ContactPersonId { get; set; }

        public string DataAreaId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string WebLoginName { get; set; }


        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("ContactPersonId", this.ContactPersonId);
            writer.WriteElementString("DataAreaId", this.DataAreaId);
            writer.WriteElementString("OldPassword", this.OldPassword);
            writer.WriteElementString("NewPassword", this.NewPassword);
            writer.WriteElementString("WebLoginName", this.WebLoginName);

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

            this.ContactPersonId = reader.ReadElementString();
            this.DataAreaId = reader.ReadElementString();
            this.OldPassword = reader.ReadElementString();
            this.NewPassword = reader.ReadElementString();
            this.WebLoginName = reader.ReadElementString();
        }
    }
}
