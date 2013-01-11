using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// látogatóhoz tartozó aktív kosár törlése 
    /// </summary>
    public class RemoveCart
    {
        public RemoveCart() : this(0, "", "") { }

        public RemoveCart(int cartId, string language, string visitorId)
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
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
