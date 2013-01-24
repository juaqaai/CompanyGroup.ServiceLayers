using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetShoppingCartInfoRequest
    {
        public GetShoppingCartInfoRequest() : this(0, "") { }

        public GetShoppingCartInfoRequest(int cartId, string visitorId)
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
