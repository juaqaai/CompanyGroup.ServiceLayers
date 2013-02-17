using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class OrderLineInfo
    {
        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>     
        public DateTime ShippingDateRequested { set; get; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ItemId { set; get; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// ár
        /// </summary>
        public string SalesPrice { set; get; }

        /// <summary>
        /// valutanem
        /// </summary>
        public string CurrencyCode { set; get; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { set; get; }

        /// <summary>
        /// sor összesen
        /// </summary>
        public decimal LineAmount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int SalesDeliverNow { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int RemainSalesPhysical { set; get; }

        /// <summary>
        /// 4 ReservPhysical (foglalt tényleges), 5 ReservOrdered (foglalt rendelt), 6 OnOrder (rendelés alatt)
        /// </summary>
        public int StatusIssue { set; get; }

        /// <summary>
        /// raktárkód
        /// </summary>
        public string InventLocationId { set; get; }

        /// <summary>
        /// fizetési mód
        /// </summary>
        public string Payment { set; get; }
    }
}
