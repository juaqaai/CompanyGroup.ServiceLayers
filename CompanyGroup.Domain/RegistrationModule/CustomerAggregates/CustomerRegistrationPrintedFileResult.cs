using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{

    [System.Xml.Serialization.XmlRoot(ElementName = "CustomerRegistrationPrintedFileResult", Namespace = "http://Shared.Web.Dynamics.Entities/CustomerRegistrationPrintedFileResult")]
    public class CustomerRegistrationPrintedFileResult : System.Xml.Serialization.IXmlSerializable
    {
        private int _Code = 0;
        private string _Message = String.Empty;

        public int Code
        {
            set { _Code = value; }
            get { return _Code; }
        }

        public string Message
        {
            set { _Message = value; }
            get { return _Message; }
        }

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _Code = Int32.Parse(reader.ReadElementString());
            _Message = reader.ReadElementString();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Code", Convert.ToString(_Code));
            writer.WriteElementString("Message", _Message);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
