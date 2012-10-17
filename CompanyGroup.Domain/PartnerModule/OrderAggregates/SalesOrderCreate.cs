using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "SalesOrder", Namespace = "http://Shared.Web.Dynamics.Entities/SalesOrder")]
    public class SalesOrderCreate : System.Xml.Serialization.IXmlSerializable
    {
        string _ContactPersonId = String.Empty;
        string _CustomerId = String.Empty;
        string _DataAreaId = String.Empty;
        string _DeliveryCity = String.Empty;
        string _DeliveryCompanyName = String.Empty;
        string _DeliveryDate = String.Empty;
        string _DeliveryEmail = String.Empty;
        string _DeliveryId = String.Empty;
        string _DeliveryPersonName = String.Empty;
        string _DeliveryPhone = String.Empty;
        string _DeliveryStreet = String.Empty;
        string _DeliveryZip = String.Empty;
        string _Message = String.Empty;
        bool _PartialDelivery = true;
        bool _RequiredDelivery = false;
        int _SalesSource = 0;
        string _InventLocationId = String.Empty;
        string _Transporter = String.Empty;
        private List<SalesOrderLineCreate> _Lines = new List<SalesOrderLineCreate>();

        public string ContactPersonId { get { return _ContactPersonId; } set { _ContactPersonId = value; } }

        public string CustomerId { get { return _CustomerId; } set { _CustomerId = value; } }

        public string DataAreaId { get { return _DataAreaId; } set { _DataAreaId = value; } }

        public string DeliveryCity { get { return _DeliveryCity; } set { _DeliveryCity = value; } }

        public string DeliveryCompanyName { get { return _DeliveryCompanyName; } set { _DeliveryCompanyName = value; } }

        public string DeliveryDate { get { return _DeliveryDate; } set { _DeliveryDate = value; } }

        public string DeliveryEmail { get { return _DeliveryEmail; } set { _DeliveryEmail = value; } }

        public string DeliveryId { get { return _DeliveryId; } set { _DeliveryId = value; } }

        public string DeliveryPersonName { get { return _DeliveryPersonName; } set { _DeliveryPersonName = value; } }

        public string DeliveryPhone { get { return _DeliveryPhone; } set { _DeliveryPhone = value; } }

        public string DeliveryStreet { get { return _DeliveryStreet; } set { _DeliveryStreet = value; } }

        public string DeliveryZip { get { return _DeliveryZip; } set { _DeliveryZip = value; } }

        public string Message { get { return _Message; } set { _Message = value; } }

        public bool PartialDelivery { get { return _PartialDelivery; } set { _PartialDelivery = value; } }

        public bool RequiredDelivery { get { return _RequiredDelivery; } set { _RequiredDelivery = value; } }

        public int SalesSource { get { return _SalesSource; } set { _SalesSource = value; } }

        public string InventLocationId { get { return _InventLocationId; } set { _InventLocationId = value; } }

        public string Transporter { get { return _Transporter; } set { _Transporter = value; } }

        public List<SalesOrderLineCreate> Lines
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
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("DeliveryCity", _DeliveryCity);
            writer.WriteElementString("DeliveryCompanyName", _DeliveryCompanyName);
            writer.WriteElementString("DeliveryDate", _DeliveryDate);
            writer.WriteElementString("DeliveryEmail", _DeliveryEmail);
            writer.WriteElementString("DeliveryId", _DeliveryId);
            writer.WriteElementString("DeliveryPersonName", _DeliveryPersonName);
            writer.WriteElementString("DeliveryPhone", _DeliveryPhone);
            writer.WriteElementString("DeliveryStreet", _DeliveryStreet);
            writer.WriteElementString("DeliveryZip", _DeliveryZip);
            writer.WriteElementString("Message", _Message);
            writer.WriteElementString("PartialDelivery", String.Format("{0}", _PartialDelivery ? 1 : 0));
            writer.WriteElementString("RequiredDelivery", String.Format("{0}", _RequiredDelivery ? 1 : 0));
            writer.WriteElementString("SalesSource", Convert.ToString(_SalesSource));
            writer.WriteElementString("InventLocationId", _InventLocationId);
            writer.WriteElementString("Transporter", _Transporter);

            writer.WriteStartElement("Lines");
            foreach (SalesOrderLineCreate line in _Lines)
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
            _DataAreaId = reader.ReadElementString();
            _DeliveryCity = reader.ReadElementString();
            _DeliveryCompanyName = reader.ReadElementString();
            _DeliveryDate = reader.ReadElementString();
            _DeliveryEmail = reader.ReadElementString();
            _DeliveryId = reader.ReadElementString();
            _DeliveryPersonName = reader.ReadElementString();
            _DeliveryPhone = reader.ReadElementString();
            _DeliveryStreet = reader.ReadElementString();
            _DeliveryZip = reader.ReadElementString();
            _Message = reader.ReadElementString();
            _PartialDelivery = int.Parse(reader.ReadElementString()) > 0;
            _RequiredDelivery = int.Parse(reader.ReadElementString()) > 0;
            _SalesSource = int.Parse(reader.ReadElementString());
            _InventLocationId = reader.ReadElementString();
            _Transporter = reader.ReadElementString();
            _Lines.Clear();

            int depth = reader.Depth;
            bool read = reader.Read();
            while ((read) && (reader.Depth > depth))
            {
                reader.MoveToContent();

                if (reader.IsStartElement())
                {
                    SalesOrderLineCreate line = new SalesOrderLineCreate();
                    line.ReadXml(reader);
                    _Lines.Add(line);
                }

                read = reader.Read();
            }
        }

    }
}
