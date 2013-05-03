using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
     
    /// <summary>
    /// bankszámla adatokat összefogó osztály 
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "CustomerRegistrationPrintedFile", Namespace = "http://Shared.Web.Dynamics.Entities/CustomerRegistrationPrintedFile")]
    public class CustomerRegistrationPrintedFile : System.Xml.Serialization.IXmlSerializable
    {
        private string _FileName = String.Empty;
        private string _DataAreaId = String.Empty;
        private Int64 _RecId = 0;

        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        public string FileName
        {
            set { _FileName = value; }
            get { return _FileName; }
        }

        public Int64 RecId
        {
            set { _RecId = value; }
            get { return _RecId; }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _DataAreaId = reader.ReadElementString();
            _FileName = reader.ReadElementString();
            _RecId = Int64.Parse(reader.ReadElementString());
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("FileName", _FileName);
            writer.WriteElementString("RecId", Convert.ToString(_RecId));
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
