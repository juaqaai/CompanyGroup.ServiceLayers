using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "InvoiceInfo", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class InvoiceInfo
    {
        /// <summary>
        /// VR, vagy BR azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SalesId", Order = 1)]
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceDate", Order = 2)]
        public string InvoiceDate { set; get; }

        /// <summary>
        /// bizonylat lejárati ideje
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "DueDate", Order = 3)]
        public string DueDate { set; get; }

        /// <summary>
        /// számla végösszege
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceAmount", Order = 4)]
        public string InvoiceAmount { set; get; }

        /// <summary>
        /// számlán lévő tartozás
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceCredit", Order = 5)]
        public string InvoiceCredit { set; get; }

        /// <summary>
        /// pénznem
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CurrencyCode", Order = 6)]
        public string CurrencyCode { set; get; }

        /// <summary>
        /// számla azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoiceId", Order = 7)]
        public string InvoiceId { set; get; }

        /// <summary>
        /// fizetési feltétel
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Payment", Order = 8)]
        public string Payment { set; get; }

        /// <summary>
        /// számla típusa (0 napló, 1 árajánlat, 2 előfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet)
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SalesType", Order = 9)]
        public int SalesType { set; get; }

        /// <summary>
        /// vevői hivatkozás
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CusomerRef", Order = 10)]
        public string CusomerRef { set; get; }

        /// <summary>
        /// számlán szereplő név
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoicingName", Order = 11)]
        public string InvoicingName { set; get; }

        /// <summary>
        /// számlázási cím
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InvoicingAddress", Order = 12)]
        public string InvoicingAddress { set; get; }

        /// <summary>
        /// kapcsolattartó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ContactPersonId", Order = 13)]
        public string ContactPersonId { set; get; }

        /// <summary>
        /// nyomtatva
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Printed", Order = 14)]
        public bool Printed { set; get; }

        /// <summary>
        /// visszáru 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ReturnItemId", Order = 15)]
        public string ReturnItemId { set; get; }

        /// <summary>
        /// sorok
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Lines", Order = 16)]
        public List<InvoiceLineInfo> Lines { get; set; }
    }
}
