using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// látogatóhoz tartozó aktív kosár törlése 
    /// </summary>
    public class RemoveCart
    {
        public RemoveCart() : this("", "", "") { }

        public RemoveCart(string cartId, string language, string visitorId)
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
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
