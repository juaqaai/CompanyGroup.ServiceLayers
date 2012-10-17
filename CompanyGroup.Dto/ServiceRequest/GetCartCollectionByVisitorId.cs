using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetShoppingCartInfo
    {
        public GetShoppingCartInfo() : this("", "") { }

        public GetShoppingCartInfo(string cartId, string visitorId)
        {
            this.CartId = cartId;

            this.VisitorId = visitorId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string CartId { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
