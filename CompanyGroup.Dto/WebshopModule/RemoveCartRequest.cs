using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// látogatóhoz tartozó aktív kosár törlése 
    /// </summary>
    public class RemoveCartRequest
    {
        public RemoveCartRequest() : this(0, "", "", "") { }

        public RemoveCartRequest(int cartId, string language, string visitorId, string currency)
        {
            this.CartId = cartId;

            this.Language = language;

            this.VisitorId = visitorId;

            this.Currency = currency;
        }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// választott nyelv
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
