using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class ActivateCartRequest
    {
        public ActivateCartRequest() : this(0, String.Empty, String.Empty, String.Empty) { }

        public ActivateCartRequest(int cartId, string language, string visitorId, string currency)
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
