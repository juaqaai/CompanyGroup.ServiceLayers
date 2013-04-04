using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// használt cikk adatokat összefogó osztály
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "SecondhandOrder", Namespace = "http://CompanyGroup.Domain.WebshopModule/SecondhandOrder")]
    public class SecondhandOrderCreate : System.Xml.Serialization.IXmlSerializable
    {
        string _ContactPersonId = String.Empty;
        string _CustomerId = String.Empty;
        string _CustomerRef = String.Empty;
        string _CustomerOrderNo = String.Empty;
        string _DataAreaId = String.Empty;
        string _DeliveryCity = String.Empty;
        string _DeliveryName = String.Empty;
        string _DeliveryDate = String.Empty;
        string _DeliveryEmail = String.Empty;
        string _DeliveryStreet = String.Empty;
        string _DeliveryZip = String.Empty;
        string _InventLocationId = String.Empty;
        int _LineCount = 0;
        string _Payment = String.Empty;
        bool _RequiredDelivery = false;

        private List<SecondhandOrderLineCreate> _Lines = new List<SecondhandOrderLineCreate>();

        public string ContactPersonId { get { return _ContactPersonId; } set { _ContactPersonId = value; } }

        public string CustomerId { get { return _CustomerId; } set { _CustomerId = value; } }

        public string CustomerRef { get { return _CustomerRef; } set { _CustomerRef = value; } }

        public string CustomerOrderNo { get { return _CustomerOrderNo; } set { _CustomerOrderNo = value; } }

        public string DataAreaId { get { return _DataAreaId; } set { _DataAreaId = value; } }

        public string DeliveryCity { get { return _DeliveryCity; } set { _DeliveryCity = value; } }

        public string DeliveryName { get { return _DeliveryName; } set { _DeliveryName = value; } }

        public string DeliveryDate { get { return _DeliveryDate; } set { _DeliveryDate = value; } }

        public string DeliveryEmail { get { return _DeliveryEmail; } set { _DeliveryEmail = value; } }

        public string DeliveryStreet { get { return _DeliveryStreet; } set { _DeliveryStreet = value; } }

        public string DeliveryZip { get { return _DeliveryZip; } set { _DeliveryZip = value; } }

        public string InventLocationId { get { return _InventLocationId; } set { _InventLocationId = value; } }

        public int LineCount { get { return _LineCount; } set { _LineCount = value; } }

        public string Payment { get { return _Payment; } set { _Payment = value; } }        

        public bool RequiredDelivery { get { return _RequiredDelivery; } set { _RequiredDelivery = value; } }

        public List<SecondhandOrderLineCreate> Lines
        {
            get { return _Lines; }
            set { _Lines = value; }
        }

        private string CompleteString(string s)
        {
            return (s.Length == 1) ? "0" + s : s;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("ContactPersonId", _ContactPersonId);
            writer.WriteElementString("CustomerId", _CustomerId);
            writer.WriteElementString("CustomerOrderNo", _CustomerOrderNo);
            writer.WriteElementString("CustomerRef", _CustomerRef);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("DeliveryCity", _DeliveryCity);
            writer.WriteElementString("DeliveryDate", _DeliveryDate);
            writer.WriteElementString("DeliveryEmail", _DeliveryEmail);
            writer.WriteElementString("DeliveryName", _DeliveryName);
            writer.WriteElementString("DeliveryStreet", _DeliveryStreet);
            writer.WriteElementString("DeliveryZip", _DeliveryZip);
            writer.WriteElementString("InventLocationId", _InventLocationId);
            writer.WriteElementString("Payment", _Payment);
            writer.WriteElementString("RequiredDelivery", String.Format("{0}", _RequiredDelivery ? 1 : 0));
            writer.WriteElementString("LineCount", String.Format("{0}", _LineCount));
            writer.WriteStartElement("Lines");
            foreach (SecondhandOrderLineCreate line in _Lines)
            {
                writer.WriteStartElement("Line");
                line.WriteXml(writer);
                writer.WriteEndElement();
            }
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

            _ContactPersonId = reader.ReadElementString();
            _CustomerId = reader.ReadElementString();
            _CustomerOrderNo = reader.ReadElementString();
            _CustomerRef = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _DeliveryCity = reader.ReadElementString();
            _DeliveryDate = reader.ReadElementString();
            _DeliveryEmail = reader.ReadElementString();
            _DeliveryName = reader.ReadElementString();
            _DeliveryStreet = reader.ReadElementString();
            _DeliveryZip = reader.ReadElementString();
            _InventLocationId = reader.ReadElementString();
            _Payment = reader.ReadElementString();
            _RequiredDelivery = int.Parse(reader.ReadElementString()) > 0;
            _LineCount = int.Parse(reader.ReadElementString());
            _Lines.Clear();

            int depth = reader.Depth;
            bool read = reader.Read();
            while ((read) && (reader.Depth > depth))
            {
                reader.MoveToContent();

                if (reader.IsStartElement())
                {
                    SecondhandOrderLineCreate line = new SecondhandOrderLineCreate();
                    line.ReadXml(reader);
                    _Lines.Add(line);
                }

                read = reader.Read();
            }
        }

    }
}
