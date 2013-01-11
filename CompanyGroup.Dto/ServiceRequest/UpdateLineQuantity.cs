using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kosár műveletekhez tartozó kérés objektum
    /// </summary>
    public class UpdateLineQuantity
    {
        public UpdateLineQuantity() : this(0, 0, String.Empty, String.Empty, 0, String.Empty) { }

        public UpdateLineQuantity(int cartId, int lineId, string language, string dataAreaId, int quantity, string visitorId)
        { 
            CartId = cartId;
            LineId = lineId;
            Language = language;
            Quantity = quantity;
            VisitorId = visitorId;
        }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// sor azonosító
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// beállított nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
