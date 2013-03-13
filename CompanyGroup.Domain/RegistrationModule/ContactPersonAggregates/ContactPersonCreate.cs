using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// kapcsolattartó adatokat összefogó osztály 
    /// REGISTRATIONMETHOD GENDER LASTNAME FIRSTNAME EMAIL CELLULARPHONE PHONE PHONELOCAL FAX FUNCTIONID 
    /// ALLOWORDER ALLOWRECEIPTOFGOODS EMAILARRIVEOFGOODS EMAILOFORDERCONFIRM EMAILOFDELIVERY WEBADMIN 
    /// PRICELISTDOWNLOAD INVOICEINFO DIRECTOR FINANCEMANAGER SALESMANAGER SIMPLECONTACT SIMPLESALES 
    /// LEFTCOMPANY REGISTRATIONSTATE REFRECID PWD WEBLOGINNAME REFTABLEID CONTACTPERSONID 
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "ContactPersonReg", Namespace = "http://Shared.Web.Dynamics.Entities/ContactPersonReg")]
    public class ContactPersonCreate : System.Xml.Serialization.IXmlSerializable
    {
        private int _AllowOrder = 0;
        private int _AllowReceiptOfGoods = 0;
        private string _CellularPhone = String.Empty;
        private string _ContactPersonId = String.Empty;
        private string _DataAreaId = String.Empty;
        private int _Director = 0;
        private string _Email = String.Empty;
        private int _EmailArriveOfGoods = 0;
        private int _EmailOfDelivery = 0;
        private int _EmailOfOrderConfirm = 0;
        private string _Fax = String.Empty;
        private int _FinanceManager = 0;
        private string _FirstName = String.Empty;
        private string _FunctionId = String.Empty;
        private int _Gender = 0;
        private int _InvoiceInfo = 0;
        private string _LastName = String.Empty;
        private int _LeftCompany = 0;
        private int _Method = 0;
        private int _Newsletter = 0;
        private string _Phone = String.Empty;
        private string _PhoneLocal = String.Empty;
        private int _PriceListDownload = 0;
        private long _RecId = 0;
        private long _RefRecId = 0;
        private int _SalesManager = 0;
        private int _SimpleContact = 0;
        private int _SimpleSales = 0;
        private int _WebAdmin = 0;
        private string _WebLoginName = String.Empty;
        private string _WebPassword = String.Empty;

        public int AllowOrder
        {
            set { _AllowOrder = value; }
            get { return _AllowOrder; }
        }

        public int AllowReceiptOfGoods
        {
            set { _AllowReceiptOfGoods = value; }
            get { return _AllowReceiptOfGoods; }
        }

        public string CellularPhone
        {
            set { _CellularPhone = value; }
            get { return _CellularPhone; }
        }

        public string ContactPersonId
        {
            set { _ContactPersonId = value; }
            get { return _ContactPersonId; }
        }

        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        public int Director
        {
            get { return _Director; }
            set { _Director = value; }
        }

        public string Email
        {
            set { _Email = value; }
            get { return _Email; }
        }

        public int EmailArriveOfGoods
        {
            set { _EmailArriveOfGoods = value; }
            get { return _EmailArriveOfGoods; }
        }

        public int EmailOfDelivery
        {
            set { _EmailOfDelivery = value; }
            get { return _EmailOfDelivery; }
        }

        public int EmailOfOrderConfirm
        {
            set { _EmailOfOrderConfirm = value; }
            get { return _EmailOfOrderConfirm; }
        }

        public string Fax
        {
            set { _Fax = value; }
            get { return _Fax; }
        }

        public int FinanceManager
        {
            get { return _FinanceManager; }
            set { _FinanceManager = value; }
        }

        public string FirstName
        {
            set { _FirstName = value; }
            get { return _FirstName; }
        }

        public string FunctionId
        {
            set { _FunctionId = value; }
            get { return _FunctionId; }
        }

        public int Gender
        {
            set { _Gender = value; }
            get { return _Gender; }
        }

        public int InvoiceInfo
        {
            get { return _InvoiceInfo; }
            set { _InvoiceInfo = value; }
        }

        public string LastName
        {
            set { _LastName = value; }
            get { return _LastName; }
        }

        public int LeftCompany
        {
            get { return _LeftCompany; }
            set { _LeftCompany = value; }
        }

        public int Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        public int Newsletter
        {
            get { return _Newsletter; }
            set { _Newsletter = value; }
        }

        public string Phone
        {
            set { _Phone = value; }
            get { return _Phone; }
        }

        public string PhoneLocal
        {
            get { return _PhoneLocal; }
            set { _PhoneLocal = value; }
        }

        public int PriceListDownload
        {
            get { return _PriceListDownload; }
            set { _PriceListDownload = value; }
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

        public int SalesManager
        {
            get { return _SalesManager; }
            set { _SalesManager = value; }
        }

        public int SimpleContact
        {
            get { return _SimpleContact; }
            set { _SimpleContact = value; }
        }

        public int SimpleSales
        {
            get { return _SimpleSales; }
            set { _SimpleSales = value; }
        }

        public int WebAdmin
        {
            set { _WebAdmin = value; }
            get { return _WebAdmin; }
        }

        public string WebLoginName
        {
            set { _WebLoginName = value; }
            get { return _WebLoginName; }
        }

        public string WebPassword
        {
            set { _WebPassword = value; }
            get { return _WebPassword; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("AllowOrder", String.Format("{0}", _AllowOrder));
            writer.WriteElementString("AllowReceiptOfGoods", String.Format("{0}", _AllowReceiptOfGoods));
            writer.WriteElementString("CellularPhone", _CellularPhone);
            writer.WriteElementString("ContactPersonId", _ContactPersonId);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("Director", String.Format("{0}", _Director));
            writer.WriteElementString("Email", _Email);
            writer.WriteElementString("EmailArriveOfGoods", String.Format("{0}", _EmailArriveOfGoods));
            writer.WriteElementString("EmailOfDelivery", String.Format("{0}", _EmailOfDelivery));
            writer.WriteElementString("EmailOfOrderConfirm", String.Format("{0}", _EmailOfOrderConfirm));
            writer.WriteElementString("Fax", _Fax);
            writer.WriteElementString("FinanceManager", String.Format("{0}", _FinanceManager));
            writer.WriteElementString("FirstName", _FirstName);
            writer.WriteElementString("FunctionId", _FunctionId);
            writer.WriteElementString("Gender", String.Format("{0}", _Gender));
            writer.WriteElementString("InvoiceInfo", String.Format("{0}", _InvoiceInfo));
            writer.WriteElementString("LastName", _LastName);
            writer.WriteElementString("LeftCompany", String.Format("{0}", _LeftCompany));
            writer.WriteElementString("Method", String.Format("{0}", _Method));
            writer.WriteElementString("Newsletter", String.Format("{0}", _Newsletter));
            writer.WriteElementString("Phone", _Phone);
            writer.WriteElementString("PhoneLocal", _PhoneLocal);
            writer.WriteElementString("PriceListDownload", String.Format("{0}", _PriceListDownload));
            writer.WriteElementString("RecId", String.Format("{0}", _RecId));
            writer.WriteElementString("RefRecId", String.Format("{0}", _RefRecId));
            writer.WriteElementString("SalesManager", String.Format("{0}", _SalesManager));
            writer.WriteElementString("SimpleContact", String.Format("{0}", _SimpleContact));
            writer.WriteElementString("SimpleSales", String.Format("{0}", _SimpleSales));
            writer.WriteElementString("WebAdmin", String.Format("{0}", _WebAdmin));
            writer.WriteElementString("WebLoginName", _WebLoginName);
            writer.WriteElementString("WebPassword", _WebPassword);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _AllowOrder = int.Parse(reader.ReadElementString());
            _AllowReceiptOfGoods = int.Parse(reader.ReadElementString());
            _CellularPhone = reader.ReadElementString();
            _ContactPersonId = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _Director = int.Parse(reader.ReadElementString());
            _Email = reader.ReadElementString();
            _EmailArriveOfGoods = int.Parse(reader.ReadElementString());
            _EmailOfDelivery = int.Parse(reader.ReadElementString());
            _EmailOfOrderConfirm = int.Parse(reader.ReadElementString());
            _Fax = reader.ReadElementString();
            _FinanceManager = int.Parse(reader.ReadElementString());
            _FirstName = reader.ReadElementString();
            _FunctionId = reader.ReadElementString();
            _Gender = int.Parse(reader.ReadElementString());
            _InvoiceInfo = int.Parse(reader.ReadElementString());
            _LastName = reader.ReadElementString();
            _LeftCompany = int.Parse(reader.ReadElementString());
            _Method = int.Parse(reader.ReadElementString());
            _Newsletter = int.Parse(reader.ReadElementString());
            _Phone = reader.ReadElementString();
            _PhoneLocal = reader.ReadElementString();
            _PriceListDownload = int.Parse(reader.ReadElementString());
            _RecId = long.Parse(reader.ReadElementString());
            _RefRecId = long.Parse(reader.ReadElementString());
            _SalesManager = int.Parse(reader.ReadElementString());
            _SimpleContact = int.Parse(reader.ReadElementString());
            _SimpleSales = int.Parse(reader.ReadElementString());
            _WebAdmin = int.Parse(reader.ReadElementString());
            _WebLoginName = reader.ReadElementString();
            _WebPassword = reader.ReadElementString();
        }

    }
}
