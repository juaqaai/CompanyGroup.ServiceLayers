using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// kosár törlés művelethez tartozó kérés objektum
    /// </summary>
    public class RemoveLineRequest
    {
        public RemoveLineRequest() : this(0, 0, String.Empty, String.Empty, String.Empty) { }

        public RemoveLineRequest(int cartId, int lineId, string language, string visitorId, string currency)
        {
            this.CartId = cartId;

            this.LineId = lineId;

            this.Language = language;

            this.VisitorId = visitorId;

            this.Currency = currency;
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
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
