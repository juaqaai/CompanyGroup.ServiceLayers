using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "MailAddrReg", Namespace = "http://Shared.Web.Dynamics.Entities/MailAddrReg")]
    public class MailAddressCreate : System.Xml.Serialization.IXmlSerializable
    {
        private string _country = String.Empty;
        private string _city = String.Empty;
        private string _postCode = String.Empty;
        private string _county = String.Empty;
        private string _street = String.Empty;

        public string City
        {
            set { _city = value; }
            get { return _city; }
        }

        public string Country
        {
            set { _country = value; }
            get { return _country; }
        }

        public string County
        {
            set { _county = value; }
            get { return _county; }
        }

        public string PostCode
        {
            set { _postCode = value; }
            get { return _postCode; }
        }

        public string Street
        {
            set { _street = value; }
            get { return _street; }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _city = reader.ReadElementString();
            _country = reader.ReadElementString();
            _county = reader.ReadElementString();
            _postCode = reader.ReadElementString();
            _street = reader.ReadElementString();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("City", _city);
            writer.WriteElementString("Country", _country);
            writer.WriteElementString("County", _county);
            writer.WriteElementString("PostCode", _postCode);
            writer.WriteElementString("Street", _street);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
