using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// bankszámla adatokat összefogó osztály 
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "BankAccountReg", Namespace = "http://Shared.Web.Dynamics.Entities/BankAccountReg")]
    public class BankAccountCreate : System.Xml.Serialization.IXmlSerializable
    {
        private string _AccountNumber = String.Empty;
        private string _DataAreaId = String.Empty;
        private Int64 _RecId = 0;
        private Int64 _RefRecId = 0;
        private int _Method = 0;

        public string AccountNumber
        {
            set { _AccountNumber = value; }
            get { return _AccountNumber; }
        }

        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        public Int64 RecId
        {
            set { _RecId = value; }
            get { return _RecId; }
        }

        public Int64 RefRecId
        {
            set { _RefRecId = value; }
            get { return _RefRecId; }
        }

        public int Method
        {
            set { _Method = value; }
            get { return _Method; }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _AccountNumber = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _RecId = Int64.Parse(reader.ReadElementString());
            _RefRecId = Int64.Parse(reader.ReadElementString());
            _Method = int.Parse(reader.ReadElementString());
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("AccountNumber", _AccountNumber);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("RecId", Convert.ToString(_RecId));
            writer.WriteElementString("RefRecId", Convert.ToString(_RefRecId));
            writer.WriteElementString("Method", Convert.ToString(_Method));
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
