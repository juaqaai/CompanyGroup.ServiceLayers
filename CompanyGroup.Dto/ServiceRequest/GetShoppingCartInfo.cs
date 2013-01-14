using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class GetShoppingCartInfo
    {
        public GetShoppingCartInfo() : this(0, "") { }

        public GetShoppingCartInfo(int cartId, string visitorId)
        {
            this.CartId = cartId;

            this.VisitorId = visitorId;
        }

        /// <summary>
        /// kosar azonosítója
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
