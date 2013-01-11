using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kosár törlés művelethez tartozó kérés objektum
    /// </summary>
    public class RemoveLine
    {
        public RemoveLine() : this(0, 0, String.Empty, String.Empty) { }

        public RemoveLine(int cartId, int lineId, string language, string visitorId)
        {
            this.CartId = cartId;

            this.LineId = lineId;

            this.Language = language;

            this.VisitorId = visitorId;
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
    }
}
