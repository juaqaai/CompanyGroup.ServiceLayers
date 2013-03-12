using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// kiszállítási cím adatait összefogó osztály 
    /// CITY POSTCODE STREET COUNTRY COUNTY REFRECID REFTABLEID ADDRESSRECID REGISTRATIONMETHOD 
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "DeliveryAddrReg", Namespace = "http://Shared.Web.Dynamics.Entities/DeliveryAddrReg")]
    public class DeliveryAddressCreate : System.Xml.Serialization.IXmlSerializable
    {
        private string _AddressId = String.Empty;

        private string _City = String.Empty;

        private string _County = String.Empty;

        private string _Country = String.Empty;

        private string _DataAreaId = String.Empty;

        private string _PostCode = String.Empty;

        private Int64 _RecId = 0;

        private Int64 _RefRecId = 0;

        private string _Street = String.Empty;

        public string AddressId
        {
            get { return _AddressId; }
            set { _AddressId = value; }
        }

        public string City
        {
            get { return this._City; }
            set { this._City = value; }
        }

        public string County
        {
            get { return this._County; }
            set { this._County = value; }
        }

        public string Country
        {
            get { return this._Country; }
            set { this._Country = value; }
        }

        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        public string PostCode
        {
            get { return this._PostCode; }
            set { this._PostCode = value; }
        }

        public Int64 RecId
        {
            set { _RecId = value; }
            get { return _RecId; }
        }

        public Int64 RefRecId
        {
            get { return _RefRecId; }
            set { _RefRecId = value; }
        }

        public string Street
        {
            get { return this._Street; }
            set { this._Street = value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _AddressId = reader.ReadElementString();
            _City = reader.ReadElementString();
            _County = reader.ReadElementString();
            _Country = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _PostCode = reader.ReadElementString();
            _RecId = Int64.Parse(reader.ReadElementString());
            _RefRecId = Int64.Parse(reader.ReadElementString());
            _Street = reader.ReadElementString();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("AddressId", _AddressId);
            writer.WriteElementString("City", _City);
            writer.WriteElementString("County", _County);
            writer.WriteElementString("Country", _Country);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("PostCode", _PostCode);
            writer.WriteElementString("RecId", Convert.ToString(_RecId));
            writer.WriteElementString("RefRecId", Convert.ToString(_RefRecId));
            writer.WriteElementString("Street", _Street);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

    }
}
