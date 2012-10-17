using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// beszerzési rendelés sor
    /// </summary>
    public class PurchaseOrderLine
    {
        /// <summary>
        /// beszerzési rendelés sor konstruktor
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="purchQty"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="confirmedDlv"></param>
        /// <param name="qtyOrdered"></param>
        /// <param name="remainInventPhysical"></param>
        /// <param name="remainPurchPhysical"></param>
        /// <param name="dataAreaId"></param>
        public PurchaseOrderLine(string itemId, int purchQty, DateTime deliveryDate, DateTime confirmedDlv,	int qtyOrdered,	int remainInventPhysical, int remainPurchPhysical, string dataAreaId)
        {
            this.ItemId = itemId;
            this.PurchQty = purchQty;
            this.DeliveryDate = deliveryDate;
            this.ConfirmedDlv = confirmedDlv;
            this.QtyOrdered = qtyOrdered;	
            this.RemainInventPhysical = remainInventPhysical;
            this.RemainPurchPhysical = remainPurchPhysical;
            this.DataAreaId = dataAreaId;
        }

        public string ItemId { get; set; }

        public int PurchQty{ get; set; }

        public DateTime DeliveryDate{ get; set; }

        public DateTime ConfirmedDlv{ get; set; }

        public int QtyOrdered{ get; set; }

        public int RemainInventPhysical{ get; set; }

        public int RemainPurchPhysical{ get; set; }

        public string DataAreaId { get; set; }
    }
}
