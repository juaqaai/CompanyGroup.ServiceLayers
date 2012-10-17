using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "SalesOrderLine", Namespace = "http://Shared.Web.Dynamics.Entities/SalesOrderLine")]
    public class SalesOrderLineCreate : System.Xml.Serialization.IXmlSerializable
    {
        /*
    ItemId      itemId;
    Real        qty;
    ConfigId    configId;
    str 10      taxItem;
    InventDimId inventDimId;
    int         itemNum;         
         */

        private string _ItemId = String.Empty;

        private int _Qty = 0;

        private string _ConfigId = String.Empty;

        private string _TaxItem = String.Empty;

        private string _InventDimId = String.Empty;

        public string ItemId { get { return _ItemId; } set { _ItemId = value; } }

        public int Qty { get { return _Qty; } set { _Qty = value; } }

        public string ConfigId { get { return _ConfigId; } set { _ConfigId = value; } }

        public string TaxItem { get { return _TaxItem; } set { _TaxItem = value; } }

        public string InventDimId { get { return _InventDimId; } set { _InventDimId = value; } }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _ConfigId = reader.ReadElementString();
            _InventDimId = reader.ReadElementString();
            _ItemId = reader.ReadElementString();
            _TaxItem = reader.ReadElementString();
            _Qty = int.Parse(reader.ReadElementString());
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("ConfigId", _ConfigId);
            writer.WriteElementString("InventDimId", _InventDimId);
            writer.WriteElementString("ItemId", _ItemId);
            writer.WriteElementString("TaxItem", _TaxItem);
            writer.WriteElementString("Qty", Convert.ToString(_Qty));
        }

        #endregion
    }
}
