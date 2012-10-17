using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "InvoiceLineInfo", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class InvoiceLineInfo
    {
        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ItemDate", Order = 1)]
        public string ItemDate { set; get; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ItemId", Order = 2)] 
        public string ItemId { set; get; }

        /// <summary>
        /// terméknév
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 3)] 
        public string Name { set; get; }

        /// <summary>
        /// ár
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SalesPrice", Order = 4)] 
        public string SalesPrice { set; get; }

        /// <summary>
        /// valutanem
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CurrencyCode", Order = 5)] 
        public string CurrencyCode { set; get; }

        /// <summary>
        /// mennyiség
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Quantity", Order = 6)] 
        public int Quantity { set; get; }

        /// <summary>
        /// sor összesen
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LineAmount", Order = 7)]
        public string LineAmount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "DeliveryType", Order = 8)] 
        public int DeliveryType { set; get; }

        /// <summary>
        /// adótartalom
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "TaxAmount", Order = 9)]
        public string TaxAmount { set; get; }

    }
}
