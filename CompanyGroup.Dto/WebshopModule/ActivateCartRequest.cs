using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class ActivateCartRequest
    {
        public ActivateCartRequest() : this(0, String.Empty, String.Empty) { }

        public ActivateCartRequest(int cartId, string language, string visitorId)
        { 
            CartId = cartId;

            Language = language;

            VisitorId = visitorId;
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
    }
}
