using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// SalesId	    InvoiceDate	            DueDate	                InvoiceAmount	InvoiceCredit	InvoiceId	Payment	        SalesType	CusomerRef	InvoicingName	InvoicingAddress	        ContactPersonId	Printed	VisszaruId	ItemDate	            LineNum	ItemId	    Name	                                                            Quantity	SalesPrice	LineAmount	QuantityPhysical	Remain	DeliveryType	TaxAmount	LineAmountMst	TaxAmountMst
    /// VR605656	2012-10-04 00:00:00.000	2012-11-03 00:00:00.000	193345	        193345	        HI043793/12	Átutalás 30 nap	3		                SERCO KFT.	    1037 Budapest Bécsi út 314.		            1		            2012-10-04 00:00:00.000	0	    ESPP400-9	Fujitsu Esprimo P400 PC, Intel Core i5-2320, 2GB, 500GB, W7Prof.	1	        132230	    132230	    1	                0	    0	            35702	    132230	        35702
    /// </summary>
    public class InvoiceInfo : CompanyGroup.Domain.Core.NoSqlEntity
    {
        public InvoiceInfo(string customerId, string dataAreaId, string salesId, DateTime invoiceDate, DateTime dueDate, decimal invoiceAmount, decimal invoiceCredit, string currencyCode, string invoiceId, string payment, InvoiceSalesType salesType, 
                           string cusomerRef, string invoicingName, string invoicingAddress, string contactPersonId, bool printed, string returnItemId)
        {
            this.CustomerId = customerId;
            this.DataAreaId = dataAreaId;
            this.SalesId = salesId;
            this.InvoiceDate = invoiceDate;
            this.DueDate = dueDate;
            this.InvoiceAmount = invoiceAmount;
            this.InvoiceCredit = invoiceCredit;
            this.CurrencyCode = currencyCode;
            this.InvoiceId = invoiceId;
            this.Payment = payment;
            this.SalesType = salesType;
            this.CusomerRef = cusomerRef;
            this.InvoicingName = invoicingName;
            this.InvoicingAddress = invoicingAddress;
            this.ContactPersonId = contactPersonId;
            this.Printed = printed;
            this.ReturnItemId = returnItemId;
            this.Lines = new List<InvoiceLineInfo>();
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CustomerId", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string CustomerId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("DataAreaId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string DataAreaId { set; get; }

        /// <summary>
        /// VR, vagy BR
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SalesId", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceDate", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]       
        public DateTime InvoiceDate { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("DueDate", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime DueDate { set; get; }

        /// <summary>
        /// szémla végösszege
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceAmount", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public decimal InvoiceAmount { set; get; }

        /// <summary>
        /// számlán lévő tartozás
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceCredit", Order = 8)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public decimal InvoiceCredit { set; get; }

        /// <summary>
        /// pénznem
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CurrencyCode", Order = 9)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string CurrencyCode { set; get; }

        /// <summary>
        /// számla azonosító
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceId", Order = 10)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string InvoiceId { set; get; }

        /// <summary>
        /// fizetési feltétel
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Payment", Order = 11)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string Payment { set; get; }

        /// <summary>
        /// számla típusa (0 napló, 1 árajánlat, 2 előfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SalesType", Order = 12)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        public InvoiceSalesType SalesType { set; get; }

        /// <summary>
        /// vevői hivatkozás
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CusomerRef", Order = 13)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string CusomerRef { set; get; }

        /// <summary>
        /// számlán szereplő név
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoicingName", Order = 14)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string InvoicingName { set; get; }

        /// <summary>
        /// számlázási cím
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoicingAddress", Order = 15)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string InvoicingAddress { set; get; }

        /// <summary>
        /// kapcsolattartó
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ContactPersonId", Order = 16)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string ContactPersonId { set; get; }

        /// <summary>
        /// nyomtatva
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Printed", Order = 17)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(false)]
        public bool Printed { set; get; }

        /// <summary>
        /// visszáru 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ReturnItemId", Order = 18)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string ReturnItemId { set; get; }

        /// <summary>
        /// számla sorok 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public List<InvoiceLineInfo> Lines
        {
            get { return this.InvoiceLineInfoArray.ToList(); }
            set { this.InvoiceLineInfoArray = value.ToArray(); }
        }

        /// <summary>
        /// számla sorok 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceLineInfoArray", Order = 19)]
        public InvoiceLineInfo[] InvoiceLineInfoArray { set; get; }

        /// <summary>
        /// rendelés sor hozzáadása
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(InvoiceLineInfo line)
        {
            this.Lines.Add(line);
        }

        /// <summary>
        /// létrehoz egy rendelés info-t tételeivel együtt  
        /// </summary>
        /// <param name="lineInfos"></param>
        /// <returns></returns>
        public static InvoiceInfo Create(List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> lineInfos)
        {
            InvoiceInfo invoiceInfo = null;

            List<InvoiceLineInfo> invoiceLineInfoList = new List<InvoiceLineInfo>();

            lineInfos.ForEach(x =>
            {
                if (invoiceInfo == null)
                {
                    invoiceInfo = new InvoiceInfo(x.CustomerId, x.DataAreaId, x.SalesId,x.InvoiceDate,x.DueDate, x.InvoiceAmount, x.InvoiceCredit, x.CurrencyCode, x.InvoiceId, x.Payment, x.SalesType, 
                                                  x.CusomerRef, x.InvoicingName, x.InvoicingAddress, x.ContactPersonId, x.Printed, x.ReturnItemId);
                }

                InvoiceLineInfo invoiceLineInfo = new InvoiceLineInfo(x.ItemDate, x.LineNum, x.ItemId, x.Name, x.Quantity, x.SalesPrice, x.LineAmount,
                                                                      x.QuantityPhysical, x.Remain, x.DeliveryType, x.TaxAmount, x.LineAmountMst, x.TaxAmountMst, x.DetailCurrencyCode);

                invoiceLineInfoList.Add(invoiceLineInfo);
            });

            if (invoiceInfo != null)
            {
                invoiceInfo.InvoiceLineInfoArray = invoiceLineInfoList.ToArray();
            }

            return invoiceInfo;
        }

    }
}
