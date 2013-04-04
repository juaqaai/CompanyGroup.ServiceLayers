using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    [System.Xml.Serialization.XmlRoot(ElementName = "SalesOrderResponse", Namespace = "http://CompanyGroup.Domain.WebshopModule/SalesOrderResponse")]
    public class SalesOrderCreateResult : System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// megrendelés válaszüzenet
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        public SalesOrderCreateResult(int code, string message)
        {
            this.Code = code;

            this.Message = message;
        }

        public SalesOrderCreateResult() : this(0, String.Empty) { }



        public string Message { get; set; }

        public string SalesId { get; set; }

        public int Code { get; set; }

        public string DataAreaId { get; set; }

		//<?xml version=\"1.0\" encoding=\"utf-16\"?><SalesOrderResponse xmlns=\"http://CompanyGroup.Domain.WebshopModule/SalesOrderResponse\">
        //<SalesId>VR636087</SalesId>
        //<Code>1</Code>
        //<Message>The createSalesOrder process completed successfully!</Message>
        //<DataAreaId>hrp</DataAreaId>
        //</SalesOrderResponse>"
        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SalesId", this.SalesId);
            writer.WriteElementString("Code", String.Format("{0}", this.Code));
            writer.WriteElementString("Message", this.Message);
            writer.WriteElementString("DataAreaId", this.DataAreaId);
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

            this.SalesId = reader.ReadElementString();
            this.Code = reader.ReadElementContentAsInt();
            this.Message = reader.ReadElementString();
            this.DataAreaId = reader.ReadElementString();
        }
    }
}
