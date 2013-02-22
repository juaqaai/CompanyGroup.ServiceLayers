using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class InvoiceLineInfo
    {
        /// <summary>
        /// szállítást ekkorra kérte
        /// </summary>
        public string ItemDate { set; get; }

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
        public string LineAmount { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int DeliveryType { set; get; }

        /// <summary>
        /// adótartalom
        /// </summary>
        public string TaxAmount { set; get; }

        /// <summary>
        /// termékleírás
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// kép kikereséshez szükséges rekord azonosító
        /// </summary>
        public long RecId { set; get; }
    }
}
