using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "OrderLineInfo", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class OrderLineInfo
    {
        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ShippingDateRequested", Order = 1)]        
        public DateTime ShippingDateRequested { set; get; }

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
        public decimal LineAmount { set; get; }

        /// <summary>
        /// 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt)
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "StatusIssue", Order = 8)] 
        public int StatusIssue { set; get; }

        /// <summary>
        /// raktárkód
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InventLocationId", Order = 9)]
        public string InventLocationId { set; get; }

        /// <summary>
        /// fizetési mód
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Payment", Order = 10)]
        public string Payment { set; get; }
    }
}
