using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class ActivateCart
    {
        public ActivateCart() : this(String.Empty, String.Empty, String.Empty) { }

        public ActivateCart(string cartId, string language, string visitorId)
        { 
            CartId = cartId;

            Language = language;

            VisitorId = visitorId;
        }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public string CartId { get; set; }

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
