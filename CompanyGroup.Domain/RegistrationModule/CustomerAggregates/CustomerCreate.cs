using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// cégregisztrációs adatokat összefogó osztály
    /// REGNAME REGEMAIL REGPHONE HRP BSC REGTYPE OTHERTYPE NAME REGISTERNUMBER VATNUMBER EUVATNUMBER 
    /// SIGNATUREENTITYFILE COMPANYSERTIFICATE 
    /// INVOICECITY INVOICEPOSTCODE INVOICECOUNTRY INVOICECOUNTY INVOICESTREET INVOICEPHONE INVOICEFAX INVOICEEMAIL 
    /// NEWSLETTERSUBSCRIPTIONHRP NEWSLETTERSUBSCRIPTIONBSC 
    /// REGISTRATIONMETHOD ACCOUNTNUM
    /// </summary>
    [System.Xml.Serialization.XmlRoot(ElementName = "CustomerReg", Namespace = "http://Shared.Web.Dynamics.Entities/CustomerReg")]
    public class CustomerCreate : System.Xml.Serialization.IXmlSerializable
    {
        private string _CompanyCertificateFile = String.Empty;
        private string _CustomerId = String.Empty;
        private string _CustomerName = String.Empty;
        private string _DataAreaId = String.Empty;
        private string _InvoiceCity = String.Empty;
        private string _InvoiceCountry = String.Empty;
        private string _InvoiceCounty = String.Empty;
        private string _InvoiceEmail = String.Empty;
        private string _InvoiceFax = String.Empty;
        private string _InvoicePhone = String.Empty;
        private string _InvoicePostCode = String.Empty;
        private string _InvoiceStreet = String.Empty;
        private int _Method = 0;
        private Int64 _RecId = 0;
        private string _RegEmail = String.Empty;
        private string _RegName = String.Empty;
        private string _RegNumber = String.Empty;
        private string _RegPhone = String.Empty;
        private int _NewsletterSubScription = 0;
        private string _SignatureEntityFile = String.Empty;
        private string _VatNumber = String.Empty;
        private ContactPersonCreate _WebAdministrator = new ContactPersonCreate();
        private MailAddressCreate _MailAddress = new MailAddressCreate();
        //private System.Collections.Generic.List<BankAccountReg> _bankAccounts = new System.Collections.Generic.List<BankAccountReg>();

        public string CompanyCertificateFile
        {
            set { _CompanyCertificateFile = value; }
            get { return _CompanyCertificateFile; }
        }

        public string CustomerId
        {
            set { _CustomerId = value; }
            get { return _CustomerId; }
        }

        public string CustomerName
        {
            set { _CustomerName = value; }
            get { return _CustomerName; }
        }

        public string DataAreaId
        {
            set { _DataAreaId = value; }
            get { return _DataAreaId; }
        }

        public string InvoiceCity
        {
            set { _InvoiceCity = value; }
            get { return _InvoiceCity; }
        }

        public string InvoiceCountry
        {
            set { _InvoiceCountry = value; }
            get { return _InvoiceCountry; }
        }

        public string InvoiceCounty
        {
            set { _InvoiceCounty = value; }
            get { return _InvoiceCounty; }
        }

        public string InvoiceEmail
        {
            set { _InvoiceEmail = value; }
            get { return _InvoiceEmail; }
        }

        public string InvoiceFax
        {
            set { _InvoiceFax = value; }
            get { return _InvoiceFax; }
        }

        public string InvoicePhone
        {
            set { _InvoicePhone = value; }
            get { return _InvoicePhone; }
        }

        public string InvoicePostCode
        {
            set { _InvoicePostCode = value; }
            get { return _InvoicePostCode; }
        }

        public string InvoiceStreet
        {
            set { _InvoiceStreet = value; }
            get { return _InvoiceStreet; }
        }

        public int Method
        {
            set { _Method = value; }
            get { return _Method; }
        }

        public Int64 RecId
        {
            set { _RecId = value; }
            get { return _RecId; }
        }

        public string RegEmail
        {
            set { _RegEmail = value; }
            get { return _RegEmail; }
        }

        public string RegName
        {
            set { _RegName = value; }
            get { return _RegName; }
        }

        public string RegNumber
        {
            set { _RegNumber = value; }
            get { return _RegNumber; }
        }

        public string RegPhone
        {
            set { _RegPhone = value; }
            get { return _RegPhone; }
        }

        public int NewsletterSubScription
        {
            set { _NewsletterSubScription = value; }
            get { return _NewsletterSubScription; }
        }

        public string SignatureEntityFile
        {
            set { _SignatureEntityFile = value; }
            get { return _SignatureEntityFile; }
        }

        public string VatNumber
        {
            set { _VatNumber = value; }
            get { return _VatNumber; }
        }

        public ContactPersonCreate WebAdministrator
        {
            get { return _WebAdministrator; }
            set { _WebAdministrator = value; }
        }

        public MailAddressCreate MailAddress
        {
            get { return _MailAddress; }
            set { _MailAddress = value; }
        }

        //public System.Collections.Generic.List<BankAccountReg> BankAccounts
        //{
        //    set { _bankAccounts = value; }
        //    get { return _bankAccounts; }
        //}

        /// <summary>
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            _CompanyCertificateFile = reader.ReadElementString();
            _CustomerId = reader.ReadElementString();
            _CustomerName = reader.ReadElementString();
            _DataAreaId = reader.ReadElementString();
            _InvoiceCity = reader.ReadElementString();
            _InvoiceCountry = reader.ReadElementString();
            _InvoiceCounty = reader.ReadElementString();
            _InvoiceEmail = reader.ReadElementString();
            _InvoiceFax = reader.ReadElementString();
            _InvoicePhone = reader.ReadElementString();
            _InvoicePostCode = reader.ReadElementString();
            _InvoiceStreet = reader.ReadElementString();
            _RecId = Int64.Parse(reader.ReadElementString());
            _RegEmail = reader.ReadElementString();
            _RegName = reader.ReadElementString();
            _RegNumber = reader.ReadElementString();
            _RegPhone = reader.ReadElementString();
            _NewsletterSubScription = int.Parse(reader.ReadElementString());
            _SignatureEntityFile = reader.ReadElementString();
            _VatNumber = reader.ReadElementString();
            //_WebAdministrator
            //_MailAddress
            //_bankAccounts
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("CompanyCertificateFile", _CompanyCertificateFile);
            writer.WriteElementString("CustomerId", _CustomerId);
            writer.WriteElementString("CustomerName", _CustomerName);
            writer.WriteElementString("DataAreaId", _DataAreaId);
            writer.WriteElementString("InvoiceCity", _InvoiceCity);
            writer.WriteElementString("InvoiceCountry", _InvoiceCountry);
            writer.WriteElementString("InvoiceCounty", _InvoiceCounty);
            writer.WriteElementString("InvoiceEmail", _InvoiceEmail);
            writer.WriteElementString("InvoiceFax", _InvoiceFax);
            writer.WriteElementString("InvoicePhone", _InvoicePhone);
            writer.WriteElementString("InvoicePostCode", _InvoicePostCode);
            writer.WriteElementString("InvoiceStreet", _InvoiceStreet);
            writer.WriteElementString("Method", Convert.ToString(_Method));
            writer.WriteElementString("RecId", Convert.ToString(_RecId));
            writer.WriteElementString("RegEmail", _RegEmail);
            writer.WriteElementString("RegName", _RegName);
            writer.WriteElementString("RegNumber", _RegNumber);
            writer.WriteElementString("RegPhone", _RegPhone);
            writer.WriteElementString("NewsletterSubScription", Convert.ToString(_NewsletterSubScription));
            writer.WriteElementString("SignatureEntityFile", _SignatureEntityFile);
            writer.WriteElementString("VatNumber", _VatNumber);
            writer.WriteStartElement("WebAdministrator");
            _WebAdministrator.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteStartElement("MailAddress");
            _MailAddress.WriteXml(writer);
            writer.WriteEndElement();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }
    }
}
