using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "SalesOrderLine", Namespace = "http://CompanyGroup.Domain.WebshopModule/SalesOrderLine")]
    public class SalesOrderLineCreate : System.Xml.Serialization.IXmlSerializable
    {
        private string _ItemId = String.Empty;

        private int _Qty = 0;

        private string _ConfigId = String.Empty;

        public string ItemId { get { return _ItemId; } set { _ItemId = value; } }

        public int Qty { get { return _Qty; } set { _Qty = value; } }

        public string ConfigId { get { return _ConfigId; } set { _ConfigId = value; } }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _ConfigId = reader.ReadElementString();
            _ItemId = reader.ReadElementString();
            _Qty = int.Parse(reader.ReadElementString());
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("ConfigId", _ConfigId);
            writer.WriteElementString("ItemId", _ItemId);
            writer.WriteElementString("Qty", Convert.ToString(_Qty));
        }

        #endregion
    }
}
