using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// cégregisztráció eredményadatokat összefogó osztály
    /// 
    ///xml = strFmt('<?xml version=\"1.0\" encoding=\"utf-16\"?>' +
    ///      '<CustomerRegResult xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://Shared.Web.Dynamics.Entities/CustomerRegResult\">' +
    ///      '<CustomerId>%1</CustomerId>' +
    ///      '<DataAreaId>%2</DataAreaId>' +
    ///      '<RecId>%3</RecId>' +
    ///      '<RegId>%4</RegId>' +
    ///      '<Result>%5</Result>' +
    ///      '</CustomerRegResult>', _customerId, curExt(), _recId, _regId, _result);                 
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "CustomerRegResult", Namespace = "http://Shared.Web.Dynamics.Entities/CustomerRegResult")]
    public class CustomerCreateResult : System.Xml.Serialization.IXmlSerializable
    {
        private string _CustomerId = String.Empty;
        private string _DataAreaId = String.Empty;
        private Int64 _RecId = 0;
        private string _RegId = String.Empty;

        public string CustomerId
        {
            set { _CustomerId = value; }
            get { return _CustomerId; }
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


        public string RegId
        {
            set { _RegId = value; }
            get { return _RegId; }
        }

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _CustomerId = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _RecId = Int64.Parse(reader.ReadElementString());
            _RegId = reader.ReadElementString();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("CustomerId", _CustomerId);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("RecId", Convert.ToString(_RecId));
            writer.WriteElementString("RegId", _RegId);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
