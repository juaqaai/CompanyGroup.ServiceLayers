using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class OrderLineInfo
    {
        public int Id { set; get; }

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
        /// sor brutto összesen
        /// </summary>
        public decimal LineAmountBrutto { set; get; }

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

        public bool StatusIssueIsReservPhysical
        {
            get { return this.StatusIssue.Equals(4); }
            set {}
        }

        public bool StatusIssueIsReservOrdered 
        { 
            get { return this.StatusIssue.Equals(5); }
            set {}
        }

        public bool StatusIssueIsOnOrder 
        { 
            get { return this.StatusIssue.Equals(6); }
            set {}
        }

        /// <summary>
        /// van-e készleten?
        /// </summary>
        public bool InStock { set; get; }

        /// <summary>
        /// elérhető-e a webshop-ban?
        /// </summary>
        public bool AvailableInWebShop { set; get; }

        /// <summary>
        /// létezik-e hozzá kép?
        /// </summary>
        public bool PictureExists { set; get; }
    }
}
